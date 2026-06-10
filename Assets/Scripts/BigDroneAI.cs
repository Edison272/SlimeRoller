using UnityEngine;

public class BigDroneAI : MonoBehaviour
{
    public enum DroneState { Patrol, Attack, Activated, Pursuing, Deactivated }
    public DroneState currentState = DroneState.Deactivated;
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
    [SerializeField] private float attack_delay = 2.5f;

    [SerializeField] private BoltShooter drone_shooter;
    [SerializeField] private float hover_height = 3.0f;
    [SerializeField] private float patrol_speed = 3.5f;
    [SerializeField] private float activation_move_time = 5f;
    [SerializeField] private float activation_move_speed = 0.5f;

    private float attack_delay_timer = 0f;
    private UnityEngine.AI.NavMeshAgent agent;
    private int currentWaypoint = 0;
    private float visionDistance;
    private float visionAngle;
    private bool isActivated = false; // Track if drone has been activated
    private float activation_z_target = 0f;
    private float activation_move_timer = 0f;
    private Vector3 deactivatedPosition;

    // Stun variables
    private bool isStunned = false;
    private float stun_timer = 0f;
    private float stun_duration;

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
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = patrol_speed;
        agent.isStopped = true;

        deactivatedPosition = transform.position;
        currentState = DroneState.Deactivated;

        if (waypoints != null && waypoints.Length > 0)
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
        if (isStunned)
        {
            ChangeAmbientSound(droneSound);

            // Hover
            Vector3 hoverPosition = transform.position;
            hoverPosition.y = hover_height;
            transform.position = hoverPosition;

            // Stun logic
            stun_timer += Time.deltaTime;
            if (stun_timer >= stun_duration)
            {
                isStunned = false;
                agent.isStopped = false;
                if (!isActivated)
                {
                    currentState = DroneState.Deactivated;
                }
                else
                {
                    currentState = DroneState.Pursuing;
                }
            }
            return;
        }

        switch (currentState)
        {
            // Patrol State
            case DroneState.Patrol:
                GetComponent<Renderer>().material = patrolMaterial;
                visionCone.color = Color.yellow;
                SetDroneLights(patrolLightColor);
                Patrol();
                break;
            // Deactivated State - waits until the door opens and activation is triggered
            case DroneState.Deactivated:
                GetComponent<Renderer>().material = patrolMaterial;
                visionCone.color = Color.yellow;
                SetDroneLights(patrolLightColor);
                Deactivated();
                break;
            // Attack State
            case DroneState.Attack:
                GetComponent<Renderer>().material = attackMaterial;
                visionCone.color = Color.red;
                SetDroneLights(Color.red);
                Attack();
                break;
            // Activated State - Moving along Z axis
            case DroneState.Activated:
                GetComponent<Renderer>().material = attackMaterial;
                visionCone.color = Color.red;
                SetDroneLights(Color.red);
                ActivationMovement();
                break;
            // Pursuing State - Permanently follow player
            case DroneState.Pursuing:
                GetComponent<Renderer>().material = attackMaterial;
                visionCone.color = Color.red;
                SetDroneLights(Color.red);
                PermanentPursuit();
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

    void Deactivated()
    {
        // Hold position and stay dormant until activation is triggered.
        Vector3 hoverPosition = transform.position;
        hoverPosition.y = hover_height;
        transform.position = hoverPosition;

        if (agent != null)
        {
            agent.ResetPath();
            agent.isStopped = true;
        }
    }

    void Patrol()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            currentState = DroneState.Pursuing;
            return;
        }

        // Hover
        Vector3 hoverPosition = transform.position;
        hoverPosition.y = hover_height;
        transform.position = hoverPosition;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypoint].position);
        }

        if (InVisionCone())
        {
            agent.ResetPath();
            ChangeAmbientSound(alarmSound);
            SetDroneLights(Color.red);
            attack_delay_timer = 0f;
            currentState = DroneState.Attack;
        }
    }

    void Attack()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            currentState = DroneState.Pursuing;
            return;
        }

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

    public void Stun(float duration)
    {
        isStunned = true;
        stun_duration = duration;
        stun_timer = 0f;
        agent.isStopped = true;

        visionCone.color = Color.blue;
        SetDroneLights(Color.blue);
    }

    public void ActivateDrone()
    {
        if (!isActivated)
        {
            isActivated = true;

            agent.ResetPath();
            agent.isStopped = true;
            currentState = DroneState.Activated;
            activation_z_target = player != null ? player.position.z : transform.position.z;
            activation_move_timer = 0f;
        }
    }

    void ActivationMovement()
    {
        // Hover at current height
        Vector3 hoverPosition = transform.position;
        hoverPosition.y = hover_height;
        transform.position = hoverPosition;

        // Move along Z axis toward the player before switching to pursuit.
        activation_move_timer += Time.deltaTime;

        if (activation_move_timer < activation_move_time)
        {
            if (audioSource2.clip != alarmSound)
            {
                ChangeAmbientSound(alarmSound);
            }

            float direction = Mathf.Sign(activation_z_target - transform.position.z);
            if (direction == 0f)
            {
                direction = 1f;
            }

            float zDelta = activation_move_speed * Time.deltaTime;
            float maxMove = Mathf.Min(zDelta, Mathf.Abs(activation_z_target - transform.position.z));

            Vector3 rayDirection = direction > 0f ? Vector3.forward : Vector3.back;
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up * 0.25f, rayDirection, out hit, maxMove + 0.2f, drone_shooter != null ? drone_shooter.collision_mask : default))
            {
                maxMove = Mathf.Max(0f, hit.distance - 0.15f);
            }

            float newZ = transform.position.z + maxMove * direction;
            Vector3 moveDirection = transform.position;
            moveDirection.z = newZ;
            moveDirection.y = hover_height;
            transform.position = moveDirection;
            return;
        }

        transform.position = new Vector3(transform.position.x, hover_height, activation_z_target);

        attack_delay_timer = 0f;
        attack_time = Time.time;

        agent.isStopped = false;
        agent.speed = patrol_speed;

        currentState = DroneState.Pursuing;
    }

    void PermanentPursuit()
    {
        // Hover at current height
        Vector3 hoverPosition = transform.position;
        hoverPosition.y = hover_height;
        transform.position = hoverPosition;

        // Use NavMesh pathing so the drone can route around walls when following the player.
        if (agent != null)
        {
            agent.isStopped = false;
            agent.speed = patrol_speed;
            agent.SetDestination(player.position);
        }

        // Always face the player
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

        // Continuously shoot at the player
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
}