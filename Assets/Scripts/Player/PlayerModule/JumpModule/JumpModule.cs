using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class JumpModule : PlayerModule
{
    // charging jump mechanic
    public bool charging = false;
    public float curr_charge_time;
    public readonly JumpModuleSO base_data;
    public JumpModule(
        PlayerController player, 
        PlayerModuleSO base_data_so
        ) : base(player, base_data_so)
    {
        // set base_data
        base_data = (JumpModuleSO)base_data_so;
        base_data.SetPlayerStats(player);

        // connect relevant inputs
        player.player_ability.canceled += UseModule;
        player.player_ability.started += ChargeModule;
        // connect other information functions
    }

    public override void OnDeactivate()
    {
        base.OnDeactivate();
        
        // unsubscribe input handlers to avoid orphaned callbacks
        if (player != null && player.player_ability != null)
        {
            player.player_ability.canceled -= UseModule;
            player.player_ability.started -= ChargeModule;
        }
    }

    // update functions are called by the player.
    public override void FixedUpdateModule()
    {
        if (!player.on_ground)
        {
            return;
        }
        if (charging)
        {
            curr_charge_time = Math.Min(curr_charge_time + Time.deltaTime, base_data.max_charge_time);
        }
    }
    public override void UpdateModule()
    {

    }

    // on first press, set charging to true so that the ability what constantly increment charge level
    public virtual void ChargeModule(InputAction.CallbackContext context)
    {
        charging = true;
    }

    // on release, jump upwards and then reset charge
    public override void UseModule(InputAction.CallbackContext context)
    {
        charging = false;
        if (!player.on_ground)
        {
            return;
        }

        float jump_scale = base_data.jump_scale * curr_charge_time;
        float base_jump_amt = base_data.base_jump_amt;
        float jump_power = base_jump_amt + jump_scale;
        player.ApplyImpulse(Vector2.up, jump_power);
        curr_charge_time = 0;
    }

    # region Get Data
    public float GetJumpChargePerc()
    {
        return curr_charge_time / base_data.max_charge_time;
    }
    #endregion
}