using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public Mastermind mastermind;

    public GameObject healthBarPrefab;
    public int maxHealth;
    /*[HideInInspector]*/
    public int health;

    //[HideInInspector] public HealthBar healthBar;
    [HideInInspector] public CoolHealthBar healthBar;
    public GameObject Explosion;
    SpriteRenderer spriteRenderer;
    Color originalColor;
    [HideInInspector] public bool healthAlreadySet = false;
    [HideInInspector] public bool dead;

    public virtual void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        if (!healthAlreadySet) health = maxHealth;

        healthBar = CreateHealthBar(healthBarPrefab);
    }

    public virtual void Damage(int damageAmount)
    {
        health -= damageAmount;
        healthBar.Health = health;
        healthBar.Damages += damageAmount;
        if (health <= 0 && !dead)
        {
            dead = true;
            Die();
            health = 0;
        }
    }



    public virtual void Heal(int healAmount)
    {
        health += healAmount;
        if (health > maxHealth) health = maxHealth;
        healthBar.Health = health;
    }

    public virtual void Die() { }

    public CoolHealthBar CreateHealthBar(GameObject healthBarPrefab)
    {
        GameObject healthBarObject = Instantiate(healthBarPrefab,mastermind.stuffContainer);
        CoolHealthBar newHealthBar = healthBarObject.GetComponent<CoolHealthBar>();
        newHealthBar.character = this;
        //newHealthBar.transform.position = transform.position + newHealthBar.offset;
        newHealthBar.MaxHealthPoints = maxHealth;
        newHealthBar.SetSize();
        return newHealthBar;
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
