using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : StandardEnemy
{

    Vector2 randomVector;
    [Header("Pursuit")]
    public float randomVectorMax;
    public float pursuitRotationRate;
    public float targetLead;

    [Header("Charge")]
    public float chargeAcceleration;
    public float chargeRange;
    public float chargeCooldown;
    float chargeReadyTime;

    public float chargePrepareTime;
    public float chargeRotationRate;
    public float chargeDuration;

    modes mode;

    [Header("Engine VFX")]
    public SpriteRenderer engine;
    public Color engineColor;
    public Color engineColorCharging;



    public override void Start()
    {
        base.Start();
        StartCoroutine(RandomMovement());

        engine.color = engineColor;
    }

    public enum modes
    {
        pursuit,
        preparingCharge,
        charge
    }

    private void FixedUpdate()
    {
        switch (mode)
        {
            case modes.pursuit:
                Pursuit();
                break;

            case modes.preparingCharge: 
                PrepareCharge();
                break;

            case modes.charge:
                Charge();
                break;

        }
    }


    void Pursuit()
    {
        //Move towards player
        if (player)
        {
            Vector2 targetPos = player.position;
            float distance = Vector2.Distance(transform.position, targetPos);
            if (distance > chargeRange) distance = chargeRange;
            float timeToReach = distance / 4 + chargePrepareTime;
            Vector2 interceptPos = targetPos + targetLead * player.GetComponent<Rigidbody2D>().velocity * timeToReach + randomVector * distance;

            distance = Vector2.Distance(interceptPos, transform.position);
            direction = (interceptPos - (Vector2)transform.position).normalized;
            float angle = Vector2.Angle(direction, transform.up);

            if (angle < 20 && distance < chargeRange && Time.time > chargeReadyTime && IsInScreen(0.05f)) mode = modes.preparingCharge;

            AvoidCollision();
            transform.up = Vector3.Slerp(transform.up, direction, pursuitRotationRate * Time.fixedDeltaTime);
            direction = transform.up;

            
            //transform.Find("DebugTarget").transform.position = interceptPos;
        }

        m_rigidbody.AddForce(transform.up * acceleration);

    }


    float chargeEnergy;
    void PrepareCharge()
    {
        chargeEnergy += Time.deltaTime / chargePrepareTime;
        float rotationRate = Mathf.Lerp(pursuitRotationRate, chargeRotationRate, chargeEnergy);
        engine.color = Color.Lerp(engineColor, engineColorCharging, chargeEnergy);
        if (chargeEnergy > 1)
        {
            chargeEnergy = 0;
            mode = modes.charge;
            StartCoroutine(ChargeDuration());
        }

        direction = player.position - transform.position;
        //transform.up = Vector3.Slerp(transform.up, direction, rotationRate * Time.deltaTime);
    }

    void Charge()
    {
        if(player) direction = player.position - transform.position;
        transform.up = Vector3.Slerp(transform.up, direction, chargeRotationRate * Time.deltaTime);
        m_rigidbody.AddForce(transform.up * chargeAcceleration);
    }

    IEnumerator ChargeDuration()
    {
        yield return new WaitForSeconds(chargeDuration);
        chargeReadyTime = Time.time + chargeCooldown;
        engine.color = engineColor;
        mode = modes.pursuit;
    }

    IEnumerator RandomMovement()
    {
        while(true)
        {
            randomVector = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * randomVectorMax;
            yield return new WaitForSeconds(Random.Range(0.3f, 1f));
        }

    }

}
