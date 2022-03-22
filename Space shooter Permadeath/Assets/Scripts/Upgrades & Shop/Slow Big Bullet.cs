using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowBigBullet : Upgrades
{
    //Ska ändra sizen på playerprojectile när denna uppgradering köps och även se till att skotten blir betydligt långsammare men även
    //att det gör mer damage
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
