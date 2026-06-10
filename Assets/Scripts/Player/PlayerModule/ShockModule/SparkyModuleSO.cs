using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "ShockModule", menuName = "PlayerModules/ShockModule")]
/* 
This is the template class for Player Module Objects. 
This is where the game will instantiate the class, and get object instances for
The ability's UI, pickups, and other stuff.
*/
public class ShockModuleSO : PlayerModuleSO
{
    public float cooldown_time = 2;
    public float shock_radius = 5;
    public LayerMask shock_mask;
    public GameObject shockwave_prefab;
    public float stun_time = 3f;
    public AudioClip activationSound;

    // shock module is SINGLE USE. we should refactor this later
    public int max_uses = 1;
    public PlayerModuleSO default_module; 

    // create module class instance, 
    public override PlayerModule CreateModuleData(
        PlayerController player, 
        GameObject ui_holder
        )
    {
        ShockModule shock_module = new ShockModule(player, this);
        
        // set up ui instance
        GameObject ui_instance = MonoBehaviour.Instantiate(ui_element, ui_holder.transform);
        ShockModuleUI module_ui = ui_instance.GetComponent<ShockModuleUI>();
        module_ui.module_instance = shock_module;
        
        // store UI reference in module for later deactivation
        shock_module.SetUIInstance(ui_instance);
        
        return shock_module;
    }

    public override PlayerModule CreateModuleData(
        PlayerController player
        )
    {
        return new ShockModule(player, this);
    }

    public override void CreateNewUI(
        PlayerModule player_module,
        GameObject ui_holder
        )
    {
        GameObject ui_instance = MonoBehaviour.Instantiate(ui_element, ui_holder.transform);
        ShockModuleUI module_ui = ui_instance.GetComponent<ShockModuleUI>();
        module_ui.module_instance = (ShockModule)player_module;
        
        // store UI reference in module for later deactivation
        player_module.SetUIInstance(ui_instance);
    }
}