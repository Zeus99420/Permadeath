using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Freighter : Enemy
{

    public GameObject pickup;
    
    Transform pickupTransform;

    Vector2 moveDirection;
    public float turnRate;

    public override void Start()
    {
        base.Start();

        List<GameObject> pickupList = mastermind.waveSpawner.pickupList;
        pickup = pickupList[Random.Range(0, pickupList.Count)];
        pickupTransform = transform.Find("Pickup Sprite");
        SpriteRenderer pickupRenderer = pickupTransform.GetComponent<SpriteRenderer>();
        SpriteRenderer prefabRenderer = pickup.GetComponent<SpriteRenderer>();
        pickupRenderer.sprite = prefabRenderer.sprite;
        pickupRenderer.color = prefabRenderer.color;

        Vector2 targetPosition;
        targetPosition.x = Random.Range(0.2f, 0.8f);
        targetPosition.y = Random.Range(0.2f, 0.8f);
        targetPosition = Camera.main.ViewportToWorldPoint(targetPosition);
        moveDirection = (targetPosition - (Vector2)transform.position).normalized;
        transform.up = moveDirection;
    }

    public void FixedUpdate()
    {
        direction = moveDirection;
        AvoidCollision();
        //transform.up = Vector3.Slerp(transform.up, direction, turnRate * Time.fixedDeltaTime);
        transform.up = Vector3.RotateTowards(transform.up, direction, turnRate * Time.fixedDeltaTime, 1f);

        m_rigidbody.AddForce(transform.up * acceleration);
    }

    public override void Die()
    {
        GameObject newPickup = Instantiate(pickup, transform.position, Quaternion.identity,mastermind.stuffContainer);
        newPickup.GetComponent<Pickup>().mastermind = mastermind;
        base.Die();
    }

    private void OnBecameInvisible()
    {
        if (!dead)
        {
            Destroy(healthBar.gameObject);
            Destroy(gameObject);

            mastermind.Invoke("CountEnemies", 0f);
        }
    }
}
