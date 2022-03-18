using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorPlates : Upgrades
{
    public int increaseAmount;
    public Sprite Shield;
    public override void Buy()
    {
        player.GetComponent<SpriteRenderer>().sprite = Shield;
        player.GetComponent<PlayerMovement>().maxHealth += increaseAmount;
        player.GetComponent<PlayerMovement>().health += increaseAmount;

    }
}
