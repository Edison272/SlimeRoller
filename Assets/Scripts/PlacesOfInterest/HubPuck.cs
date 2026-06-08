using UnityEngine;

public class HubPuck : MonoBehaviour
{
    [SerializeField] private SceneController sceneController;
    private bool playerHasEntered = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (sceneController == null)
        {
            sceneController = FindFirstObjectByType<SceneController>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("thing hit the level puck");
        if (other.CompareTag("Player") && !playerHasEntered)
        {
            //Debug.Log("Player hit the level puck");
            sceneController.LoadHub();
            playerHasEntered = true;
        }
    }
}
