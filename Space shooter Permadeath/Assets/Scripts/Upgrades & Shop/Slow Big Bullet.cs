using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowBigBullet : Upgrades
{
    //Ska �ndra sizen p� playerprojectile n�r denna uppgradering k�ps och �ven se till att skotten blir betydligt l�ngsammare men �ven
    //att det g�r mer damage
    public GameObject PlayerProjectile;

    public override void BuyFirst()
    {
        Weapons weapons = player.GetComponent<Weapons>();
        weapons.fireMode = "SlowBigBulletFire";
        
    }

    public override void BuyAnother()
    {
        Weapons weapons = player.GetComponent<Weapons>();
    }

}
