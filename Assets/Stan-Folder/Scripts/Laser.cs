using UnityEngine;
using UnityEngine.Events;

public class Laser : Toggleable
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float laserDistance = 8.0f;
    [SerializeField] private LayerMask ignoreMask;
    [SerializeField] private UnityEvent OnHitTarget;
    [SerializeField] private int numBounces = 0; // How many times can this laser reflect?

    private bool laserEnabled = true; // Variable to hold the laser distance so it can be re-enabled

    private RaycastHit rayHit;
    private Ray ray;

    private void Start()
    {
        lineRenderer.positionCount = numBounces + 2;
    }
    private void Update()
    {
        lineRenderer.positionCount = 0;
        if (laserEnabled)
        {
            lineRenderer.positionCount = numBounces + 2;
            UpdateReflection(0, laserDistance, transform.position, transform.forward);
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
                    if (rend.material.name.Contains("Eyes") && reflectionCount <= numBounces)
                    {
                        distance -= output.distance;
                        Debug.Log(Vector3.Reflect(dir.normalized, output.normal).normalized);
                        UpdateReflection(reflectionCount, distance, output.point, Vector3.Reflect(dir.normalized, output.normal).normalized);
                        return;
                    }
                    // Maybe make it an else if statement
                    if (output.collider.TryGetComponent(out PlayerController target))
                    {
                        // Call the kill player function here
                    }
                }
                return;
            }
        }

        // If not, set the line to the rest
        lineRenderer.SetPosition(reflectionCount, collision);
        lineRenderer.SetPosition(reflectionCount + 1, collision + dir.normalized * distance);
        lineRenderer.positionCount = reflectionCount + 2;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, ray.direction * laserDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(rayHit.point, 0.23f);
    }
<<<<<<< Updated upstream:Assets/Stan-Folder/Scripts/Laser.cs
}
=======

    // 'On' in this case would be disabling the laser
    public override void ToggleOn()
    {
        laserEnabled = false;
    }

    public override void ToggleOff()
    {
        laserEnabled = true;
    }
    public override void ToggleDisable()
    {
        laserEnabled = false;
    }
}
>>>>>>> Stashed changes:Assets/Scripts/Hazards/Lasers/Laser.cs
