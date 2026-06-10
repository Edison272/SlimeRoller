using UnityEngine;

public class HubPuck : MonoBehaviour
{
    [SerializeField] private int levelIndex;
    private SceneController sceneController;
    private bool playerHasEntered = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sceneController = SceneController.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("thing hit the level puck");
        if ((other.CompareTag("Player") || other.CompareTag("ShadowPlayer")) && !playerHasEntered)
        {
            //Debug.Log("Player hit the level puck");
            GameManager.Instance.BeatLevel(levelIndex);
            sceneController.LoadHub();
            playerHasEntered = true;
        }
    }
}
