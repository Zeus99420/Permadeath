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
    public float avoidRadius;
    public float avoidForce;

    protected Vector2 direction;
    public float acceleration;

    public int collisionDamage;
    public int collisionSelfDamage;




    public override void Start()
    {
        maxHealth = (int)(maxHealth*Random.Range(0.7f, 1.3f));
        base.Start();

        healthBar.gameObject.SetActive(false);

        m_rigidbody = GetComponent<Rigidbody2D>();
    }
    public bool IsInScreen(float margin)
    {
        //Kollar om fienden är en bit inom skärmen. Används t ex så att spelaren inte ska bli skjuten av en fiende som inte syns.
        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
         if (0+margin < viewportPosition.x && viewportPosition.x < 1-margin && 0+margin < viewportPosition.y && viewportPosition.y < 1-margin)
        {
            return true;
        }
        else return false;

    }

    public void AvoidCollision1()
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

    public void AvoidCollision2()
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
                    if (avoidDistance < 0) avoidDistance = 0;
                    float distanceFactor = (1.5f-avoidDistance) /1.5f;
                    float force = 0.7f * avoidForce * distanceFactor;
                    if (force > acceleration) force = acceleration;
                    //Vector2 avoidDirection = -colliderDistance.normal;
                    Vector2 avoidDirection = ((Vector2)collider.transform.position - (Vector2)transform.position).normalized;
                    collider.attachedRigidbody.AddForce(avoidDirection * force);
                }
                //Vector2 avoidDirection = -colliderDistance.normal;
                //float force = (0.15f * avoidForce) / avoidDistance;
                //force = Mathf.Clamp(force, 0, avoidForce * 2);
                //collider.attachedRigidbody.AddForce(avoidDirection*force);
            }
        }
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
                    //float distanceFactor = (1.5f - avoidDistance) / 1.5f;
                    float distanceFactor = 1 / avoidDistance;
                    float force = 0.6f * collider.GetComponent<Enemy>().avoidForce * distanceFactor;
                    //Vector2 avoidDirection = colliderDistance.normal;
                    Vector2 avoidDirection = -((Vector2)collider.transform.position - (Vector2)transform.position).normalized;
                    Vector2 moveVector = (direction * acceleration + avoidDirection * force)/acceleration;
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
            collision.gameObject.GetComponent<PlayerMovement>().Damage(collisionDamage);
            Damage(collisionSelfDamage);
        }


    }


}
