using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerModuleType {JUMP}
public abstract class PlayerModule
{
    protected PlayerController player;
    public PlayerModule(PlayerController player)
    {
        this.player = player;
    }
    public abstract void FixedUpdateModule();
    public abstract void UpdateModule();

    public abstract void UseModule(InputAction.CallbackContext context);

}