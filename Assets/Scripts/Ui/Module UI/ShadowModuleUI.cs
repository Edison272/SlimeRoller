using UnityEngine;

public class ShadowModuleUI : MonoBehaviour
{
    public ShadowModule module_instance;

    // ui components
    public GameObject ProgressBar;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ProgressBar.transform.localScale = new Vector3(module_instance.GetShadowChargePerc(),1,1);
    }
}
