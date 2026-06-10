using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject homePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;

    public void OnPlayButtonPressed()
    {
        SceneManager.LoadScene("Hub");
    }

    public void OnSettingsButtonPressed()
    {
        homePanel.SetActive(false);
        creditsPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OnCreditsButtonPressed()
    {
        homePanel.SetActive(false);
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void OnBackButtonPressed()
    {
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        homePanel.SetActive(true);
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }
}
