using UnityEngine;
using System.Collections;

public class AbilityCollectible : MonoBehaviour
{
    [Tooltip("The module ScriptableObject that will be given to the player on pickup")]
    public PlayerModuleSO moduleSO;

    [Tooltip("Destroy this pickup after it is collected")]
    public bool destroyOnPickup = true;

    [Tooltip("Time in seconds before this collectible respawns")]
    public float respawnTime = 10f;

    [Tooltip("Enable respawning for this collectible")]
    public bool enableRespawn = false;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private bool isActive = true;
    private Coroutine respawnCoroutine;
    private Collider collectibleCollider;
    private Renderer collectibleRenderer;
    private GameObject[] childObjects;

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        collectibleCollider = GetComponent<Collider>();
        collectibleRenderer = GetComponent<Renderer>();
        
        // Cache all child objects for respawning
        childObjects = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            childObjects[i] = transform.GetChild(i).gameObject;
        }
    }

    void Reset()
    {
        // ensure collider is trigger for ease of use
        Collider c = GetComponent<Collider>();
        if (c) c.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;

        // try to find a PlayerController on the entering object or its parents
        PlayerController pc = other.GetComponentInParent<PlayerController>();
        if (pc == null) return;

        if (moduleSO != null)
        {
            pc.SetModule(moduleSO);
        }

        if (destroyOnPickup)
        {
            if (enableRespawn)
            {
                CollectAndRespawn();
            }
            else
            {
                // Destroy all children first
                foreach (GameObject child in childObjects)
                {
                    if (child != null)
                        Destroy(child);
                }
                Destroy(gameObject);
            }
        }
    }

    private void CollectAndRespawn()
    {
        isActive = false;

        if (respawnCoroutine != null)
        {
            StopCoroutine(respawnCoroutine);
        }
        
        // Disable collider and renderer instead of the GameObject so coroutine keeps running
        if (collectibleCollider != null)
            collectibleCollider.enabled = false;
        if (collectibleRenderer != null)
            collectibleRenderer.enabled = false;
        
        // Disable all child objects
        foreach (GameObject child in childObjects)
        {
            if (child != null)
                child.SetActive(false);
        }
        
        respawnCoroutine = StartCoroutine(RespawnAfterDelay());
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnTime);
        
        // Reset position and rotation
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        
        // Re-enable collider and renderer
        if (collectibleCollider != null)
            collectibleCollider.enabled = true;
        if (collectibleRenderer != null)
            collectibleRenderer.enabled = true;
        
        // Re-enable all child objects
        foreach (GameObject child in childObjects)
        {
            if (child != null)
                child.SetActive(true);
        }
        
        isActive = true;
    }
}
