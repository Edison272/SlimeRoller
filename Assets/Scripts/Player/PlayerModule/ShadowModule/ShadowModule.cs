using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
public class ShadowModule : PlayerModule
{
    //private List<Rigidbody> affectedObjects = new List<Rigidbody>();
    private bool ShadowActive;
    private float timeSinceUse;
    private float cooldownTime;
    private float activeDuration;
    
    public readonly ShadowModuleSO base_data;
    public ShadowModule(
        PlayerController player, 
        PlayerModuleSO base_data_so
        ) : base(player, base_data_so)
    {
        // set base_data
        base_data = (ShadowModuleSO)base_data_so;

        // connect relevant inputs
        player.player_ability.canceled += ReleaseModule;
        player.player_ability.started += UseModule;
        
        // connect other information functions
    }

    // update functions are called by the player.
    public override void FixedUpdateModule()
    {
        if (!ShadowActive)
        {
            return;
        }

        
        // turn into shadow, invisible to cameras & flat and can pass through low ceilings

    }
    public override void UpdateModule()
    {

    }

    public virtual void ReleaseModule(InputAction.CallbackContext context)
    {
        ShadowActive = false;
        OriginalTransparency();
        player.tag = "Player";
    }

    public override void UseModule(InputAction.CallbackContext context)
    {
        ShadowActive = true;
        timeSinceUse = 0;
        SetTransparency(0.1f);
        player.StartCoroutine(ShadowDuration());
        player.tag = "ShadowPlayer";
    }

    public float GetShadowChargePerc()
    {
        return timeSinceUse / cooldownTime;
    }

    private IEnumerator ShadowDuration()
    {
        while (timeSinceUse < activeDuration) // Note to self: might need to add clearance checks to avoid getting stuck.
        {
            timeSinceUse += Time.deltaTime;
            yield return null;
        }
        OriginalTransparency();
        ShadowActive = false;
        player.tag = "Player";
    }

    private void SetTransparency(float alpha)
    {
        // write code that sets transparency, but also stores the original so that i can reset it after.
        return; // not finihsed yet
    }

    private void GetTransparency()
    {
        // stores original transparency of the player, so that it can be reset after the shadow effect wears off.
        return; // not finihsed yet
    }

    private void OriginalTransparency()
    {
        // resets the transparency to the original value after the shadow effect wears off.
        return; // not finihsed yet
    }

}