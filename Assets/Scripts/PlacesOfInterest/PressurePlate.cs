using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

/// <summary>
/// To use, attach whatever objects that should be toggled by this pressure plate into objectList
/// </summary>
public class PressurePlate : MonoBehaviour
{
    // The list of objects that this pressure plate controls
    // Technically works with different GameObjects, but would need a standardized method to call between the types
    [SerializeField] private List<Toggleable> objectList;

    public Material plateMaterial;

    private void Start()
    {
        plateMaterial = GetComponent<Renderer>().material;
        // Disable the OnTriggerColliders for all of these objects
        foreach (Toggleable d in objectList)
        {
            d.ToggleDisable();
        }
        plateMaterial.color = Color.red;
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (Toggleable d in objectList)
        {
            d.ToggleOn();
        }
        plateMaterial.color = Color.green;
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (Toggleable d in objectList)
        {
            d.ToggleOff();
        }
        plateMaterial.color = Color.red;
    }
}
