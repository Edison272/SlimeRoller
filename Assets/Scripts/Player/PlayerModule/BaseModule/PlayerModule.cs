using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerModuleType {JUMP}
public abstract class PlayerModule
{
    protected PlayerController player;
    protected GameObject ui_instance;

    // get player and ui instance from the scripta
    public PlayerModule(
        PlayerController player, 
        PlayerModuleSO base_data)
    {
        this.player = player;
    }
    public abstract void FixedUpdateModule();
    public abstract void UpdateModule();

    public abstract void UseModule(InputAction.CallbackContext context);

    // set the UI GameObject for this module
    public void SetUIInstance(GameObject uiGameObject)
    {
        ui_instance = uiGameObject;
    }

    // called when the module is being replaced or removed so it can cleanup subscriptions/state
    public virtual void OnDeactivate()
    {
        // deactivate the UI when module is deactivated
        if (ui_instance != null)
        {
            ui_instance.SetActive(false);
        }
    }

}