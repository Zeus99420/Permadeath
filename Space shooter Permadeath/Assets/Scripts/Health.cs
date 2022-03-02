using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health 
{
    public int health;
    public int MAXhealth;

    public Health(int MAXhealth)
    {
        this.MAXhealth = MAXhealth;
        health = MAXhealth;
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
        if (health < 0) health = 0;
        Debug.Log("health: " + health);
    }

    public void Heal(int healAmount)
    {
        health += healAmount;
        if (health < MAXhealth) health = MAXhealth;
    }

    public void CheckifDead()
    {
        if (health < 0) health = 0;
              
    }
}

