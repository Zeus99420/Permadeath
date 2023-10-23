using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcShield : AreaShield
{
    public float maxHealth;
    public float initialHealth;
    [HideInInspector] public float health;
    public float regen;

    public EdgeCollider2D shieldCollider;
    public LineRenderer shieldRenderer;
    float lastDamageTime;
    public Color normalColor;
    public Color damagedColor;
    public Color brokenColor;
    public float alphaMin;
    public float alphaMax;

    Gradient gradient = new Gradient();


    private void Start() 
    {
        health = initialHealth;
    }

    public void ShieldUpdate()
    {
        health += regen * Time.deltaTime;
        if (health > maxHealth) health = maxHealth;
        SetAppearance();

        if (health <= 0)
        {
            shieldCollider.enabled = false;
        }

        else
        {
            shieldCollider.enabled = true;
        }
    }

    public override void Damage(Collider2D collider, int damageAmount)
    {
        health -= damageAmount;
        lastDamageTime = Time.time;
    }

    public override void Collision(Collider2D collider, GameObject other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (health >= collisionSelfDamage)
        {
            player.Damage(collisionDamageMax);
        }

        else
        {
            int collisionDamage = (int)(collisionDamageMax * (health / collisionSelfDamage));
            player.Damage(collisionDamage);
        }

        Damage(collider, collisionSelfDamage);
    }

    public override float GetHealth(Collider2D collider)
    {
        return health;
    }



    public void SetPosition(float offset, float arc, float radius)
    {


        Vector2[] arcPoints = new Vector2[10];
        float startAngle = -arc / 2;
        float endAngle = arc / 2;
        int segments = 10;

        float angle = startAngle;
        float arcLength = endAngle - startAngle;
        for (int i = 0; i < segments; i++)
        {
            arcPoints[i].x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            arcPoints[i].y = offset + Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            shieldRenderer.SetPosition(i, arcPoints[i]);

            angle += (arcLength / segments);
        }

        shieldCollider.points = arcPoints;
    }

    public void SetAppearance()
    {
        shieldRenderer.enabled = true;
        float alpha=0;
        Color color;

        if (health < 0)
        {
            color = brokenColor;
            alpha = Mathf.Lerp(0.4f, 0, (Time.time - lastDamageTime) / 0.15f);
        }

        else
        {
            if (Time.time < lastDamageTime + 0.05f)
            {
                color = damagedColor;
                alpha = 0.9f;
            }

            else
            {
                color = normalColor;
                alpha = Mathf.Lerp(alphaMin, alphaMax, health / maxHealth);
            }
        }


        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(color,0.0f), new GradientColorKey(color,1.0f)},
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
        shieldRenderer.colorGradient = gradient;
    }




}
