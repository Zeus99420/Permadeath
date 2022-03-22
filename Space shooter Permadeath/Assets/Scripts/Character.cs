using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int maxHealth;
    /*[HideInInspector]*/ public int health;

    [HideInInspector] public HealthBar healthBar;
    public GameObject Explosion;
    SpriteRenderer spriteRenderer;
    Color originalColor;
    [HideInInspector] public bool healthAlreadySet = false;

    public virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        if (!healthAlreadySet) health = maxHealth;       
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

    //Karaktärer blinkar kort i en färg, t ex när de tar skada.
    public IEnumerator Flicker(Color flickerColor)
    {
        spriteRenderer.color = flickerColor;
        yield return new WaitForSeconds(0.08f);
        spriteRenderer.color = originalColor;
    }

}
