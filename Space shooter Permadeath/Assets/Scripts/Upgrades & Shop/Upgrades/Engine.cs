using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : Upgrades
{
    public float increaseAmount;
    public Sprite BiggerEngine;
    public override void Buy()
    {
        player.GetComponent<SpriteRenderer>().sprite = BiggerEngine;
        player.GetComponent<PlayerMovement>().baseAcceleration +=increaseAmount;
    }

    public override string GetDescription()
    {
        float currentAcceleration = player.GetComponent<PlayerMovement>().baseAcceleration;
        return ("Your ship flies faster." +
            "\n\nacceleration: " + currentAcceleration + " -> " + (currentAcceleration+increaseAmount)
            );
    }
}
