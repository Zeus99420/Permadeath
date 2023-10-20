using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    /*[HideInInspector]*/
    public int damage;
    [HideInInspector] public Weapons weapons;
    protected bool destroyWhenInvisible = true;
    protected bool destroyed;

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        StandardHit(other);
    }

    protected bool canHit=true;
    public void StandardHit(Collider2D other)
    {
        if(canHit)
        {
            if (other.gameObject.tag == "EnemyShield")
            {
                other.GetComponentInParent<Enemy>().ShieldDamage(other, damage);
                Remove();
            }

            else if (other.gameObject.tag == "Enemy")
            {
                if (!other.GetComponentInParent<Character>().dead)
                {
                    other.GetComponentInParent<Character>().Damage(damage);
                    Remove();
                }
            }
        }

    }

    void OnBecameInvisible()
    {
        if (destroyWhenInvisible) Destroy(gameObject);
    }
    
    protected void Remove()
    {
        canHit = false;
        Destroy(gameObject);
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
