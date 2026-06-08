using UnityEngine;
using UnityEditor;

public class LaserArray : MonoBehaviour
{
    public LineRenderer lineRenderer;
    [SerializeField] private LayerMask ignoreMask;
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
        SetupLineRenderer();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            // if (Physics.Raycast(r, out output, distance, ~ignoreMask))
            // {
            //     // Draw a line starting from the previous point to the collision point
            //     lineRenderer.SetPosition(reflectionCount, collision);
            //     lineRenderer.SetPosition(reflectionCount + 1, output.point);

            //     // Check if we're hitting a reflective surface
            //     Renderer rend = output.collider.GetComponent<Renderer>();
            //     reflectionCount += 1;
            //     if (rend != null)
            //     {
            //         // Using the material Eyes for now as a 'reflective surface'
            //         // Do we want to use materials to determine whether they can reflect?  Or something else
            //         if (rend.material.name.Contains("Eyes") && reflectionCount <= numBounces)
            //         {
            //             distance -= output.distance;
            //             Debug.Log(Vector3.Reflect(dir.normalized, output.normal).normalized);
            //             UpdateReflection(reflectionCount, distance, output.point, Vector3.Reflect(dir.normalized, output.normal).normalized);
            //             return;
            //         }
            //         // Maybe make it an else if statement
            //         if (output.collider.TryGetComponent(out PlayerController target))
            //         {
            //             // Call the kill player function here
            //         }
            //     }
            //     return;
            // }
        }
    }

    public void SetupLineRenderer()
    {
        lineRenderer.positionCount = transform.childCount;
        Vector3[] positions = new Vector3[transform.childCount];
        for (int i = 0; i < transform.childCount-1; i++)
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
