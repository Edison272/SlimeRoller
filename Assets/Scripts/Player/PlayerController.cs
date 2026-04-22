using System;
using Unity.Cinemachine;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

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

    // Looking values
    Vector3 cam_look_dir = Vector3.zero;
    float body_rotate_speed = 7;
    Vector3 true_look_dir = Vector3.zero;

    // Physics
    [SerializeField] Rigidbody player_rb;

    // Player Components
    public GameObject SlimeMembrane;
    public GameObject SlimeCore;

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

        if (move_dir != Vector2.zero)
        {
            Vector3 rotate_to_dir = forward_accel * cam_look_dir + sideways_accel * (Quaternion.Euler(0,90,0) * cam_look_dir).normalized;
            true_look_dir = Vector3.Slerp(true_look_dir, rotate_to_dir, Time.deltaTime * body_rotate_speed);
        }
        SlimeCore.transform.forward = true_look_dir;
    }

    void FixedUpdate()
    {
        player_rb.AddForce(cam_look_dir * forward_accel + sideways_accel * (Quaternion.Euler(0,90,0) * cam_look_dir).normalized);
    }

    void OnMove(InputValue action)
    {
        move_dir = action.Get<Vector2>();
        forward_accel = move_dir.y * base_speed;
        sideways_accel = move_dir.x * base_speed;
    }

    void OnAbility(InputValue action)
    {
        
    }

    #region Physics
    // This is called automatically when a collision begins
    private void OnCollisionEnter(Collision collision)
    {
        // collision.gameObject gives access to the object you hit
        Debug.Log("Collided with: " + collision.gameObject.tag);
    }

    #endregion
}
