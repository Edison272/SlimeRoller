using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
public class ShockModule : PlayerModule
{
    // recharging jump mechanic
    public float curr_cooldown_time = 0;
    public readonly ShockModuleSO base_data;
    private Collider[] shock_colliders = new Collider[32];
    public int uses {get; private set;} = 0;
    public ShockModule(
        PlayerController player, 
        PlayerModuleSO base_data_so
        ) : base(player, base_data_so)
    {
        // set base_data
        base_data = (ShockModuleSO)base_data_so;
        base_data.SetPlayerStats(player);
        // connect relevant inputs
        player.player_ability.started += UseModule;
        // connect other information functions
        
        uses = base_data.max_uses;
        if (base_data.electricSound != null && player.loopSource != null)
        {
            player.loopSource.clip = base_data.electricSound;
            player.loopSource.loop = true;
            player.loopSource.Play();
        }
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

    public override void OnDeactivate()
    {
        base.OnDeactivate();

        if (player != null && player.loopSource != null && player.loopSource.clip == base_data.electricSound)
        {
            player.loopSource.Stop();
            player.loopSource.clip = null;
            player.loopSource.loop = false;
        }

        // unsubscribe input handler
        if (player != null && player.player_ability != null)
        {
            player.player_ability.started -= UseModule;
        }
    }

    // on release, jump upwards and then reset charge
    public override void UseModule(InputAction.CallbackContext context)
    {
        if (player.loopSource != null && player.loopSource.clip == base_data.electricSound)
        {
            player.loopSource.Stop();
            player.loopSource.clip = null;
            player.loopSource.loop = false;
        }

        if (curr_cooldown_time > 0)
        {
            return;
        }

        if (base_data.activationSound != null && player.audioSource != null)
        {
            player.audioSource.PlayOneShot(base_data.activationSound);
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
            GameObject target = shock_colliders[i].gameObject;
            DroneAI drone_ai = target.gameObject.GetComponent<DroneAI>();
            BigDroneAI big_drone_ai = target.gameObject.GetComponent<BigDroneAI>();
            SecurityCameraAI camera_ai = target.gameObject.GetComponent<SecurityCameraAI>();

            if (camera_ai)
            {
                camera_ai.Stun(base_data.stun_time);
            }

            if (drone_ai)
            {
                drone_ai.Stun(base_data.stun_time);
            }

            if (big_drone_ai)
            {
                big_drone_ai.Stun(base_data.stun_time);
            }
        }
        curr_cooldown_time += base_data.cooldown_time;
        GameObject shockwave_instance = MonoBehaviour.Instantiate(base_data.shockwave_prefab, player.transform.position, Quaternion.identity);
        MonoBehaviour.Destroy(shockwave_instance, 3);

        // VFX
        shockwave_instance.transform.localScale = Vector3.one * base_data.shock_radius;

        // destroy this module after using it enough times
        uses -= 1;
        if (uses <= 0)
        {
           player.SetModule(base_data.default_module);
        }

    }

    # region Get Data
    public float GetShockRechargePerc()
    {
        return 1 - curr_cooldown_time / base_data.cooldown_time;
    }
    #endregion
}