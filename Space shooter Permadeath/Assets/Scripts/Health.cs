using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health 
{
    public int health;
    public int MAXhealth;

    GameObject gameObject;

    public Health(int MAXhealth, GameObject gameObject)
    {
        this.MAXhealth = MAXhealth;
        health = MAXhealth;
        this.gameObject = gameObject;
    }

    public int GetHealth()
    {
        return health;
    }
    public float GetHealthPercent()
    {
        return ((float)health / (float)MAXhealth);
    }
    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            GameObject.Destroy(gameObject);
            health = 0;
        }
    }

    public void Heal(int healAmount)
    {
        health += healAmount;
        if (health < MAXhealth) health = MAXhealth;
    }



}

