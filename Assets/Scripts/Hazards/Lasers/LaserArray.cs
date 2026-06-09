using UnityEngine;
using UnityEditor;

public class LaserArray : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public AudioSource audioSource;
    public AudioClip laserSound;
    [SerializeField] private LayerMask ignoreMask;
    [SerializeField] private int numBounces = 0;

    private bool laserEnabled = true; // Variable to hold the laser distance so it can be re-enabled

    void Awake()
    {
        if(!lineRenderer)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer.positionCount = numBounces + 2;
        audioSource.clip = laserSound;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.positionCount = 0;
        if (laserEnabled)
        {
            Vector3 initialPos = transform.GetChild(0).position;
            Vector3 initialDir = (transform.GetChild(1).position - transform.GetChild(0).position).normalized;
            float initialDist = Vector3.Distance(transform.GetChild(0).position, transform.GetChild(1).position);
            lineRenderer.positionCount = numBounces + 2;
            UpdateReflection(0, initialDist, initialPos, initialDir);
        }
    }
    private void UpdateReflection(int reflectionCount, float distance, Vector3 collision, Vector3 dir)
    {
        Ray r = new(collision, dir);
        RaycastHit output;
        if (reflectionCount <= numBounces)
        {
            if (Physics.Raycast(r, out output, distance, ~ignoreMask))
            {
                // Draw a line starting from the previous point to the collision point
                lineRenderer.SetPosition(reflectionCount, collision);
                lineRenderer.SetPosition(reflectionCount + 1, output.point);

                // Check if we're hitting a reflective surface
                Renderer rend = output.collider.GetComponent<Renderer>();
                reflectionCount += 1;
                if (rend != null)
                {
                    // Using the material Eyes for now as a 'reflective surface'
                    // Do we want to use materials to determine whether they can reflect?  Or something else
                    if (rend.material.name.Contains("ChromeMembrane") && reflectionCount <= numBounces)
                    {
                        distance -= output.distance;
                        Debug.Log(Vector3.Reflect(dir.normalized, output.normal).normalized);
                        UpdateReflection(reflectionCount, distance, output.point, Vector3.Reflect(dir.normalized, output.normal).normalized);
                        return;
                    }
                    // Maybe make it an else if statement
                    if (output.collider.TryGetComponent(out PlayerController target) && !rend.material.name.Contains("ChromeMembrane"))
                    {
                        // Call the kill player function here
                        target.OnDeath();
                        //}
                    }
                    return;
                }
            }

            // If not, set the line to the rest
            lineRenderer.SetPosition(reflectionCount, collision);
            lineRenderer.SetPosition(reflectionCount + 1, collision + dir.normalized * distance);
            lineRenderer.positionCount = reflectionCount + 2;
        }
    }

    public void SetupLineRenderer()
    {
        lineRenderer.positionCount = transform.childCount;
        Vector3[] positions = new Vector3[transform.childCount];
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            positions[i] = transform.GetChild(i).position;
        }
        lineRenderer.SetPositions(positions);
    }

    public void OnDrawGizmos() {
        for (int i = 1; i < transform.childCount; i++)
        {
            Transform prev_node = transform.GetChild(i-1);
            Transform curr_node = transform.GetChild(i);

            Debug.DrawLine(prev_node.position, curr_node.position, Color.green);
        }
        if (lineRenderer.positionCount != transform.childCount)
        {
        SetupLineRenderer();
        }
    }
}
