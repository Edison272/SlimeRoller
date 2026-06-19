using UnityEngine;
using System.Collections.Generic;

public class PressureSwitch : MonoBehaviour
{
    [SerializeField] private List<Toggleable> objectList;

    private Material plateMaterial;
    private bool isOn = false;
    public AudioSource audioSource;
    public AudioClip on;
    public AudioClip off;
    private void Start()
    {
        plateMaterial = GetComponent<Renderer>().material;
        foreach (Toggleable d in objectList)
        {
            d.ToggleDisable();
        }

        plateMaterial.color = Color.red;
        isOn = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        isOn = !isOn;
        foreach (Toggleable d in objectList)
        {
            if (isOn)
            {
                d.ToggleOn();
                audioSource.PlayOneShot(on);
            }
            else
            {
                d.ToggleOff();
                audioSource.PlayOneShot(off);
            }
        }

        plateMaterial.color = isOn ? Color.green : Color.red;
    }
}