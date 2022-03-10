using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorPlates : Upgrades
{
    public int increaseAmount;
    public override void Buy()
    {
        player.GetComponent<PlayerMovement>().health.MAXhealth += increaseAmount;
        player.GetComponent<PlayerMovement>().health.health += increaseAmount;

    }
}
