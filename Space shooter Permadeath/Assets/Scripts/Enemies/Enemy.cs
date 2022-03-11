using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject Explosion;
    public Transform player;
    public Mastermind mastermind;
    protected Rigidbody2D m_rigidbody;

    [HideInInspector] public int value;

    public float avoidRadius;
    public float avoidForce;

    protected Vector2 direction;

    public int collisionDamage;
    public int collisionSelfDamage;
    [HideInInspector] public Health health;
    public int maxHealth;



    void Start()
    {
        health = new Health(maxHealth, gameObject);
        m_rigidbody = GetComponent<Rigidbody2D>();
    }
    public bool IsInScreen()
    {
        //Kollar om fienden är en bit inom skärmen, så att inte spelaren ska bli skjuten av en fiende som inte syns
        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
         if (0.05f < viewportPosition.x && viewportPosition.x < 0.95f && 0.05f < viewportPosition.y && viewportPosition.y < 0.95f)
        {
            return true;
        }
        else return false;

    }

    public void AvoidCollision()
    {
        Collider2D[] avoidColliders;
        avoidColliders = Physics2D.OverlapCircleAll(transform.position, avoidRadius, LayerMask.GetMask("Enemy"));

        foreach (Collider2D collider in avoidColliders)
        {
            //if (collider.gameObject != gameObject)
            //{
                Vector2 avoidDirection = ((Vector2)collider.transform.position - (Vector2)transform.position).normalized;
                float distanceFactor = 1-Vector2.Distance((Vector2)collider.transform.position, (Vector2)transform.position)/avoidRadius;
                //float avoidDistance = Vector2.Distance((Vector2)collider.transform.position, (Vector2)transform.position);
                if (distanceFactor>0)
                {
                    collider.attachedRigidbody.AddForce(avoidDirection * avoidForce *distanceFactor);

                }
            //}
        }

    }

    private void OnDestroy()
    {
        mastermind.UpdateMoney(value);
        mastermind.UpdateScore(value);
        mastermind.Invoke("CountEnemies",0f);
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
