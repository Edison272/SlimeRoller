using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "GravityModule", menuName = "PlayerModules/GravityModule")]
/* 
This is the template class for Player Module Objects. 
This is where the game will instantiate the class, and get object instances for
The ability's UI, pickups, and other stuff.
*/
public class GravityModuleSO : PlayerModuleSO
{
    public float pullRadius = 10;
    public float pullStrength = 20;
    public float orbitRadius = 3;
    public float orbitStrength = 20;
    public float liftStrength = 5;

    // create module class instance, 
    public override PlayerModule CreateModuleData(
        PlayerController player, 
        GameObject ui_holder
        )
    {
        GravityModule gravity_module = new GravityModule(player, this);
        
        // set up ui instance
        GameObject ui_instance = MonoBehaviour.Instantiate(ui_element, ui_holder.transform);
        GravityModuleUI module_ui = ui_instance.GetComponent<GravityModuleUI>();
        module_ui.module_instance = gravity_module;
        
        return gravity_module;
    }

    public override PlayerModule CreateModuleData(
        PlayerController player
        )
    {
        return new GravityModule(player, this);
    }
    public override void CreateNewUI(
        PlayerModule player_module,
        GameObject ui_holder
        )
    {
        GameObject ui_instance = MonoBehaviour.Instantiate(ui_element, ui_holder.transform);
        GravityModuleUI module_ui = ui_instance.GetComponent<GravityModuleUI>();
        module_ui.module_instance = (GravityModule)player_module;
    }
}
