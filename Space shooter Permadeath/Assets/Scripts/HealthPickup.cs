using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup
{
    public int healAmount;
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerMovement>().Heal(healAmount);


            //Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
