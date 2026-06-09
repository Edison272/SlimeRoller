using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
public class ChromeModule : PlayerModule
{
    public bool chromeActive {get; private set;}
    public readonly ChromeModuleSO base_data;
    public ChromeModule(
        PlayerController player, 
        PlayerModuleSO base_data_so
        ) : base(player, base_data_so)
    {
        // set base_data
        base_data = (ChromeModuleSO)base_data_so;

        chromeActive = true;
        
        // connect other information functions
    }

    public override void OnDeactivate()
    {
        base.OnDeactivate();
        
        // unsubscribe input handlers and clear state
        if (player != null && player.player_ability != null)
        {

        }
        chromeActive = false;
    }

    // update functions are called by the player.
    public override void FixedUpdateModule()
    {

    }
    public override void UpdateModule()
    {

    }

    public virtual void ReleaseModule(InputAction.CallbackContext context)
    {

    }

    public override void UseModule(InputAction.CallbackContext context)
    {

    }

}