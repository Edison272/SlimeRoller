using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "ChromeModule", menuName = "PlayerModules/ChromeModule")]
/* 
This is the template class for Player Module Objects. 
This is where the game will instantiate the class, and get object instances for
The ability's UI, pickups, and other stuff.
*/
public class ChromeModuleSO : PlayerModuleSO
{

    // create module class instance, 
    public override PlayerModule CreateModuleData(
        PlayerController player, 
        GameObject ui_holder
        )
    {
        ChromeModule chrome_module = new ChromeModule(player, this);
        
        // set up ui instance
        GameObject ui_instance = MonoBehaviour.Instantiate(ui_element, ui_holder.transform);
        ChromeModuleUI module_ui = ui_instance.GetComponent<ChromeModuleUI>();
        module_ui.module_instance = chrome_module;
        
        // store UI reference in module for later deactivation
        chrome_module.SetUIInstance(ui_instance);
        
        return chrome_module;
    }

    public override PlayerModule CreateModuleData(
        PlayerController player
        )
    {
        return new ChromeModule(player, this);
    }
    public override void CreateNewUI(
        PlayerModule player_module,
        GameObject ui_holder
        )
    {
        GameObject ui_instance = MonoBehaviour.Instantiate(ui_element, ui_holder.transform);
        ChromeModuleUI module_ui = ui_instance.GetComponent<ChromeModuleUI>();
        module_ui.module_instance = (ChromeModule)player_module;
        
        // store UI reference in module for later deactivation
        player_module.SetUIInstance(ui_instance);
    }
}
