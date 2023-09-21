using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCannon : Enemy
{

    [Header("Weapon")]
    public float targetingRotationSpeed;
    public float attackingRotationSpeed;
    public float cooldown;
    float nextShotTime;

    public GameObject projectile;
    public float projectileSize;
    public float projectileSpeed;
    public int projectileDamage;

    public List<Transform> weapons;

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
        initialPosition.y = Random.Range(0.25f, 0.75f);
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

            //foreach (Transform weapon in weapons)
            //{
            //    weapon.GetComponent<LineRenderer>().enabled = true;
            //}
        }

    }

    void Target()
    {
        LeadTarget(targetingRotationSpeed);

        if (Vector2.Angle(transform.up, direction) < 10f)
        {
            mode = modes.attacking;
        }

        Shield();
    }

    void Attack()
    {
        LeadTarget(attackingRotationSpeed);

        if (Time.time > nextShotTime)
        {
            StartCoroutine(Fire());
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


    public float chargeTime;
    public float lineAlpha;

    IEnumerator Fire()
    {
        nextShotTime = Time.time + cooldown;

        //Randomly picks one of the least recently fired weapons to fire from.
        int r = Random.Range(0, weapons.Count/2);
        Transform weapon = weapons[r];
        weapons.RemoveAt(r);
        weapons.Add(weapon);

        LineRenderer lineRenderer = weapon.GetComponent<LineRenderer>();
        Gradient c = lineRenderer.colorGradient;
        GradientAlphaKey[] alphaKeys = c.alphaKeys;
        lineRenderer.enabled = true;

        for (float t=0;t<1;t += Time.deltaTime/chargeTime)
        {
            alphaKeys[0].alpha = t * lineAlpha;
            c.alphaKeys = alphaKeys;
            lineRenderer.colorGradient = c;
            yield return null;
        }

        GameObject newProjectile = Instantiate(projectile, weapon.position, weapon.rotation, mastermind.stuffContainer);
        newProjectile.transform.localScale *= projectileSize;
        newProjectile.GetComponent<Rigidbody2D>().velocity = projectileSpeed * weapon.up;
        newProjectile.GetComponent<EnemyProjectile>().damage = projectileDamage;

        weapon.GetComponent<LineRenderer>().enabled = false;
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
    public float minBase;
    public float maxBase;

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
            float shieldBase = Mathf.Lerp(minBase, maxBase, shieldFill);


            Vector2[] arcPoints = new Vector2[10];
            float startAngle = -arc / 2;
            float endAngle = arc / 2;
            int segments = 10;

            float angle = startAngle;
            float arcLength = endAngle - startAngle;
            for (int i = 0; i < segments; i++)
            {
                arcPoints[i].x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
                arcPoints[i].y = shieldBase + Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

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
