using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class RailGunStats
{
    public float projectileSpeed;
    public float projectileSize;
     public int damage;
    public float rotationSpeed;
    public float chargeTime;
    public float cooldown;

    public float randomLead;
    public float randomOffset;

}

public class RailgunTurret : EnemyArmament
{
    public RailGunStats stats;
    public float lineAlpha;

    public GameObject projectile;
    public Transform weapon;

    float extraLead;
    Vector2 offset;

    [HideInInspector] public bool firing;
    [HideInInspector] public bool ready=true;

    public LineRenderer lineRenderer;

    public override void Initialize()
    {
        base.Initialize();
        SetRandom();
    }

    void Update()
    {
        if (!firing) Track();
    }

    void Track()
    {
        LeadTarget(stats.projectileSpeed,offset,default, stats.chargeTime +extraLead);
        transform.up = Vector3.Slerp(transform.up, interceptDirection, stats.rotationSpeed * Time.deltaTime);
    }

    void SetRandom()
    {
        extraLead = Random.Range(stats.randomLead/2f, stats.randomLead);
        offset.x = Random.Range(-stats.randomOffset, stats.randomOffset);
        offset.y = Random.Range(-stats.randomOffset, stats.randomOffset);
    }

    public IEnumerator Fire()
    {
        firing = true;
        ready = false;

        Gradient c = lineRenderer.colorGradient;
        GradientAlphaKey[] alphaKeys = c.alphaKeys;
        lineRenderer.enabled = true;

        for (float t = 0; t < 1; t += Time.deltaTime / stats.chargeTime)
        {
            alphaKeys[0].alpha = t * lineAlpha;
            c.alphaKeys = alphaKeys;
            lineRenderer.colorGradient = c;
            yield return null;
        }

        GameObject newProjectile = Instantiate(projectile, weapon.position, weapon.rotation, mastermind.stuffContainer);
        newProjectile.transform.localScale *= stats.projectileSize;
        newProjectile.GetComponent<Rigidbody2D>().velocity = stats.projectileSpeed * weapon.up;
        newProjectile.GetComponent<EnemyProjectile>().damage = stats.damage;

        weapon.GetComponent<LineRenderer>().enabled = false;
        firing = false;
        SetRandom();
        yield return new WaitForSeconds(stats.cooldown);
        ready = true;
    }
}
