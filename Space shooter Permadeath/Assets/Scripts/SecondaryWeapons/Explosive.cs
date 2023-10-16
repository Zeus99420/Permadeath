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
    public float friendlyDamageMultiplier;

    protected bool detonated = false;
    [HideInInspector] public Coroutine countdown;

    List<Character> alreadyHit = new List<Character>();

    public virtual IEnumerator Countdown(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(Explode());
    }

    public Color ringColor1;
    public Color ringColor2;
    public IEnumerator Explode()
    {
        OnExplosion();
        detonated = true;
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

    public virtual void OnExplosion() { }




    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!detonated)
        {
            StopCoroutine(countdown);
            StartCoroutine(Explode());
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!detonated)
        {
            StopCoroutine(countdown);
            StartCoroutine(Explode());
        }


        else
        {
            Character target = other.GetComponentInParent<Character>();

            if (other.gameObject.tag == "EnemyShield" && !alreadyHit.Contains(target))
            {
                ShieldHit(other, damage);
                alreadyHit.Add(target);

            }

            else if (other.gameObject.tag == "Enemy" && !alreadyHit.Contains(target))
            {
                alreadyHit.Add(target);
                if (CheckForShield(other.transform) == false)
                {
                    Hit(target, damage);
                }
            }


            else if (other.gameObject.tag == "Player" && friendlyDamageMultiplier != 0 && !alreadyHit.Contains(target))
            {
                Hit(target, (int)(damage * friendlyDamageMultiplier));
            }
        }
 
    }

    bool CheckForShield(Transform other)
    {
        Vector2 direction = (other.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, other.position);
        LayerMask layerMask = LayerMask.GetMask("EnemyShield");
        if (Physics2D.Raycast(transform.position, direction,distance,layerMask).collider == null) return false;
        else return true;
    }

    void Hit(Character target, int damage)
    {
        alreadyHit.Add(target);
        if (!target.dead)
        {
            target.Damage(damage);
        }
    }

    //void ShieldHit(Enemy target, int damage)
    void ShieldHit(Collider2D collider, int damage)
    {
        Enemy target = collider.GetComponentInParent<Enemy>();
        alreadyHit.Add(target);
        target.ShieldDamage(collider, damage);
    }


}
