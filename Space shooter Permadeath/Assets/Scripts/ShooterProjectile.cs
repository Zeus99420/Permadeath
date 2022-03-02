using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterProjectile : MonoBehaviour
{
    public int damage;
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerMovement>().health.Damage(damage);

            //Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
