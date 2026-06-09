using UnityEngine;
using TMPro;

public class ShadowModuleUI : MonoBehaviour
{
    public ShadowModule module_instance;

    // ui components
    public GameObject DurationBar;
    public TextMeshProUGUI DurationText;
    public GameObject ChargeBar;
    public TextMeshProUGUI ChargeText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChargeBar.transform.localScale = new Vector3(module_instance.GetShadowChargePerc(),1,1);
        ChargeText.text = Mathf.RoundToInt(module_instance.GetShadowChargePerc() * 100).ToString() + "%";
        DurationBar.transform.localScale = new Vector3(module_instance.GetShadowDurationPerc(),1,1);
        DurationText.text = Mathf.RoundToInt(module_instance.GetShadowDurationInSec()).ToString() + "s";
    }
}
