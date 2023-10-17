using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable] public class PdcStats
{
    public int damage;
    public float rateOfFire;
    public float heatPerShot;
    public float heatRecovery;
    public float overheatRecovery;
    public float attackRange;
    public float projectileSpeed;
    public float projectileSize;
    public float maxTraverse;
    public float rotationSpeed;

    //public float randomLead;
    //public float randomOffset;
}

public class PDC : EnemyArmament
{
    public PdcStats s;

    public GameObject projectile;
    public Transform turret;
    public SpriteRenderer turretRenderer;
    public List<Transform> guns;
    public List<SpriteRenderer> gunRenderers;

    public Color normal;
    public Color heated;
    public Color overheatedMax;
    public Color overheatedMin;

    public float heat;
    bool overheated;
    int nextGun;
    float nextShotTime;
    float traverse;
    float relativeAngle;
    bool inSight;

    private void Update()
    {
        Track();

        if (overheated) Overheated();
        else
        {
            heat -= Time.deltaTime / s.heatRecovery;
            if (heat < 0) heat = 0;
            if (nextShotTime < Time.time && IsInScreen(0) && interceptDistance < s.attackRange && inSight) Fire();
            foreach (SpriteRenderer sprite in gunRenderers) sprite.color = Color.Lerp(normal, heated, heat);
            turretRenderer.color = Color.Lerp(normal, heated, heat);
        }
    }

    void Track()
    {
        LeadTarget(s.projectileSpeed, s.attackRange);
        float targetAngle = Vector2.SignedAngle(transform.up, interceptDirection);
        if (Mathf.Abs(targetAngle) < s.maxTraverse) inSight = true;
        else inSight = false;
        targetAngle = Mathf.Clamp(targetAngle, -s.maxTraverse, s.maxTraverse);
        traverse = Mathf.Lerp(traverse, targetAngle, s.rotationSpeed * Time.deltaTime);
        turret.localRotation = Quaternion.Euler(0, 0, traverse);

        //relativeAngle = Vector2.Angle(turret.up, interceptDirection);
    }

    void Fire()
    {
        nextShotTime = Time.time + 1f / s.rateOfFire;
        heat += s.heatPerShot;
        if (heat > 1) overheated = true;
        nextGun = (nextGun + 1) % 2;

        GameObject newProjectile = Instantiate(projectile, guns[nextGun].position, turret.rotation, mastermind.stuffContainer);
        newProjectile.transform.localScale *= s.projectileSize;
        newProjectile.GetComponent<Rigidbody2D>().velocity = s.projectileSpeed * turret.up;
        newProjectile.GetComponent<EnemyProjectile>().damage = s.damage;
    }
    void Overheated()
    {
        heat -= Time.deltaTime / s.overheatRecovery;
       foreach (SpriteRenderer sprite in gunRenderers) sprite.color = Color.Lerp(overheatedMin, overheatedMax, heat);
       turretRenderer.color = Color.Lerp(overheatedMin, overheatedMax, heat);

        if (heat <0)
        {
            heat = 0;
            overheated = false;
        }
    }
}
