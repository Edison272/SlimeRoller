using UnityEngine;
using TMPro;

public class LoadingScreenUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelNameText;
    [SerializeField] private string fallbackLabel = "Loading...";

    private void Awake()
    {
        if (levelNameText == null)
        {
            levelNameText = GetComponent<TextMeshProUGUI>();
        }

        string nextLevelName = SceneTransitionData.NextLevelName;
        levelNameText.text = string.IsNullOrEmpty(nextLevelName)
            ? fallbackLabel
            : nextLevelName;
    }
}
