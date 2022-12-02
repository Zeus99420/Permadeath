using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardProjectile : PlayerProjectile
{
    // ANVÄNDS AV UPPGRADERINGAR
    public Color standYourGroundColor;
    public bool piercing;
    public float piercingMultiplier;
    List<GameObject> alreadyHit = new List<GameObject>();

    private void Start()
    {
        if (weapons.standYourGroundTrail)
        {
            TrailRenderer trail = GetComponent<TrailRenderer>();
            trail.enabled = true;
            float power = (weapons.standYourGroundMultiplier - 1) / (weapons.standYourGroundMultiplierMax - 1);
            Color color = trail.startColor;
            color.a *= power;
            trail.startColor = color;
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.color = Color.Lerp(sprite.color, standYourGroundColor, power);

        }
        piercing = weapons.piercing;
        piercingMultiplier = weapons.piercingMultiplier;

    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (piercing) PiercingHit(other);
        else StandardHit(other);
    }



    public void PiercingHit(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" && !alreadyHit.Contains(other.gameObject))
        {
            alreadyHit.Add(other.gameObject);
            Character enemy = other.GetComponent<Character>();
            if (!enemy.dead)
            {
                float newDamage;
                if (damage < enemy.health) newDamage = damage * piercingMultiplier;
                else newDamage = damage - enemy.health * (1f - piercingMultiplier);
                enemy.Damage(damage);

                if (newDamage < 1) Destroy(gameObject);
                else damage = (int)newDamage;
            }
        }
    }
}
