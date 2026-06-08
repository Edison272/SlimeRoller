using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class ShockModule : PlayerModule
{
    // recharging jump mechanic
    public float curr_cooldown_time = 0;
    public readonly ShockModuleSO base_data;
    private Collider[] shock_colliders = new Collider[32];
    public ShockModule(
        PlayerController player, 
        PlayerModuleSO base_data_so
        ) : base(player, base_data_so)
    {
        // set base_data
        base_data = (ShockModuleSO)base_data_so;

        // connect relevant inputs
        player.player_ability.started += UseModule;
        // connect other information functions
    }

    // update functions are called by the player.
    public override void FixedUpdateModule()
    {
    }
    public override void UpdateModule()
    {
        if (curr_cooldown_time > 0)
        {
            curr_cooldown_time = Math.Max(
                curr_cooldown_time - Time.deltaTime, 
                0);
        }
    }

    // on release, jump upwards and then reset charge
    public override void UseModule(InputAction.CallbackContext context)
    {
        if (curr_cooldown_time > 0)
        {
            return;
        }
        
        /* Reuse the pre-allocated array for Physics.OverlapSphereNonAlloc.
        Find all shockable targets within range (mask)
        */
        int numColliders = Physics.OverlapSphereNonAlloc(
            player.transform.position, 
            base_data.shock_radius, 
            shock_colliders, 
            base_data.shock_mask);

        // Iterate through detected colliders and send the AddDamage message.
        for (int i = 0; i < numColliders; i++)
        {
            
        }

        curr_cooldown_time += base_data.cooldown_time;
    }

    # region Get Data
    public float GetShockRechargePerc()
    {
        return 1 - curr_cooldown_time / base_data.cooldown_time;
    }
    #endregion
}