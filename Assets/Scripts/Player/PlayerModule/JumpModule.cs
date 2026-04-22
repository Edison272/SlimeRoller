using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class JumpModule : PlayerModule
{
    protected PlayerController player;
    bool charging = true;
    public JumpModule(PlayerController player) : base(player)
    {
        player.player_ability.canceled += UseModule;
        player.player_ability.started += ChargeModule;
    }
    public override void UpdateModule()
    {
        
    }

    public virtual void ChargeModule(InputAction.CallbackContext context)
    {
        
    }

    public override void UseModule(InputAction.CallbackContext context)
    {
        
    }
}