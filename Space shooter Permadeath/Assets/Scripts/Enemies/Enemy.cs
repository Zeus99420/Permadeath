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
        base.Start();
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    public bool IsInScreen(float margin)
    {
        return IsInScreen(margin, transform.position);
    }
    public bool IsInScreen(float margin, Vector2 position)
    {
        //Kollar om fienden är en bit inom skärmen. Används t ex så att spelaren inte ska bli skjuten av en fiende som inte syns.
        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(position);
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

        Vector2 moveVector = direction;
        foreach (Collider2D collider in avoidColliders)
        {
            if (collider != GetComponent<Collider2D>())
            {
                ColliderDistance2D colliderDistance = GetComponent<Collider2D>().Distance(collider);
                float avoidDistance = colliderDistance.distance;

                if (avoidDistance < 2)
                {
                    if (avoidDistance < 0.1f) avoidDistance = 0.1f;
                    float distanceFactor = 1 / avoidDistance;
                    float force = 0.6f * collider.GetComponentInParent<Enemy>().avoidForce * distanceFactor;
                    Vector2 avoidDirection = -((Vector2)collider.transform.position - (Vector2)transform.position).normalized;
                    moveVector += (avoidDirection * force*0.5f) / acceleration;

                }
            }
        }

        //if (moveVector.magnitude > 1) moveVector.Normalize();
        float magnitude = Mathf.Clamp(moveVector.magnitude, 0.3f, 1f);
        direction = moveVector.normalized * magnitude;
    }

    public override void Damage(int damageAmount)
    {
        healthBar.gameObject.SetActive(true);
        base.Damage(damageAmount);
        StartCoroutine(Flicker(Color.white));
    }

    [HideInInspector] public float shieldHealth = 0;
    public virtual void ShieldDamage(Collider2D collider, int damageAmount)
    {
        GetComponent<AreaShield>().Damage(collider, damageAmount);
    }

    public override void Die()
    {
        mastermind.UpdateMoney(value);
        mastermind.UpdateScore(value);
        PlayExplosion();
        Destroy(healthBar.gameObject);
        Destroy(gameObject);
        mastermind.Invoke("CountEnemies", 0f);

        //Andra fiendetyper än freighter har en chans att droppa en pickup beroende på sin value.
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
                GetComponent<AreaShield>().Collision(collision.otherCollider, collision.gameObject);
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
