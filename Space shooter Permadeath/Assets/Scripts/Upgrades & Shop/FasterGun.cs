using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FasterGun : Upgrades
{
    public float increaseAmount;
    public Sprite BiggerGun;
    public override void Buy() 
    {
        player.GetComponent<SpriteRenderer>().sprite = BiggerGun;
        player.GetComponent<Weapons>().rateOfFire += increaseAmount;
    }

   
   
}
