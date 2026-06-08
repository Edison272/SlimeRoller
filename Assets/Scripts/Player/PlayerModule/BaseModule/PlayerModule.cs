using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerModuleType {JUMP}
public abstract class PlayerModule
{
    protected PlayerController player;

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

    // called when the module is being replaced or removed so it can cleanup subscriptions/state
    public virtual void OnDeactivate()
    {
        // default: no-op
    }

}