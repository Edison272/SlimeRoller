using UnityEngine;
using UnityEngine.AI;

public class DroneAI : MonoBehaviour
{
    public enum DroneState { Patrol, Attack }
    public DroneState currentState = DroneState.Patrol;
    public Material patrolMaterial;
    public Material attackMaterial;
    public Transform[] waypoints;
    public Light visionCone;
    public Transform eyePoint;
    public Transform player;
    public GameObject laserPrefab;

    public Light[] droneLights;
    private Color patrolLightColor = new Color32(255, 148, 0, 255);

    // manage attack speed
    [SerializeField] private float attack_speed = 0.1f;
    [SerializeField] private float attack_time = 0;

    [SerializeField] private BoltShooter drone_shooter;
    [SerializeField] private float hover_height = 3.0f;
    private NavMeshAgent agent;
    private int currentWaypoint = 0;
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
        // NavMeshAgent setup and first destination
        agent = GetComponent<NavMeshAgent>();

        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypoint].position);
        }

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
            // Patrol State
            case DroneState.Patrol:
                GetComponent<Renderer>().material = patrolMaterial;
                visionCone.color = Color.yellow;
                SetDroneLights(patrolLightColor);
                Patrol();
                break;
            // Attack State
            case DroneState.Attack:
                GetComponent<Renderer>().material = attackMaterial;
                visionCone.color = Color.red;
                SetDroneLights(Color.red);
                Attack();
                break;
        }
    }

    void SetDroneLights(Color color)
    {
        foreach (Light light in droneLights)
        {
            if (light != null)
            {
                light.color = color;
            }
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

    void Patrol()
    {
        // Hover
        Vector3 hoverPosition = transform.position;
        hoverPosition.y = hover_height;
        transform.position = hoverPosition;
        
        if(!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypoint].position);
        }

        if (InVisionCone())
        {
            agent.ResetPath();
            ChangeAmbientSound(alarmSound);
            SetDroneLights(Color.red);
            currentState = DroneState.Attack;
        }
    }

    void Attack()
    {
        // Hover
        Vector3 hoverPosition = transform.position;
        hoverPosition.y = hover_height;
        transform.position = hoverPosition;

        if (!InVisionCone())
        {
            agent.SetDestination(waypoints[currentWaypoint].position);
            SetDroneLights(patrolLightColor);
            ChangeAmbientSound(droneSound);
            currentState = DroneState.Patrol;
            return;
        }

        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

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
