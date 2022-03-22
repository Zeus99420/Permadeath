using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomb : Character
{
    [HideInInspector] public int damage;
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {
           other.GetComponent<PlayerMovement>().Damage(damage);
           Die();
        }
    }

    public override void Die()
    {
        PlayExplosion();
        Destroy(gameObject);
    }
}