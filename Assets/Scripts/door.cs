using UnityEngine;

public class Door : Toggleable
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
    public AudioClip closeSound;

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

        SetDoorClosedState();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorOpen = true;
            SetDoorOpenState();
            if (openSound != null)
            {
                audioSource.PlayOneShot(openSound);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorOpen = false;
            SetDoorClosedState();
            if (closeSound != null)
            {
                audioSource.PlayOneShot(closeSound);
            }
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
        else
        {
            leftDoor.transform.position = Vector3.MoveTowards(
                leftDoor.transform.position,
                leftClosedPos,
                openSpeed * Time.deltaTime);

            rightDoor.transform.position = Vector3.MoveTowards(
                rightDoor.transform.position,
                rightClosedPos,
                openSpeed * Time.deltaTime);
        }
    }

    void SetDoorOpenState()
    {
        status.material = motion;
        light1.color = Color.green;
        light2.color = Color.green;
    }

    void SetDoorClosedState()
    {
        status.material = noMotion;
        light1.color = Color.red;
        light2.color = Color.red;
    }

    public override void ToggleOn()
    {
        doorOpen = true;
        SetDoorOpenState();
        if (openSound != null)
        {
            audioSource.PlayOneShot(openSound);
        }
    }

    public override void ToggleOff()
    {
        doorOpen = false;
        SetDoorClosedState();
        if (closeSound != null)
        {
            audioSource.PlayOneShot(closeSound);
        }
    }

    public override void ToggleDisable()
    {
        GetComponent<BoxCollider>().enabled = false;
    }
}