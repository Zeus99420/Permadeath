using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCannon : Enemy
{

    public float cooldown;
    float nextShotTime;

    [Header("Weapon")]
    public float targetingRotationSpeed;
    public float attackingRotationSpeed;

    public GameObject projectile;
    public float projectileSize;
    public float projectileSpeed;
    public int projectileDamage;


    //[Header("Ramp Up")]
    //public float rampUpTime;
    //public float initialCooldown;
    //public float finalCooldown;
    //public float initialProjectileSpeed;
    //public float finalProjectileSpeed;

    public List<Transform> weapons;

    public float salvoDuration;

    modes mode;
    public enum modes
    {
        moving,
        targeting,
        attacking
    }
    public override void Start()
    {
        base.Start();
        shieldHealth = initialShield;


        //Checks which side of the screen the player is on and moves to enter from the opposite side
        Vector2 initialPosition;
        if (Camera.main.WorldToViewportPoint(player.position).x < 0.5f)
        {
            initialPosition.x = 1.1f;
            transform.up = Vector2.left;
        }
        else
        {
            initialPosition.x = -0.1f;
            transform.up = Vector2.right;
        }
        initialPosition.y = Random.Range(0.3f, 0.7f);
        transform.position = Camera.main.ViewportToWorldPoint(initialPosition, 0);


        //Shuffle weapon order
        for (int t = 0; t < weapons.Count; t++)
        {
            Transform tmp = weapons[t];
            int r = Random.Range(t, weapons.Count);
            weapons[t] = weapons[r];
            weapons[r] = tmp;
        }

        //projectileSpeed = initialProjectileSpeed;

    }



    private void FixedUpdate()
    {
        switch (mode)
        {
            case modes.moving:
                Move();
                break;

            case modes.targeting:
                Target();
                break;

            case modes.attacking:
                Attack();
                break;
        }

    }

    // Update is called once per frame
    void Move()
    {
        direction = transform.up;

        AvoidCollision();
        m_rigidbody.AddForce(direction * acceleration);

        if (IsInScreen(0.15f))
        {
            mode = modes.targeting;

            foreach (Transform weapon in weapons)
            {
                weapon.GetComponent<LineRenderer>().enabled = true;
            }
        }

    }

    void Target()
    {
        LeadTarget(targetingRotationSpeed);

        if (Vector2.Angle(transform.up, direction) < 0.1f)
        //if ((Vector2)transform.up == direction)
        {
            mode = modes.attacking;
            //StartCoroutine(RampUp());
            StartCoroutine(Salvo());
        }

        Shield();
    }

    void Attack()
    {
        LeadTarget(attackingRotationSpeed);

        if (Time.time > nextShotTime)
        {
            Fire();
        }

        Shield();
    }

    void LeadTarget(float rotationSpeed)
    {
        Vector2 targetPos = player.position;
        float distance = Vector2.Distance(transform.position, targetPos);
        float timeToReach = distance / projectileSpeed;
        Vector2 interceptPos = targetPos + player.GetComponent<Rigidbody2D>().velocity * timeToReach;

        direction = (interceptPos - (Vector2)transform.position).normalized;

        Vector3 newRotation = Vector3.RotateTowards(transform.up, direction, rotationSpeed * Time.deltaTime, 0f);
        transform.up = newRotation;
    }



    //IEnumerator RampUp()
    //{
    //    float rampUp = 0;
    //    while (rampUp <1)
    //    {
    //        rampUp += Time.deltaTime / rampUpTime;
    //        if (rampUp > 1) rampUp = 1;
    //        cooldown = Mathf.Lerp(initialCooldown, finalCooldown, rampUp);
    //        projectileSpeed = Mathf.Lerp(initialProjectileSpeed, finalProjectileSpeed, rampUp);
    //        yield return null;
    //    }

    //}

    IEnumerator Salvo()
    {
        yield return new WaitForSeconds(salvoDuration);
        mode = modes.targeting;
    }

    void Fire()
    {
        nextShotTime = Time.time + cooldown;

        //Randomly picks one of the two least recently fired weapons to fire from.
        int r = Random.Range(0, 1);
        Transform weapon = weapons[r];
        weapons.RemoveAt(r);
        weapons.Add(weapon);

        GameObject newProjectile = Instantiate(projectile, weapon.position, transform.rotation, mastermind.stuffContainer);
        newProjectile.transform.localScale *= projectileSize;
        newProjectile.GetComponent<Rigidbody2D>().velocity = projectileSpeed * transform.up;
        newProjectile.GetComponent<ShooterProjectile>().damage = projectileDamage;
    }
    [Header("Shield")]
    public float initialShield;
    public float shieldMaxHealth;
    public float shieldRegen;
    public LineRenderer shieldRenderer;
    public EdgeCollider2D shieldCollider;

    [Header("Shield Dimensions")]
    public float minRadius;
    public float maxRadius;
    public float minArc;
    public float maxArc;

    void Shield()
    {
        shieldHealth += shieldRegen * Time.deltaTime;
        if (shieldHealth > shieldMaxHealth) shieldHealth = shieldMaxHealth;

        if (shieldHealth <= 0)
        {
            shieldCollider.enabled = false;
            shieldRenderer.enabled = false;        
        }

        else
        {
            shieldCollider.enabled = true;
            shieldRenderer.enabled = true;

            float shieldFill = (float)shieldHealth / shieldMaxHealth;
            float arc = Mathf.Lerp(minArc, maxArc, shieldFill);
            float radius = Mathf.Lerp(minRadius, maxRadius, shieldFill);


            Vector2[] arcPoints = new Vector2[10];
            float startAngle = -arc / 2;
            float endAngle = arc / 2;
            int segments = 10;

            float angle = startAngle;
            float arcLength = endAngle - startAngle;
            for (int i = 0; i < segments; i++)
            {
                arcPoints[i].x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
                arcPoints[i].y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

                shieldRenderer.SetPosition(i, arcPoints[i]);

                angle += (arcLength / segments);
            }

            shieldCollider.points = arcPoints;




            //Sköldens blir mindre genomskinlig när den får mer liv.
            //float shieldAlpha = shieldHealth / shieldMaxHealth * 0.6f;
            //Gradient gradient = new Gradient();
            //gradient.SetKeys(
            //    new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.red, 1.0f) },
            //    new GradientAlphaKey[] { new GradientAlphaKey(shieldAlpha, 0.0f), new GradientAlphaKey(shieldAlpha, 1.0f) }
            //);
            //shieldRenderer.colorGradient = gradient;
        }


    }

    public override void ShieldDamage(int damageAmount)
    {
        shieldHealth -= damageAmount;
    }
}
