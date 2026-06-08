using UnityEngine;

public class AbilityCollectible : MonoBehaviour
{
    [Tooltip("The module ScriptableObject that will be given to the player on pickup")]
    public PlayerModuleSO moduleSO;

    [Tooltip("Destroy this pickup after it is collected")]
    public bool destroyOnPickup = true;

    void Reset()
    {
        // ensure collider is trigger for ease of use
        Collider c = GetComponent<Collider>();
        if (c) c.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // try to find a PlayerController on the entering object or its parents
        PlayerController pc = other.GetComponentInParent<PlayerController>();
        if (pc == null) return;

        if (moduleSO != null)
        {
            pc.SetModule(moduleSO);
        }

        if (destroyOnPickup)
        {
            Destroy(gameObject);
        }
    }
}
