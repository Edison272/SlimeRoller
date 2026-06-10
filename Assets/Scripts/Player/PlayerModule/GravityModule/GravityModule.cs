using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
public class GravityModule : PlayerModule
{
    private List<Rigidbody> affectedObjects = new List<Rigidbody>();
    public bool gravityActive {get; private set;}
    public readonly GravityModuleSO base_data;
    private Collider[] objectsInRange = new Collider[0];
    public GravityModule(
        PlayerController player, 
        PlayerModuleSO base_data_so
        ) : base(player, base_data_so)
    {
        // set base_data
        base_data = (GravityModuleSO)base_data_so;
        base_data.SetPlayerStats(player);

        // connect relevant inputs
        player.player_ability.canceled += ReleaseModule;
        player.player_ability.started += UseModule;
        
        // connect other information functions
    }

    public override void OnDeactivate()
    {
        base.OnDeactivate();
        
        // unsubscribe input handlers and clear state
        if (player != null && player.player_ability != null)
        {
            player.player_ability.canceled -= ReleaseModule;
            player.player_ability.started -= UseModule;
        }
        gravityActive = false;
        affectedObjects.Clear();
    }

    // update functions are called by the player.
    public override void FixedUpdateModule()
    {
        if (!gravityActive)
        {
            return;
        }

        objectsInRange = Physics.OverlapSphere(player.transform.position, base_data.pullRadius);

        affectedObjects.Clear();

        foreach (Collider obj in objectsInRange)
        {
            if (obj.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                if (obj.CompareTag("Crate"))
                {
                    affectedObjects.Add(rb);
                }
            }
            Debug.Log("bullet", obj);
            if (obj.CompareTag("Bullet"))
            {
                
                obj.transform.parent.GetComponent<LaserBolt>().speed /= 1f;
            }
        }

        // Applies physics to nearby objects
        foreach (Rigidbody rb in affectedObjects)
        {
            Vector3 direction = player.transform.position - rb.transform.position;
            float distance = direction.magnitude;

            if (distance < base_data.orbitRadius)
            {
                Vector3 tangent = Vector3.Cross(direction, Vector3.up);
                rb.AddForce(tangent * base_data.orbitStrength);
                rb.AddForce(Vector3.up * base_data.liftStrength, ForceMode.Acceleration);
                continue;
            }

            direction.Normalize();

            rb.AddForce(direction * base_data.pullStrength, ForceMode.Acceleration);

        }

    }
    public override void UpdateModule()
    {

    }

    public virtual void ReleaseModule(InputAction.CallbackContext context)
    {
        gravityActive = false;
        affectedObjects.Clear();
    }

    public override void UseModule(InputAction.CallbackContext context)
    {
        gravityActive = true;
    }
    public virtual int GetGravityTargets()
    {
        return affectedObjects.Count; 
    }

}