using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : Upgrades
{
    public float increaseAmount;
    public override void Buy()
    {
        player.GetComponent<PlayerMovement>().acceleration +=increaseAmount;
    }
}
