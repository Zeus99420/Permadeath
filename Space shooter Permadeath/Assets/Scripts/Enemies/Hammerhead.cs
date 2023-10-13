using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hammerhead : Enemy
{
    Vector2 lastAngle;
    public float rotationSpeed;
    public float angle;
    public float leftRight;

    [Header("Prediction")]
    public float predictionSpeed;
    public float predictionDistance;

    [Header("Shield")]
    public float shieldRadius;
    public float shieldArc;
    public float shieldBase;

    public float shieldAlphaMin;
    public float shieldAlphaMax;

    public ArcShield shield;

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

    [Header("Turning")]
    public float minAcceleration;
    public float maxAcceleration;
    public float minRotationSpeed;
    public float maxRotationSpeed;
    public float turningPower;
    void Movement()
    {
        //direction = (player.position - transform.position).normalized;
        PredictPlayerPosition(predictionSpeed, predictionDistance);
        AvoidCollision();
        angle = Vector2.SignedAngle(transform.up, direction);
        leftRight = Mathf.Sign(angle);

        
        turningPower += (Mathf.Sqrt(Mathf.Abs(angle)) - 7) * 0.1f *  Time.fixedDeltaTime;
        turningPower = Mathf.Clamp01(turningPower);
        acceleration = Mathf.Lerp(maxAcceleration, minAcceleration, turningPower);
        rotationSpeed = Mathf.Lerp(minRotationSpeed, maxRotationSpeed, turningPower);

        //transform.up = Vector3.Slerp(transform.up, direction, rotationSpeed * Time.fixedDeltaTime);
        //m_rigidbody.AddTorque(rotationSpeed * leftRight * 0.1f);

        Vector3 newRotation = Vector3.RotateTowards(transform.up, direction, rotationSpeed * Time.deltaTime, 0f);
        transform.up = newRotation;

        m_rigidbody.AddForce(transform.up * acceleration);
    }







}
