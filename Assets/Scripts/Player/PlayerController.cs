using System;
using Unity.Cinemachine;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
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

    void Awake()
    {
        // get local components if they don't exist
        if (!player_rb)
        {
            player_rb = GetComponent<Rigidbody>();
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
        Vector3 cam_xz_pos = new Vector3(free_cam.transform.position.x, transform.position.y, free_cam.transform.position.z);
        cam_look_dir = (transform.position - cam_xz_pos).normalized;

        if (move_dir != Vector2.zero)
        {
            Vector3 rotate_to_dir = forward_accel * cam_look_dir + sideways_accel * (Quaternion.Euler(0,90,0) * cam_look_dir).normalized;
            true_look_dir = Vector3.Slerp(true_look_dir, rotate_to_dir, Time.deltaTime * body_rotate_speed);
        }
        Debug.DrawLine(transform.position, transform.position + true_look_dir * 2);

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
}
