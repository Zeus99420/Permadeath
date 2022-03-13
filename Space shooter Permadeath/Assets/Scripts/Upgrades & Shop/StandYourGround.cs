using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandYourGround : Upgrades
{
    public float multiplierMaxBase;
    public float multiplierMaxIncrease;
    public float ChargeTime;
    public float UnchargeTime;

    public override void BuyFirst()
    {
        Weapons weapons = player.GetComponent<Weapons>();
        weapons.preFireEffects.Add(weapons.StandYourGround);

        weapons.standYourGroundMultiplierMax = multiplierMaxBase;
        weapons.standYourGroundChargeTime = ChargeTime;
        weapons.standYourGroundUnchargeTime = UnchargeTime;

    }
    public override void BuyAnother()
    {
        Weapons weapons = player.GetComponent<Weapons>();
        weapons.standYourGroundMultiplierMax += multiplierMaxIncrease;
    }
}
