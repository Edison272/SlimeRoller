using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class JumpModule : PlayerModule
{
    // charging jump mechanic
    public bool charging = false;
    public float max_charge_time = 2;
    public float curr_charge_time;
    public float jump_scale = 60;
    public float base_jump_amt = 60;
    public JumpModule(PlayerController player) : base(player)
    {
        // connect relevant inputs
        player.player_ability.canceled += UseModule;
        player.player_ability.started += ChargeModule;
    }

    // update functions are called by the player.
    public override void FixedUpdateModule()
    {
        if (charging)
        {
            curr_charge_time = Math.Min(curr_charge_time + Time.deltaTime, max_charge_time);
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
        Debug.Log(jump_scale * curr_charge_time);
        player.ApplyImpulse(Vector2.up, base_jump_amt + jump_scale * curr_charge_time);
        curr_charge_time = 0;
    }
}