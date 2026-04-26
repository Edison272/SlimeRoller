using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // where to set the precise player position and rotation after respawning
    public Transform RespawnTransform;
    public static Transform set_respawn_transform;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Checkpoint.SetCheckpoint(this);
        }
    }

    public static void SetCheckpoint(Checkpoint new_checkpoint)
    {
        set_respawn_transform = new_checkpoint.transform;
    }
}
