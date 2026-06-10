using UnityEngine;

public class Door2 : Toggleable
{
    private bool doorOpen = false;
    public bool zAxis = false;

    public GameObject leftDoor;
    public GameObject rightDoor;

    public float openDistance = 2f;
    public float openSpeed = 2f;

    public Renderer status;

    public Material noMotion;
    public Material motion;

    public Light light1;
    public Light light2;

    private Vector3 leftClosedPos;
    private Vector3 rightClosedPos;
    private Vector3 leftOpenPos;
    private Vector3 rightOpenPos;

    public AudioSource audioSource;
    public AudioClip openSound;
    [SerializeField] private BigDroneAI bigDrone;

    void Start()
    {
        leftClosedPos = leftDoor.transform.position;
        rightClosedPos = rightDoor.transform.position;

        if (zAxis)
        {
            leftOpenPos = leftClosedPos + new Vector3(0, 0, openDistance);
            rightOpenPos = rightClosedPos + new Vector3(0, 0, -openDistance);
        }
        else
        {
            leftOpenPos = leftClosedPos + new Vector3(-openDistance, 0, 0);
            rightOpenPos = rightClosedPos + new Vector3(openDistance, 0, 0);
        }

        status.material = noMotion;
        light1.color = Color.red;
        light2.color = Color.red;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !doorOpen)
        {
            doorOpen = true;
            SetDoorOpenState();

            if (openSound != null)
            {
                audioSource.PlayOneShot(openSound);
            }

            ActivateBigDrone();
        }
    }

    void Update()
    {
        if (doorOpen)
        {
            leftDoor.transform.position = Vector3.MoveTowards(
                leftDoor.transform.position,
                leftOpenPos,
                openSpeed * Time.deltaTime);

            rightDoor.transform.position = Vector3.MoveTowards(
                rightDoor.transform.position,
                rightOpenPos,
                openSpeed * Time.deltaTime);
        }
    }

    void SetDoorOpenState()
    {
        status.material = motion;
        light1.color = Color.green;
        light2.color = Color.green;
    }

    public override void ToggleOn()
    {
        if (doorOpen)
            return;

        doorOpen = true;
        SetDoorOpenState();

        if (openSound != null)
        {
            audioSource.PlayOneShot(openSound);
        }

        ActivateBigDrone();
    }

    private void ActivateBigDrone()
    {
        if (bigDrone == null)
        {
            bigDrone = FindFirstObjectByType<BigDroneAI>();
        }

        if (bigDrone != null)
        {
            bigDrone.ActivateDrone();
        }
    }

    public override void ToggleOff()
    {

    }

    public override void ToggleDisable()
    {
        GetComponent<BoxCollider>().enabled = false;
    }
}