using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FasterGun : Upgrades
{
    public float increaseAmount;
    public override void Buy() 
    {
        player.GetComponent<Weapons>().rateOfFire += increaseAmount;
    }
}