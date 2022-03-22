using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowBigBullet : Upgrades
{
    //Ska �ndra sizen p� playerprojectile n�r denna uppgradering k�ps och �ven se till att skotten blir betydligt l�ngsammare men �ven
    //att det g�r mer damage
    public GameObject projectile;
    public float projectileSpeed;
    
    public override void BuyFirst()
    {
        Weapons weapons = player.GetComponent<Weapons>();
        weapons.fireMode = "SlowBigBulletFire";
        weapons.projectileSpeed = projectileSpeed / 4;
        GameObject playerprojectile = Instantiate(projectile, transform.position, transform.rotation);
        playerprojectile.GetComponent<PlayerProjectile>();
        playerprojectile.transform.localScale *= 5.2f;

    }

    public override void BuyAnother()
    {
        Weapons weapons = player.GetComponent<Weapons>();
    }

}
