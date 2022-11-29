using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuclearBombProjectile : MonoBehaviour
{
    public GameObject explosionPrefab;
    public float delay;
    public float explosionDuration = 0.3f;
    public int maxDamage;
    int damage;
    public float radius;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Countdown());
    }

    // Update is called once per frame
    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(delay);
        //Explode();
        StartCoroutine(Explode());
    }

    public Color ringColor1;
    public Color ringColor2;
    IEnumerator Explode()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Instantiate(explosionPrefab, transform.position, transform.rotation);

        yield return new WaitForSeconds(0.2f);
        Transform ring = transform.Find("Explosion Ring");
        ring.gameObject.SetActive(true);
        SpriteRenderer ringRenderer = ring.GetComponentInChildren<SpriteRenderer>();


        for (float t=0; t < explosionDuration; t += Time.deltaTime)
        {
            ring.localScale = Vector2.one * t / explosionDuration * radius;
            ringRenderer.color = Color.Lerp(ringColor1,ringColor2, t / explosionDuration);
            damage = (int)(maxDamage * (explosionDuration - t) / explosionDuration);
            yield return null;
        }

        Destroy(gameObject);
    }


    List<GameObject> alreadyHit = new List<GameObject>();
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" && !alreadyHit.Contains(other.gameObject))
        {
            alreadyHit.Add(other.gameObject);

            if (!other.GetComponent<Character>().dead)
            {
                other.GetComponent<Character>().Damage(damage);
            }
        }
    }

    void OLDExplode()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
        //explosion.transform.localScale = new Vector2(4, 4);

        Collider2D[] EnemiesHit = Physics2D.OverlapCircleAll(transform.position,radius);
        foreach (Collider2D other in EnemiesHit)
        {
            if (other.gameObject.tag == "Enemy")
            {
                if (!other.GetComponent<Character>().dead)
                {
                    float distance = other.Distance(gameObject.GetComponent<Collider2D>()).distance;
                    float distanceMultiplier = 1;
                    if (distance > 0) distanceMultiplier = (radius - distance) / radius;
                    Debug.Log("Distance multiplier: " + distanceMultiplier);
                    other.GetComponent<Character>().Damage((int)(damage * distanceMultiplier));
                }
            }
        }


        Destroy(gameObject);
    }
}
