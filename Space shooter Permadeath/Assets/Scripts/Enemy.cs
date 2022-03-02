using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject Explosion;
    public Transform player;
    protected Rigidbody2D m_rigidbody;

    public float avoidRadius;
    protected Vector2 direction;

    public int collisionDamage;
    public int collisionSelfDamage;
    public Health health;
    public int maxHealth;


    void Start()
    {
        health = new Health(maxHealth, gameObject);
        m_rigidbody = GetComponent<Rigidbody2D>();
    }
    public bool IsInScreen()
    {
        //Kollar om fienden �r en bit inom sk�rmen, s� att inte spelaren ska bli skjuten av en fiende som inte syns
        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
         if (0.05f < viewportPosition.x && viewportPosition.x < 0.95f && 0.05f < viewportPosition.y && viewportPosition.y < 0.95f)
        {
            return true;
        }
        else return false;

    }

    public void AvoidCollision()
    {
        Collider2D avoidCollider = null;
        avoidCollider = Physics2D.OverlapCircle(transform.position, avoidRadius, LayerMask.GetMask("Enemy"));

        if (avoidCollider != null)
        {
            Debug.Log("Evasive maneuvers!");
            direction = -((Vector2)avoidCollider.transform.position - (Vector2)transform.position).normalized;
        }
    }

    private void OnDestroy()
    {
        PlayExplosion();
    }

    void PlayExplosion()
    {
        GameObject explosion = Instantiate(Explosion);
        explosion.transform.position = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovement>().health.Damage(collisionDamage);
            health.Damage(collisionSelfDamage);
        }


    }


}
