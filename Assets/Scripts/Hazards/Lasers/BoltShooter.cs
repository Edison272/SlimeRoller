using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

[Serializable]
public class BoltShooter
{
    public LayerMask collision_mask;
    public LaserBolt laser_bolt;
    public void ShootLaser(Vector3 direction, Vector3 shoot_pos)
    {
        // A slight spread to the lasers
        direction.x += UnityEngine.Random.Range(-2f, 2f);
        direction.y += UnityEngine.Random.Range(-2f, 2f);
        direction.z += UnityEngine.Random.Range(-2f, 2f);
        direction.Normalize();
        GameObject laser_instance = MonoBehaviour.Instantiate(laser_bolt.gameObject, shoot_pos, Quaternion.LookRotation(direction, Vector3.forward));
    }
}