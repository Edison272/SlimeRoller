using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

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

    [SerializeField] private GameObject pauseMenuPanel;

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame && pauseMenuPanel != null)
        {
            TogglePauseMenu();
        }
    }

    private void TogglePauseMenu()
    {
        if (pauseMenuPanel.activeSelf)
        {
            pauseMenuPanel.SetActive(false);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            pauseMenuPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
    }

    public void HomeButtonPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void ResetButtonPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
