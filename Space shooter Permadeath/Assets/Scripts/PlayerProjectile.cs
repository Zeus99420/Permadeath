using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public int damage;
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Enemy")
        {
            //other.GetComponent<Enemy>().health.Damage(damage);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
