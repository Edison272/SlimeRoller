using UnityEngine;

public class audio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource.clip = sound;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
