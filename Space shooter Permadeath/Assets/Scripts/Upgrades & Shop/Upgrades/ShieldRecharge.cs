using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldRecharge : Upgrades
{
    public int increaseAmount;
    public override void Buy()
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        playerMovement.shieldRecharge += increaseAmount;
    }


    public override string GetDescription()
    {
        float currentRate = player.GetComponent<PlayerMovement>().shieldRecharge;
        return ("Your shield recharges faster" +
            "\n\nRecharge Rate : " + currentRate + " -> " + (currentRate + increaseAmount)
            );
    }
}
