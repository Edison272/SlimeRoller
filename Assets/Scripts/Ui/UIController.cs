using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    [field: SerializeField]
    public Canvas MainCanvas { get; private set; }

    private void Awake()
    {
        MainCanvas = this.GetComponent<Canvas>();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
