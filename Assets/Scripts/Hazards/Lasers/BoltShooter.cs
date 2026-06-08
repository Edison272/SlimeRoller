using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

[Serializable]
public class BoltShooter
{
    public LayerMask collision_mask;
    public LaserBolt laser_bolt;
    public float inaccuracy = 2;

    // get the UNNORMALIZED direction, and then the position from which the point is shot
    public void ShootLaser(Vector3 direction, Vector3 shoot_pos)
    {
        // A slight spread to the lasers
        float inaccuracy_factor = inaccuracy * direction.magnitude; 
        direction.x += UnityEngine.Random.Range(-inaccuracy, inaccuracy);
        direction.y += UnityEngine.Random.Range(-inaccuracy, inaccuracy);
        direction.z += UnityEngine.Random.Range(-inaccuracy, inaccuracy);
        direction.Normalize();
        GameObject laser_instance = MonoBehaviour.Instantiate(laser_bolt.gameObject, shoot_pos, Quaternion.LookRotation(direction, Vector3.forward));
    }
}