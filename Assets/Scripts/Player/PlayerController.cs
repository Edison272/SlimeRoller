using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState {Dead, OnGround, Jumping}
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
    [SerializeField] float base_speed = 10;
    [SerializeField] float curr_speed;
    public PlayerState player_state = PlayerState.OnGround;

    // Looking values
    Vector3 cam_look_dir = Vector3.zero;
    float body_rotate_speed = 7;
    Vector3 true_look_dir = Vector3.right;

    // Physics
    [SerializeField] Rigidbody player_rb;

    // Player Components
    public GameObject SlimeMembrane;
    public GameObject SlimeCore;
    public ParticleSystem DeathParticles;

    // all parts of the slime VFX are to be kept track of
    public GameObject[] AllVFX;

    // Player Module
    public PlayerModuleType module_type;

    void Awake()
    {
        // get local components if they don't exist
        if (!player_rb) {player_rb = GetComponent<Rigidbody>();}

        // set player input stuff
        if (!player_input) {player_input = GetComponent<PlayerInput>();}
        player_ability = player_input.actions["Ability"];

        // set player ability module
        switch (module_type) {
            case PlayerModuleType.JUMP:
                PlayerModule new_module = new JumpModule(this);
                break;
        }
        curr_speed = base_speed;

        true_look_dir = SlimeCore.transform.forward;
        player_state = PlayerState.OnGround;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
            Vector3 rotate_to_dir = forward_accel * cam_look_dir + sideways_accel * (Quaternion.Euler(0,90,0) * cam_look_dir).normalized;
            true_look_dir = Vector3.Slerp(true_look_dir, rotate_to_dir, Time.deltaTime * body_rotate_speed);
        }
        //SlimeCore.transform.forward = true_look_dir;
    }

    void FixedUpdate()
    {
        player_rb.AddForce(cam_look_dir * forward_accel + sideways_accel * (Quaternion.Euler(0,90,0) * cam_look_dir).normalized);
    
        // check if player has fallen off the map
        if (player_state != PlayerState.Dead && player_rb.position.y < -10)
        {
            OnDeath();
        }
    }

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

    #region Death
    void OnReset(InputValue action)
    {
        // player can manually reset (by dying)
        OnDeath(true);
    }

    public void OnDeath(bool intentional = false)
    {
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
        DeathParticles.Play();
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        // return to previous checkpoint
        yield return new WaitForSeconds(1);
        Transform checkpoint = Checkpoint.set_respawn_transform;
        player_rb.MovePosition(checkpoint.position);
        player_state = PlayerState.OnGround;
        DeathParticles.Play();
        ToggleVFX(true);
    }
    #endregion

    #region Physics
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
