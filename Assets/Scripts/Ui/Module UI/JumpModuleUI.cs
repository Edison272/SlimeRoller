using UnityEngine;

public class JumpModuleUI : MonoBehaviour
{
    public JumpModule module_instance;

    // ui components
    public GameObject ProgressBar;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ProgressBar.transform.localScale = new Vector3(1,1,1);
    }

    // Update is called once per frame
    void Update()
    {
        ProgressBar.transform.localScale = new Vector3(module_instance.GetJumpChargePerc(),1,1);
    }
}
