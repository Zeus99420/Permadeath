using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    /*[HideInInspector]*/
    public int damage;
    [HideInInspector] public Weapons weapons;
    public void StandardHit(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (!other.GetComponent<Character>().dead)
            {
                other.GetComponent<Character>().Damage(damage);
                //Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
