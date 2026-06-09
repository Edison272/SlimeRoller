using UnityEngine;
using UnityEngine.AI;

public class SecurityCameraAI : MonoBehaviour
{
    public enum CameraState { Scanning, Attack}
    public CameraState currentState = CameraState.Scanning;
    public Light visionCone;
    public Transform eyePoint;
    public Transform player;
    public GameObject laserPrefab;

    // manage attack speed
    [SerializeField] private float attack_speed = 0.1f;
    [SerializeField] private float attack_time = 0;
    [SerializeField] private float attack_delay = 1f;

    [SerializeField] private BoltShooter drone_shooter;
    [SerializeField] private float rotateAngle = 60f;
    [SerializeField] private float rotateSpeed = 30f;
    [SerializeField] private float pauseDuration = 2f;

    private float attack_delay_timer = 0f;
    private float pause_timer = 2f;
    private Quaternion initialRotation;
    private float currentAngle = 0f;
    private int rotateDirection = 1;
    private float visionDistance;
    private float visionAngle;

    // audio
    public AudioSource audioSource;
    public AudioClip shootSound;
    public AudioSource audioSource2;
    public AudioClip droneSound;
    public AudioClip alarmSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Store initial rotation
        initialRotation = transform.localRotation;

        // Vision Cone Setup
        visionDistance = visionCone.range;
        visionAngle = visionCone.spotAngle;
        visionCone.color = Color.yellow;

        // Play drone sound
        audioSource2.clip = droneSound;
        audioSource2.loop = true;
        audioSource2.spatialBlend = 1f;
        audioSource2.Play();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            // Scanning State
            case CameraState.Scanning:
                visionCone.color = Color.yellow;
                Scanning();
                break;
            // Attack State
            case CameraState.Attack:
                visionCone.color = Color.red;
                Attack();
                break;
        }
    }

    // Checks if player is in line of sight
    bool InVisionCone()
    {
        Vector3 directionToPlayer = player.position - eyePoint.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > visionDistance)
        {
            return false;
        }

        float angleToPlayer = Vector3.Angle(eyePoint.forward, directionToPlayer);
        if (angleToPlayer > visionAngle / 2f)
        {
            return false;
        }

        if (Physics.Raycast(eyePoint.position, directionToPlayer.normalized, out RaycastHit hit, visionDistance, drone_shooter.collision_mask))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    void Scanning()
    {
        if (InVisionCone())
        {
            attack_delay_timer = 0f;
            currentState = CameraState.Attack;
            return;
        }

        if (pause_timer < pauseDuration) 
        {
            pause_timer += Time.deltaTime;
            return;
        }

        currentAngle += rotateDirection * rotateSpeed * Time.deltaTime;

        if (currentAngle >= rotateAngle)
        {
            currentAngle = rotateAngle;
            rotateDirection = -1;
            pause_timer = 0f;
        }
        else if (currentAngle <= -rotateAngle)
        {
            currentAngle = -rotateAngle;
            rotateDirection = 1;
            pause_timer = 0f;
        }

        transform.localRotation = initialRotation * Quaternion.Euler(0f, currentAngle, 0f);
    }

    void Attack()
    {
        if (!InVisionCone())
        {
            ChangeAmbientSound(droneSound);
            currentState = CameraState.Scanning;
            return;
        }

        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0;

        Vector3 centerForward = initialRotation * Vector3.forward;
        float angleToPlayer = Vector3.SignedAngle(centerForward, directionToPlayer, Vector3.up);
        float clampedAngle = Mathf.Clamp(angleToPlayer, -rotateAngle, rotateAngle);

        Quaternion targetRotation = initialRotation * Quaternion.Euler(0f, clampedAngle, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

        currentAngle = clampedAngle;

        if (attack_delay_timer < attack_delay)
        {
            attack_delay_timer += Time.deltaTime;
            return;
        }

        if (Time.time >= attack_time)
        {
            Shoot();
            attack_time = Time.time + attack_speed;
        }
    }

    void Shoot()
    {
        // shoot a laser towards the player
        Vector3 directionToPlayer = player.position - eyePoint.position;
        Quaternion rotation = Quaternion.LookRotation(directionToPlayer.normalized);
        drone_shooter.ShootLaser(directionToPlayer, eyePoint.position);
        audioSource.PlayOneShot(shootSound);
    }

    void ChangeAmbientSound(AudioClip clip)
    {
        if (audioSource2.clip == clip)
            return;

        audioSource2.Stop();
        audioSource2.clip = clip;
        audioSource2.loop = true;
        audioSource2.Play();
    }
}
