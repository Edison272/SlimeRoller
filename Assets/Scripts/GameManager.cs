using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private float volume = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            ApplyAudioVolume();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyAudioVolume();
    }

    public float Volume
    {
        get => volume;
        set
        {
            volume = Mathf.Clamp01(value);
            ApplyAudioVolume();
        }
    }

    private void ApplyAudioVolume()
    {
        AudioListener.volume = volume;
    }

    public int HighestLevel = 0;

    public void BeatLevel(int levelIndex)
    {
        if (levelIndex > HighestLevel)
        {
            HighestLevel = levelIndex;
        }
    }
}
