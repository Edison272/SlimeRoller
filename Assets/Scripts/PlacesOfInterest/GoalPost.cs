using UnityEngine;

public class GoallPost : MonoBehaviour
{
    // where to take the player after they've reached the goal zone
    public Checkpoint new_spawn_point;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            EventBus.Singleton.LevelComplete?.Invoke();

            // kill player and respawn them at the new level
            Checkpoint.SetCheckpoint(new_spawn_point);
            other.GetComponent<PlayerController>().OnDeath(true);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
