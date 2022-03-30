using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deflector : Upgrades
{

    public int baseHealth;
    public int healthIncrease;
    public float rechargeTime;

    public override void BuyFirst()
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        playerMovement.haveDeflector = true;
        playerMovement.maxDeflectorHealth = baseHealth;
        playerMovement.deflectorRechargeTime = rechargeTime;

        playerMovement.deflectorRenderer.enabled = true;
    }

    public override void BuyAnother()
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        playerMovement.maxDeflectorHealth += healthIncrease;
    }

}
