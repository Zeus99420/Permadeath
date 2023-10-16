using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaShield : MonoBehaviour
{
    public int collisionSelfDamage;
    public int collisionDamageMax;

    public virtual float GetHealth(Collider2D collider) { return 0f; }

    public virtual void Damage(Collider2D collider, int damageAmount)
    {
        Debug.Log("Shield Damage"); 
    }



    public virtual void Collision(Collider2D collider, GameObject other)
    {}
}
