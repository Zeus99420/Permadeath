using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hammerhead : Enemy
{
    Vector2 lastAngle;
    public float rotationSpeed;
    public float angle;
    public float leftRight;

    [Header("Shield Dimensions")]
    public float shieldRadius;
    public float shieldArc;
    public float shieldBase;

    public override void Start()
    {
        base.Start();
        shield.SetPosition(shieldBase, shieldArc, shieldRadius);
    }
    private void FixedUpdate()
    {
        Movement();
        shield.ShieldUpdate();
        shield.SetAlpha(shieldAlphaMin, shieldAlphaMax);
    }

    void Movement()
    {
        direction = (player.position - transform.position).normalized;
        angle = Vector2.SignedAngle(transform.up, direction);
        leftRight = Mathf.Sign(angle);
        m_rigidbody.AddTorque(rotationSpeed * leftRight * 0.1f);

        m_rigidbody.AddForce(transform.up * acceleration);
    }

    [Header("Shield")]
    public float shieldAlphaMin;
    public float shieldAlphaMax;
    public LineRenderer shieldRenderer;
    Gradient shieldGradient = new Gradient();

    public AreaShield shield;





}
