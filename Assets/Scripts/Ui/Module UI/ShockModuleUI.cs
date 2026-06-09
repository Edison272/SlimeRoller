using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShockModuleUI : MonoBehaviour
{
    public ShockModule module_instance;

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
        float shock_perc = module_instance.GetShockRechargePerc();
        ProgressBar.transform.localScale = new Vector3(shock_perc,1,1);
        if (shock_perc < 1)
        {
            BarText.text = "Recharging...";
        }
        else
        {
            BarText.text = string.Format("Shockwave Ready! - Uses: {0}", module_instance.uses);
        }
        
    }
}
