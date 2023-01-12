using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketSwarmUpgrade : SecondaryWeaponsUpgrade
{
    public GameObject projectile;

    public int damage;
    public int rocketCount;
    public float acceleration;
    public float engineDelay;

    public float launchVelocity;
    public float launchTime;
    public float spread;


    public override SecondaryWeapons BuySecondaryWeapon()
    {
        RocketSwarm rockets = player.AddComponent<RocketSwarm>();

        rockets.projectile = projectile;
        rockets.damage = damage;
        rockets.rocketCount = rocketCount;
        rockets.acceleration = acceleration;
        rockets.engineDelay = engineDelay;
        rockets.launchVelocity = launchVelocity;
        rockets.launchTime = launchTime;
        rockets.spread = spread;

        return rockets;
    }
}
