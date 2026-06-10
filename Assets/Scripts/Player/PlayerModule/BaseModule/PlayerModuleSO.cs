using UnityEngine;

[CreateAssetMenu(fileName = "Player Module")]
/* 
This is the template class for Player Module Objects. 
This is where the game will instantiate the class, and get object instances for
The ability's UI, pickups, and other stuff.
*/
public abstract class PlayerModuleSO : ScriptableObject
{
    // UI element
    public GameObject ui_element;

    // set slime VFX
    public Material membrane_material;
    public Material core_material;
    public Color slime_glow;

    // stat changes
    public float speed_multiplier = 1;

    // create module class instance, 
    public abstract PlayerModule CreateModuleData(
        PlayerController player, 
        GameObject ui_holder // this would be some object within the canvas
    );

    public virtual void SetPlayerStats(PlayerController player)
    {
        player.SetCurrentSpeed(speed_multiplier);
    }

    public abstract PlayerModule CreateModuleData(
        PlayerController player
    );

    public abstract void CreateNewUI(
        PlayerModule player_module,
        GameObject ui_holder // this would be some object within the canvas
    );   
}