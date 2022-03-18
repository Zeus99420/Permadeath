using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int maxHealth;
    /*[HideInInspector]*/ public int health;

    [HideInInspector] public HealthBar healthBar;
    public GameObject Explosion;

    public bool firstTime = true;
    public virtual void Start()
    {
        if (firstTime)
        {
            health = maxHealth;
            firstTime = false;
        }
    }

    public float GetHealthPercent()
    {
        return ((float)health / (float)maxHealth);
    }
    public virtual void Damage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Die();
            health = 0;
        }
    }

    public virtual void Heal(int healAmount)
    {
        health += healAmount;
        if (health > maxHealth) health = maxHealth;
    }

    public virtual void Die() { }

    public void SetupHealthbar(GameObject healthBarPrefab)
    {
        GameObject healthBarObject = Instantiate(healthBarPrefab, new Vector3(0, 0), Quaternion.identity);
        healthBar = healthBarObject.GetComponent<HealthBar>();
        healthBar.character = this;

    }

    public virtual void PlayExplosion()
    {
        GameObject explosion = Instantiate(Explosion);
        explosion.transform.position = transform.position;
    }

}
