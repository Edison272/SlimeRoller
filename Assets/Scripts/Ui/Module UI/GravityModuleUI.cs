using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GravityModuleUI : MonoBehaviour
{
    public GravityModule module_instance;

    // ui components
    public GameObject ProgressBar;
    public TextMeshProUGUI BarText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (module_instance.gravityActive)
        {
            int targets = module_instance.GetGravityTargets();
            ProgressBar.transform.localScale = new Vector3(1,1,1);
            BarText.text = string.Format("Affected Targets: {0}", targets);
        }
        else
        {
            ProgressBar.transform.localScale = new Vector3(0,1,1);
            BarText.text = "Gravity Inactive";
        }
    }
}
