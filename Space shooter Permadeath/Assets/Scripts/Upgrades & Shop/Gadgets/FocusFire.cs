using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusFire : Gadgets
{
    public float rateOfFireMultiplier;

    public float freeRotationRate;
    public float lockedRotationRate;
    public float recoveryTime;
    public override void BuyFirst() 
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        playerMovement.focusFireEnabled = true;
        playerMovement.freeRotationRate = freeRotationRate;
        playerMovement.lockedRotationRate = lockedRotationRate;
        playerMovement.rotationRate = freeRotationRate;
        playerMovement.focusFireRecoveryTime = recoveryTime;

        Weapons weapons = player.GetComponent<Weapons>();
        weapons.rateOfFire *= rateOfFireMultiplier;
        weapons.rateOfFireMultiplier *= rateOfFireMultiplier;
    }

    public override string GetDescription()
    {
        //return ("Your projectiles fly faster and can pierce and hit multiple enemies. Damage is halved after each hit." +
        //    "\n\ndamage: x" + damageMultiplier +
        //    "\nrate of fire: x" + rateOfFireMultiplier
        //    );

        return ("Your ship can fire much faster but can only rotate slowly while firing" +
            "\n\n Rate of Fire: x" + rateOfFireMultiplier            
           );
    }








}
