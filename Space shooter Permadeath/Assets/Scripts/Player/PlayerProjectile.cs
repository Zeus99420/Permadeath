using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [HideInInspector] public int damage;
    [HideInInspector] public Weapons weapons;
    public Color standYourGroundColor;

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
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Enemy")
        {
            if (!other.GetComponent<Character>().dead)
            {
                other.GetComponent<Character>().Damage(damage);
                //Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
