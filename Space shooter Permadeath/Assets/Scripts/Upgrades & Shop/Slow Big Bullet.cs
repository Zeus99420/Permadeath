using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowBigBullet : Upgrades
{
    //Ska ändra sizen på playerprojectile när denna uppgradering köps och även se till att skotten blir betydligt långsammare men även
    //att det gör mer damage
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
