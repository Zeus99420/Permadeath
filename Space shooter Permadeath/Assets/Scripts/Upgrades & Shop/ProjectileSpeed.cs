using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpeed : Upgrades
{
    public float increaseAmount;
    public override void Buy()
    {
        player.GetComponent<Weapons>().projectileSpeed += increaseAmount;
    }
}
