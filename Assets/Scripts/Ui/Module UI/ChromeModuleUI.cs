using TMPro;
using UnityEngine;

public class ChromeModuleUI : MonoBehaviour
{
    public ChromeModule module_instance;

    public GameObject ProgressBar;
    public TextMeshProUGUI BarText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (module_instance.chromeActive)
        {
            ProgressBar.transform.localScale = new Vector3(1, 1, 1);
            BarText.text = "Chrome Active";
        }
        else
        {
            ProgressBar.transform.localScale = new Vector3(0, 1, 1);
            BarText.text = "Chrome Inactive";
        }
    }
}
