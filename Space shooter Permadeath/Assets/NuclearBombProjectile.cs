using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuclearBombProjectile : MonoBehaviour
{
    public GameObject explosion;
    public float delay;
    public int damage;
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
        Explode();
    }

    void Explode()
    {
        Instantiate(explosion, transform.position, transform.rotation);

        Collider2D[] EnemiesHit = Physics2D.OverlapCircleAll(transform.position,radius);
        foreach (Collider2D other in EnemiesHit)
        {
            if (other.gameObject.tag == "Enemy")
            {
                if (!other.GetComponent<Character>().dead)
                {
                    other.GetComponent<Character>().Damage(damage);
                }
            }
        }


        Destroy(gameObject);
    }
}
