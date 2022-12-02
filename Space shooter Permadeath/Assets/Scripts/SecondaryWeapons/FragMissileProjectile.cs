using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragMissileProjectile : Explosive
{
    public Mastermind mastermind;
    public float acceleration;
    Rigidbody2D m_rigidbody;
    [HideInInspector] public Vector2 targetPosition;

    [HideInInspector] public GameObject shrapnel;
    [HideInInspector] public int shrapnelCount;
    [HideInInspector] public int shrapnelDamage;
    [HideInInspector] public float shrapnelVelocity;

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    public override IEnumerator Countdown(float delay)
    {
        yield return new WaitForSeconds(delay);
        acceleration = 0;
        StartCoroutine(Explode());
    }

    void FixedUpdate()
    {
        m_rigidbody.velocity += Time.fixedDeltaTime * acceleration * (Vector2)(transform.up);

    }

    //public IEnumerator Countdown(float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //    //Explode();
    //    StartCoroutine(Explode());
    //}
    IEnumerator ShrapnelExplosion()
    {
        transform.position = targetPosition;
        Instantiate(explosionPrefab, transform.position, transform.rotation);

        float angleIncrement = 360 / (shrapnelCount);
        Vector2 shrapnelVector = transform.up;

        for (int t = 0; t < shrapnelCount; t++)
        {
            GameObject newShrapnel = Instantiate(shrapnel, transform.position, transform.rotation);
            newShrapnel.GetComponent<Rigidbody2D>().velocity += shrapnelVelocity * shrapnelVector;
            shrapnelVector = Quaternion.Euler(0, 0, angleIncrement) * shrapnelVector;
            newShrapnel.GetComponent<PlayerProjectile>().damage = shrapnelDamage;
        }
        Destroy(gameObject);
        yield return null;
    }


}

