using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Freighter : Enemy
{
    public float acceleration;
    public GameObject pickup;

    public override void Start()
    {
        base.Start();

        List<GameObject> pickupList = mastermind.waveSpawner.pickupList;
        pickup = pickupList[Random.Range(0, pickupList.Count)];
        SpriteRenderer spriteRenderer = transform.Find("Pickup Sprite").GetComponent<SpriteRenderer>();
        SpriteRenderer pickupSpriteRenderer = pickup.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = pickupSpriteRenderer.sprite;
        spriteRenderer.color = pickupSpriteRenderer.color;

        Vector2 targetPosition;
        targetPosition.x = Random.Range(0.2f, 0.8f);
        targetPosition.y = Random.Range(0.2f, 0.8f);
        targetPosition = Camera.main.ViewportToWorldPoint(targetPosition);
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        transform.up = direction;
    }

    public void FixedUpdate()
    {
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
        Destroy(gameObject);
        mastermind.Invoke("CountEnemies", 0f);
    }
}
