using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "JumpModule", menuName = "PlayerModules/JumpModule")]
/* 
This is the template class for Player Module Objects. 
This is where the game will instantiate the class, and get object instances for
The ability's UI, pickups, and other stuff.
*/
public class JumpModuleSO : PlayerModuleSO
{
    public float max_charge_time = 2;
    public float jump_scale = 60;
    public float base_jump_amt = 60;

    // create module class instance, 
    public override PlayerModule CreateModuleData(
        PlayerController player, 
        GameObject ui_holder
        )
    {
        JumpModule jump_module = new JumpModule(player, this);
        
        // set up ui instance
        GameObject ui_instance = MonoBehaviour.Instantiate(ui_element, ui_holder.transform);
        JumpModuleUI module_ui = ui_instance.GetComponent<JumpModuleUI>();
        module_ui.module_instance = jump_module;
        
        return jump_module;
    }

    public override PlayerModule CreateModuleData(
        PlayerController player
        )
    {
        return new JumpModule(player, this);
    }

    public override void CreateNewUI(
        PlayerModule player_module,
        GameObject ui_holder
        )
    {
        GameObject ui_instance = MonoBehaviour.Instantiate(ui_element, ui_holder.transform);
        JumpModuleUI module_ui = ui_instance.GetComponent<JumpModuleUI>();
        module_ui.module_instance = (JumpModule)player_module;
    }
}