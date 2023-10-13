using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaShield : MonoBehaviour
{
    public int collisionSelfDamage;
    public int collisionDamageMax;

    [HideInInspector] public float health;

    public LineRenderer shieldRenderer;

    public virtual void Damage(Collider2D collider, int damageAmount)
    {
        Debug.Log("Shield Damage"); 
    }



    public virtual void Collision(Collider2D collider, GameObject other)
    {}
}
