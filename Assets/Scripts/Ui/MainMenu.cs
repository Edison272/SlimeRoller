using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject homePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;

    [SerializeField] private TextMeshProUGUI volumeValueText;

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

    public void OnVolumeChanged(float value)
    {
        GameManager.Instance.Volume = value;
        volumeValueText.text = Mathf.RoundToInt(value * 100).ToString();
    }

}
