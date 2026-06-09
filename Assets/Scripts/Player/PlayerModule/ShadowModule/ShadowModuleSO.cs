using UnityEngine;

[CreateAssetMenu(fileName = "ShadowModule", menuName = "PlayerModules/ShadowModule")]
/* 
This is the template class for Player Module Objects. 
This is where the game will instantiate the class, and get object instances for
The ability's UI, pickups, and other stuff.
*/
public class ShadowModuleSO : PlayerModuleSO
{
    public float cooldownTime = 5f;
    public float activeDuration = 3f;
    public GameObject reconstructionParticlePrefab;
    public GameObject shadowProjectorPrefab;
    public Vector3 shadowProjectorOffset = new Vector3(0f, 0.05f, 0f);

    // create module class instance, 
    public override PlayerModule CreateModuleData(
        PlayerController player, 
        GameObject ui_holder
        )
    {
        ShadowModule shadow_module = new ShadowModule(player, this);
        
        // set up ui instance
        // GameObject ui_instance = MonoBehaviour.Instantiate(ui_element, ui_holder.transform);
        // GravityModuleUI module_ui = ui_instance.GetComponent<GravityModuleUI>();
        // module_ui.module_instance = gravity_module;
        
        return shadow_module;
    }

    public override PlayerModule CreateModuleData(
        PlayerController player
        )
    {
        return new ShadowModule(player, this);
    }
    public override void CreateNewUI(
        PlayerModule player_module,
        GameObject ui_holder
        )
    {
        // GameObject ui_instance = MonoBehaviour.Instantiate(ui_element, ui_holder.transform);
        // ShadowModuleUI module_ui = ui_instance.GetComponent<ShadowModuleUI>();
        // module_ui.module_instance = (ShadowModule)player_module;
    }
}
