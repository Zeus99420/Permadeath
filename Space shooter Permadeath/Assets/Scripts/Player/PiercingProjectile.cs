using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingProjectile : PlayerProjectile
{

    public float piercingMultiplier;
    public List<GameObject> alreadyHit = new List<GameObject>();

    public override void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Enemy" && !alreadyHit.Contains(other.gameObject))
        {
            alreadyHit.Add(other.gameObject);
            Character enemy = other.GetComponent<Character>();
            if (!enemy.dead)
            {
                float newDamage;
                if (damage < enemy.health) newDamage = damage * piercingMultiplier;
                else newDamage = damage - enemy.health * piercingMultiplier;
                enemy.Damage(damage);

                if (newDamage < 1) Destroy(gameObject);
                else damage = (int)newDamage;
            }
        }
    }
}
