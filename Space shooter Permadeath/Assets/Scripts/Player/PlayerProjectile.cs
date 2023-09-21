using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    /*[HideInInspector]*/
    public int damage;
    [HideInInspector] public Weapons weapons;
    protected bool destroyWhenInvisible = true;

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
       StandardHit(other);
    }


    public void StandardHit(Collider2D other)
    {
        if (other.gameObject.tag == "EnemyShield")
        {
            other.GetComponentInParent<Character>().ShieldDamage(damage);
            Destroy(gameObject);
        }

        else if (other.gameObject.tag == "Enemy")
        {
            if (!other.GetComponentInParent<Character>().dead)
            {
                other.GetComponentInParent<Character>().Damage(damage);
                Destroy(gameObject);
            }
        }
    }

    void OnBecameInvisible()
    {
        if (destroyWhenInvisible) Destroy(gameObject);
    }

    public bool IsInScreen(float margin)
    {
        //Kollar om fienden är en bit inom skärmen. Används t ex så att spelaren inte ska bli skjuten av en fiende som inte syns.
        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        if (0 + margin < viewportPosition.x && viewportPosition.x < 1 - margin && 0 + margin < viewportPosition.y && viewportPosition.y < 1 - margin)
        {
            return true;
        }
        else return false;
    }
    }
