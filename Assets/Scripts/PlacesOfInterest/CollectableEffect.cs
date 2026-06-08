using UnityEngine;

public class CollectableEffect : MonoBehaviour
{
    [SerializeField] private float SpinSpeed = 90f;

    [SerializeField] private float FloatAmplitude = 0.25f;
    [SerializeField] private float FloatFrequency = 1f;
    [SerializeField] private bool UseLocalPosition = true;

    private Vector3 initialPosition;
    private float startTime;

    void Start()
    {
        initialPosition = UseLocalPosition ? transform.localPosition : transform.position;
        startTime = Time.time;
    }

    void Update()
    {
        // spin
        transform.Rotate(Vector3.up, SpinSpeed * Time.deltaTime, Space.Self);

        // float/bounce
        float phase = (Time.time - startTime) * FloatFrequency * Mathf.PI * 2f;
        float yOffset = Mathf.Sin(phase) * FloatAmplitude;
        Vector3 offset = Vector3.up * yOffset;

        if (UseLocalPosition)
            transform.localPosition = initialPosition + offset;
        else
            transform.position = initialPosition + offset;
    }

    void OnValidate()
    {
        if (FloatFrequency < 0f) FloatFrequency = 0f;
        if (FloatAmplitude < 0f) FloatAmplitude = 0f;
    }

    void Reset()
    {
        initialPosition = UseLocalPosition ? transform.localPosition : transform.position;
    }
}
