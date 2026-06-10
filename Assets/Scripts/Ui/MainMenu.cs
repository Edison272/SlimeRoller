using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject homePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;

    [SerializeField] private TextMeshProUGUI volumeValueText;
    public AudioSource audioSource;
    public AudioClip hover;
    public AudioClip click;

    public void OnPlayButtonPressed()
    {
        PlayClickSound();
        StartCoroutine(LoadHubAfterSound());
    }

    private IEnumerator LoadHubAfterSound()
    {
        yield return new WaitForSeconds(0.6f);
        SceneManager.LoadScene("Hub");
    }

    public void OnSettingsButtonPressed()
    {
        PlayClickSound();
        homePanel.SetActive(false);
        creditsPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OnCreditsButtonPressed()
    {
        PlayClickSound();
        homePanel.SetActive(false);
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void OnBackButtonPressed()
    {
        PlayClickSound();
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        homePanel.SetActive(true);
    }

    public void OnQuitButtonPressed()
    {
        PlayClickSound();
        Application.Quit();
    }

    public void OnVolumeChanged(float value)
    {
        GameManager.Instance.Volume = value;
        volumeValueText.text = Mathf.RoundToInt(value * 100).ToString();
    }

    private void PlayClickSound()
    {
        if (audioSource != null && click != null)
        {
            audioSource.PlayOneShot(click);
        }
    }

    public void PlayHoverSound()
    {
        if (audioSource != null && hover != null)
        {
            audioSource.PlayOneShot(hover);
        }
    }
}
