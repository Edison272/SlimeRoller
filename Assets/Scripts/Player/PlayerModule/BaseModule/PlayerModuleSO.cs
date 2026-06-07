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

    // create module class instance, 
    public abstract PlayerModule CreateModuleData(
        PlayerController player, 
        GameObject ui_holder // this would be some object within the canvas
    );

    public abstract PlayerModule CreateModuleData(
        PlayerController player
    );

    public abstract void CreateNewUI(
        PlayerModule player_module,
        GameObject ui_holder // this would be some object within the canvas
    );   
}