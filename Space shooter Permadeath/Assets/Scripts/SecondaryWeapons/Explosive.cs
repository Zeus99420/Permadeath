using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    public GameObject explosionPrefab;
    public float explosionDuration = 0.3f;
    public int maxDamage;
    int damage;
    public float radius;


    // Update is called once per frame
    public virtual IEnumerator Countdown(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(Explode());
    }

    public Color ringColor1;
    public Color ringColor2;
    public IEnumerator Explode()
    {
        transform.Find("Sprite").GetComponent<SpriteRenderer>().enabled = false;
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


}
