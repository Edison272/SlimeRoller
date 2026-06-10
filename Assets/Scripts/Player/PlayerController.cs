using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState { Dead, OnGround, Jumping }
public class PlayerController : MonoBehaviour
{
    // Input System
    public PlayerInput player_input;
    public InputAction player_ability;

    // Camera Control
    public Camera main_camera;
    public CinemachineCamera free_cam;

    // Movement values
    Vector2 move_dir = Vector2.zero;
    float forward_accel = 0;
    float sideways_accel = 0;
    [SerializeField] float base_speed = 100;
    [SerializeField] float curr_speed;
    public PlayerState player_state = PlayerState.OnGround;
    private LayerMask ground_check_mask = 1 << 6;

    // Looking values
    Vector3 cam_look_dir = Vector3.zero;
    float body_rotate_speed = 7;
    Vector3 true_look_dir = Vector3.right;

    // Physics
    [SerializeField] Rigidbody player_rb;

    // Player Components
    public GameObject SlimeMembrane;
    public GameObject SlimeCore;
    public Light SlimeLight;
    public ParticleSystem DeathParticles;

    // all parts of the slime VFX are to be kept track of
    public GameObject[] AllVFX;

    // Player Module
    public PlayerModule active_module;
    private PlayerModuleSO module_so;
    public PlayerModuleSO starting_module;

    // Player States - accessed by external scripts
    public bool on_ground { get; private set; } = false;
    public AudioSource audioSource;
    public AudioSource loopSource;
    public AudioClip death;

    void Awake()
    {
        // get local components if they don't exist
        if (!player_rb) { player_rb = GetComponent<Rigidbody>(); }

        // set player input stuff
        if (!player_input) { player_input = GetComponent<PlayerInput>(); }

        if (!audioSource && !loopSource)
        {
            AudioSource source = GetComponent<AudioSource>();
            audioSource = source;
            loopSource = source;
        }
        else if (!audioSource)
        {
            audioSource = loopSource;
        }
        else if (!loopSource)
        {
            loopSource = audioSource;
        }

        player_ability = player_input.actions["Ability"];

        curr_speed = base_speed;

        true_look_dir = SlimeCore.transform.forward;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // set player ability starting module
        SetModule(starting_module);
        // disable default cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // move player to face camera direction
        Vector3 cam_xz_pos = new Vector3(free_cam.transform.position.x, transform.position.y, free_cam.transform.position.z);
        cam_look_dir = (transform.position - cam_xz_pos).normalized;

        // rotate inner core to face the movement direction
        if (move_dir != Vector2.zero)
        {
            Vector3 rotate_to_dir = forward_accel * cam_look_dir + sideways_accel * (Quaternion.Euler(0, 90, 0) * cam_look_dir).normalized;
            true_look_dir = Vector3.Slerp(true_look_dir, rotate_to_dir, Time.deltaTime * body_rotate_speed);
        }
        if (true_look_dir != Vector3.zero)
        {
            SlimeCore.transform.forward = true_look_dir;
        }

        active_module.UpdateModule();
    }

    // allow external objects (pickups, etc.) to switch the player's active module
    public void SetModule(PlayerModuleSO newModuleSO)
    {
        if (newModuleSO == null) return;

        // deactivate current module if present
        if (active_module != null)
        {
            active_module.OnDeactivate();
        }

        PlayerModule new_module = null;
        if (UIController.Instance)
        {
            new_module = newModuleSO.CreateModuleData(this, UIController.Instance.MainCanvas.gameObject);
        }
        else
        {
            new_module = newModuleSO.CreateModuleData(this);
        }


        // replace active module and store reference to the SO
        active_module = new_module;
        module_so = newModuleSO;

        // set VFX
        SlimeMembrane.GetComponent<MeshRenderer>().material = module_so.membrane_material;
        SlimeCore.GetComponent<MeshRenderer>().material = module_so.core_material;
        SlimeLight.color = module_so.slime_glow;
    }

    void FixedUpdate()
    {
        // constantly set the player's movement based on the accel values
        player_rb.AddForce(cam_look_dir * forward_accel + sideways_accel * (Quaternion.Euler(0, 90, 0) * cam_look_dir).normalized);

        // check if player is on the ground
        RaycastHit ground_touch;
        on_ground = Physics.Raycast(
            transform.position,
            Vector3.down,
            out ground_touch,
            transform.localScale.y / 1.9f,
            ground_check_mask
            );
        if (on_ground)
        {
            Debug.DrawLine(transform.position, ground_touch.point, Color.green);
        }
        else
        {
            Debug.DrawLine(transform.position, transform.position + Vector3.down * (transform.localScale.y / 1.9f), Color.red);
        }

        // check if player has fallen off the map
        if (player_state != PlayerState.Dead && player_rb.position.y < -10)
        {
            OnDeath();
        }

        // update ability if necessary
        active_module.FixedUpdateModule();

    }

    // update the accel values on input
    void OnMove(InputValue action)
    {
        if (player_state != PlayerState.Dead)
        {
            move_dir = action.Get<Vector2>();
            forward_accel = move_dir.y * curr_speed;
            sideways_accel = move_dir.x * curr_speed;
        }
    }

    void OnAbility(InputValue action)
    {
        //active_module.UseModule(action);
    }
    // hide anypart of the player which is visible
    void ToggleVFX(bool is_on)
    {
        SlimeMembrane.GetComponent<MeshRenderer>().enabled = is_on;
        foreach (GameObject vfx in AllVFX)
        {
            vfx.SetActive(is_on);
        }
    }

    #region Getting and Setting Stats
    public void SetCurrentSpeed(float speed_modifier)
    {
        curr_speed = base_speed * speed_modifier;
    }
    #endregion

    #region Death
    void OnReset(InputValue action)
    {
        // player can manually reset (by dying)
        OnDeath(true);
    }

    public void OnDeath(bool intentional = false)
    {
        if (player_state == PlayerState.Dead)
        {
            return;
        }
        // reset movement
        player_rb.linearVelocity = Vector3.zero;
        forward_accel = 0;
        sideways_accel = 0;

        //die
        player_state = PlayerState.Dead;
        if (!intentional)
        {
            EventBus.Singleton.PlayerDeath?.Invoke();
        }
        ToggleVFX(false);
        audioSource.PlayOneShot(death);
        DeathParticles.Play();
        StartCoroutine(Respawn());

        SetModule(starting_module);
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1);
        Transform checkpoint = Checkpoint.set_respawn_transform;
        player_rb.MovePosition(checkpoint.transform.position);
        yield return new WaitForSeconds(1);
        player_state = PlayerState.OnGround;
        DeathParticles.Play();
        ToggleVFX(true);
    }
    #endregion

    #region Physics
    // used to add physics impulse forces to the player from the ability modules
    public void ApplyImpulse(Vector2 direction, float power)
    {
        player_rb.AddForce(direction * power, ForceMode.Impulse);

    }

    // This is called automatically when a collision begins
    private void OnCollisionEnter(Collision collision)
    {
        // collision.gameObject gives access to the object you hit
        Debug.Log("Collided with: " + collision.gameObject.tag);


        switch (collision.gameObject.tag)
        {
            case "Hazard":
                if (player_state != PlayerState.Dead)
                {
                    OnDeath();
                }
                break;
            case "Ramp":
                // double existing speed when on a ramp
                player_rb.linearVelocity *= 4;
                break;
        }
    }

    #endregion
}
