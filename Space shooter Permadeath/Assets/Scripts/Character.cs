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
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        if (!healthAlreadySet) health = maxHealth;

        CreateHealthBar(healthBarPrefab);
    }

    public float GetHealthPercent()
    {
        return ((float)health / (float)maxHealth);
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

    [HideInInspector] public float shieldHealth = 0;
    public virtual void ShieldDamage(int damageAmount)
    { }

    public virtual void Heal(int healAmount)
    {
        health += healAmount;
        if (health > maxHealth) health = maxHealth;
        healthBar.Health = health;
    }

    public virtual void Die() { }

    //public void SetupHealthbar(GameObject healthBarPrefab)
    //{
    //    GameObject healthBarObject = Instantiate(healthBarPrefab, new Vector3(0, 0), Quaternion.identity);
    //    healthBar = healthBarObject.GetComponent<HealthBar>();
    //    healthBar.character = this;

    //}

    public void CreateHealthBar(GameObject healthBarPrefab)
    {
        GameObject healthBarObject = Instantiate(healthBarPrefab,mastermind.stuffContainer);
        healthBar = healthBarObject.GetComponent<CoolHealthBar>();
        healthBar.character = this;
        healthBar.transform.position = transform.position + healthBar.offset;
        //float width = Mathf.Sqrt(maxHealth) / 10f;
        float width = Mathf.Pow(maxHealth, 0.7f) / 20f;
        float height = 0.05f + Mathf.Sqrt(maxHealth) * 0.015f;
        healthBarObject.transform.Find("Image").GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        healthBarObject.transform.Find("Background").GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        healthBar.MaxHealthPoints = maxHealth;
 
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
