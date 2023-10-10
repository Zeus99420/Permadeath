using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [HideInInspector] public Transform player;
    //[HideInInspector] public Mastermind mastermind;
    protected Rigidbody2D m_rigidbody;

    [HideInInspector] public int value;


    [Header("General")]
    public float avoidForce;

    protected Vector2 direction;
    public float acceleration;

    public int collisionDamage;
    public int collisionSelfDamage;




    public override void Start()
    {
        maxHealth = (int)(maxHealth * Random.Range(0.7f, 1.3f));
        base.Start();

        healthBar.gameObject.SetActive(false);

        m_rigidbody = GetComponent<Rigidbody2D>();
    }
    public bool IsInScreen(float margin)
    {
        //Kollar om fienden �r en bit inom sk�rmen. Anv�nds t ex s� att spelaren inte ska bli skjuten av en fiende som inte syns.
        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        if (0 + margin < viewportPosition.x && viewportPosition.x < 1 - margin && 0 + margin < viewportPosition.y && viewportPosition.y < 1 - margin)
        {
            return true;
        }
        else return false;

    }


    public void AvoidCollision()
    {
        Collider2D[] avoidColliders;
        avoidColliders = Physics2D.OverlapCircleAll(transform.position, 5, LayerMask.GetMask("Enemy"));

        foreach (Collider2D collider in avoidColliders)
        {
            if (collider != GetComponent<Collider2D>())
            {
                ColliderDistance2D colliderDistance = GetComponent<Collider2D>().Distance(collider);
                float avoidDistance = colliderDistance.distance;

                if (avoidDistance < 1.5)
                {
                    if (avoidDistance < 0.1f) avoidDistance = 0.1f;
                    float distanceFactor = 1 / avoidDistance;
                    float force = 0.6f * collider.GetComponent<Enemy>().avoidForce * distanceFactor;
                    Vector2 avoidDirection = -((Vector2)collider.transform.position - (Vector2)transform.position).normalized;
                    Vector2 moveVector = (direction * acceleration + avoidDirection * force) / acceleration;
                    if (moveVector.magnitude > 1) moveVector.Normalize();
                    direction = moveVector;
                }
            }
        }
    }

    public override void Damage(int damageAmount)
    {
        healthBar.gameObject.SetActive(true);
        base.Damage(damageAmount);
        StartCoroutine(Flicker(Color.white));
    }

    [HideInInspector] public float shieldHealth = 0;
    public virtual void ShieldDamage(int damageAmount)
    {
        GetComponent<AreaShield>().Damage(damageAmount);
    }

    public override void Die()
    {
        mastermind.UpdateMoney(value);
        mastermind.UpdateScore(value);
        PlayExplosion();
        Destroy(healthBar.gameObject);
        Destroy(gameObject);
        mastermind.Invoke("CountEnemies", 0f);

        //Andra fiendetyper �n freighter har en chans att droppa en pickup beroende p� sin value.
        if (!GetComponent<Freighter>() && Random.Range(0, 300) < value)
        {
            GameObject pickup = mastermind.waveSpawner.pickupList[0];
            GameObject newPickup = Instantiate(pickup, transform.position, Quaternion.identity, mastermind.stuffContainer);
            newPickup.GetComponent<Pickup>().mastermind = mastermind;
        }

    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.otherCollider.gameObject.tag == "EnemyShield")
            {
                GetComponent<AreaShield>().Collision(collision.gameObject);
            }

            else
            {
                collision.gameObject.GetComponent<PlayerMovement>().Damage(collisionDamage);
                Damage(collisionSelfDamage);
            }


        }


    }

    protected float distance;
    Vector2 interceptPos;
    public void PredictPlayerPosition(float reachVelocity, float maxDistance = Mathf.Infinity)
    {
        Vector2 targetPos = player.position;
        distance = Vector2.Distance(transform.position, targetPos);
        if (distance > maxDistance) distance = maxDistance;

        float timeToReach = distance / reachVelocity;
        interceptPos = targetPos + player.GetComponent<Rigidbody2D>().velocity * timeToReach;

        distance = Vector2.Distance(interceptPos, transform.position);
        direction = (interceptPos - (Vector2)transform.position).normalized;
    }





}
