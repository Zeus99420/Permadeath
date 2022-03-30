using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : Upgrades
{
    public int maxHealth;
    public float duration;
    public float cooldown;
    public Color colorReady;
    public Color colorActive;
    public Color colorDamaged;
    public Vector2 readySize;
    public Vector2 activeSize;

    public override void Buy()
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();

        playerMovement.maxBarrierHealth = maxHealth;
        playerMovement.barrierDuration = duration;
        playerMovement.barrierCooldown = cooldown;
        playerMovement.barrierColorReady = colorReady;
        playerMovement.barrierColorActive = colorActive;
        playerMovement.barrierColorDamaged = colorDamaged;
        playerMovement.barrierReadySize = readySize;
        playerMovement.barrierActiveSize = activeSize;

        playerMovement.ReadyBarrier();
        playerMovement.barrierBought = true;


    }
}
