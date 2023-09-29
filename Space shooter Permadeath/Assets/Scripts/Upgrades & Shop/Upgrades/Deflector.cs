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
        playerMovement.maxShieldHealth = baseHealth;
        playerMovement.shieldRecharge = rechargeTime;

        playerMovement.shieldRenderer.enabled = true;

        upgradeName = "Upgrade Deflector Shield";
    }

    public override void BuyAnother()
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        playerMovement.maxShieldHealth += healthIncrease;
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
            int currentDurability = player.GetComponent<PlayerMovement>().maxShieldHealth;
            return ("Increase the durability of your deflector shield." +
            "\n\ndurability: " + currentDurability + " -> " + (currentDurability + healthIncrease)
        );
        }


    }

}
