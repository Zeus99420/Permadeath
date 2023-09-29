using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmRocket : PlayerProjectile
{
    Rigidbody2D rigidBody;
    public float acceleration;
    public Vector2 targetPosition;
    public float engineDelay;

    bool engineRunning = false;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        StartCoroutine(Launch());

        destroyWhenInvisible = false;

    }

    IEnumerator Launch()
    {
        Vector2 initialPosition = (Vector2)transform.position + rigidBody.velocity * engineDelay;
        Vector2 interceptPos = targetPosition;
        float timeToReach;
        float distance;

        for (int i = 0; i < 10; i++)
        {
            distance = (initialPosition - interceptPos).magnitude;
            timeToReach = Mathf.Sqrt(distance / (0.5f * acceleration));
            interceptPos = targetPosition - GetComponent<Rigidbody2D>().velocity * timeToReach;
        }

        float engineTime = Time.time + engineDelay;
        yield return new WaitForSeconds(engineDelay/4);
        
        Vector2 targetRotation = (interceptPos - initialPosition).normalized;

        while (Time.time < engineTime)
        {
            Vector3 newRotation = Vector3.RotateTowards(transform.up, targetRotation, 10f*Time.deltaTime, 0f);
            transform.up = newRotation;
            yield return null;
        }

        transform.up = targetRotation;

        engineRunning = true;
        GetComponent<ParticleSystem>().Play();
    }

    void FixedUpdate()
    {
        if (engineRunning) rigidBody.velocity += Time.fixedDeltaTime * acceleration * (Vector2)(transform.up);
    }

    private void Update()
    {
        if (!IsInScreen(-0.5f))
        {
            Destroy(gameObject);
        }
    }

}
