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

    public override string GetDescription()
    {
        if (firstTimeBuying) return ("A shield protects you from a small amount of damage." +
            "\nRecharges over time." +
            "\n\nDurability: " + baseHealth + " damage" +
            "\nRecharge time: " + rechargeTime + " seconds"
            
            );

        else
        {
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            int currentDurability = player.GetComponent<PlayerMovement>().maxDeflectorHealth;
            return ("Increase the durability of your deflector shield." +
            "\n\ndurability: " + currentDurability + " -> " + (currentDurability + healthIncrease)
        );
        }


    }

}
