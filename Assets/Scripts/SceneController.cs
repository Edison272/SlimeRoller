using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private string HubSceneName = "Hub";
    [SerializeField] private string LoadingSceneName = "LoadingScreen";
    [SerializeField] private float MinLoadingScreenTime = 5f;

    public void LoadLevel(string levelName)
    {
        SceneTransitionData.SetNextLevel(levelName);
        StartCoroutine(LoadSceneTransition(levelName));
    }

    // loads a selected scene, while showing a loading screen for a fixed amount of time.
    private IEnumerator LoadSceneTransition(string sceneName)
    {
        float Timer = Time.time;

        yield return SceneManager.LoadSceneAsync(LoadingSceneName, LoadSceneMode.Additive);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (Time.time - Timer < MinLoadingScreenTime || asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        asyncLoad.allowSceneActivation = true;

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        yield return SceneManager.UnloadSceneAsync(LoadingSceneName);
        SceneTransitionData.Clear();
    }

    public void LoadHub()
    {
        SceneTransitionData.SetNextLevel(HubSceneName);
        StartCoroutine(LoadSceneTransition(HubSceneName));
    }
}
