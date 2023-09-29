using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldHealth : Upgrades
{
    public int increaseAmount;
    public override void Buy()
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        playerMovement.maxShieldHealth += increaseAmount;
        playerMovement.shieldHealth += increaseAmount;
        playerMovement.shieldBar.MaxHealthPoints += increaseAmount;
        playerMovement.shieldBar.SetSize();
    }


    public override string GetDescription()
    {
        float currentShield = player.GetComponent<PlayerMovement>().maxShieldHealth;
        return ("Increases the health of your shield." +
            "\n\nShield Health : " + currentShield + " -> " + (currentShield + increaseAmount)
            );
    }
}
