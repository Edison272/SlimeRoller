using UnityEngine;

public class LevelUnlocks : MonoBehaviour
{
    [SerializeField] private GameObject gameObjectToUnlock;
    [SerializeField] private int levelIndexToUnlock;
    [SerializeField] private bool activityUponUnlock = false;
    [SerializeField] private bool isDoor = false;

    private void Start()
    {
        if (!isDoor)
        {
            if (GameManager.Instance.HighestLevel >= levelIndexToUnlock)
            {
                gameObjectToUnlock.SetActive(activityUponUnlock);
            }
            else
            {
                gameObjectToUnlock.SetActive(!activityUponUnlock);
            }
        }
        if (isDoor)
        {
            if (GameManager.Instance.HighestLevel >= levelIndexToUnlock)
            {
                gameObjectToUnlock.GetComponent<Door>().enabled = true;
                gameObjectToUnlock.GetComponent<Collider>().enabled = true;
                gameObjectToUnlock.GetComponent<Door>().ToggleOn();
            }
            else
            {
                gameObjectToUnlock.GetComponent<Door>().enabled = false;
                gameObjectToUnlock.GetComponent<Collider>().enabled = false;
                gameObjectToUnlock.GetComponent<Door>().ToggleOff();
            }
        }
    }
}
