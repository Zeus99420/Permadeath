using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpPickup : Pickup
{
    int expAmount;
    public int baseExp;
    public float expFraction;

    protected override void Start()
    {
        base.Start();
        expAmount = baseExp + (int)(mastermind.expRequired * expFraction);

    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            mastermind.UpdateExp(expAmount);
            Destroy(gameObject);
        }
    }
}
