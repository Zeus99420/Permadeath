using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryWeaponsUpgrade : Upgrades
{
    SecondaryWeapons weapon;

    public float rechargeTime;
    public int maxCharges;

    public float rateOfFire; //Antal skott spelaren kan avfyra per sekund

    [Header("CHARGE INDICATOR")]
    public GameObject AmmoImagePrefab;
    public Color readyColor;
    public Color chargingColor;

    public override void Buy()
    {
        weapon = BuySecondaryWeapon();
        weapon.rechargeTime = rechargeTime;
        weapon.maxCharges = maxCharges;
        weapon.charges = maxCharges;
        weapon.rateOfFire = rateOfFire;

        weapon.AmmoImagePrefab = AmmoImagePrefab;
        weapon.readyColor = readyColor;
        weapon.chargingColor = chargingColor;
        


    }

    public virtual SecondaryWeapons BuySecondaryWeapon() { return null; }
}
