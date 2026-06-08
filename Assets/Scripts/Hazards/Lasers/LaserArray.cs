using UnityEngine;
using UnityEditor;

public class LaserArray : MonoBehaviour
{
    public LineRenderer lineRenderer;
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
            Transform prev_node = transform.GetChild(i-1);
            Transform curr_node = transform.GetChild(i);

            Debug.DrawLine(prev_node.position, curr_node.position, Color.green);
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
