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
    private int objectsOnPlate = 0;
    public Material plateMaterial;
    public AudioSource audioSource;
    public AudioClip on;
    public AudioClip off;

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
        objectsOnPlate++;
        if (objectsOnPlate == 1)
        {
            foreach (Toggleable d in objectList)
            {
                d.ToggleOn();
            }

            plateMaterial.color = Color.green;
            audioSource.PlayOneShot(on);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        objectsOnPlate--;

        if (objectsOnPlate <= 0)
        {
            objectsOnPlate = 0;

            foreach (Toggleable d in objectList)
            {
                d.ToggleOff();
            }

            plateMaterial.color = Color.red;
            audioSource.PlayOneShot(off);
        }
    }
}
