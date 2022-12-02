using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KineticRocketProjectile : PlayerProjectile
{
    public float acceleration;
    public float damageMultiplier;
    Rigidbody2D m_rigidbody;

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        m_rigidbody.AddForce(transform.up * acceleration);

    }




    public override void OnTriggerEnter2D(Collider2D other)
    {
        damage = (int)(m_rigidbody.velocity.magnitude * damageMultiplier);
        StandardHit(other);
    }
}

