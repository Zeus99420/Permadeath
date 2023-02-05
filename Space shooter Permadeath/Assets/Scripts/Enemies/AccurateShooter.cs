using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccurateShooter : Enemy
{
    //En fiende som försöker närma sig spelaren och skjuta den med projektiler



    float distance;

    public float attackRange;
    public float cooldown;
    float nextShotTime;

    public float strafeRange;

    float rotationSpeed;
    public float chargingRotationSpeed;
    public float pursuitRotationSpeed;

    public GameObject projectile;
    public float projectileSpeed;
    public int projectileDamage;
    public Transform weapon;

    public SpriteRenderer chargingProjectile;
    float weaponCharge;
    public float chargeTime;

    float randomLead;
    public float maxRandomLead;

    public override void Start()
    {
        base.Start();

        chargingProjectile = weapon.Find("chargingProjectile").GetComponent<SpriteRenderer>();
        rotationSpeed = pursuitRotationSpeed;
        randomLead = Random.Range(0, maxRandomLead);
    }

    public enum modes
    {
        pursuit,
        chargingWeapon
    }

    modes mode;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player)
        {
            Vector2 targetPos = player.position;
            distance = Vector2.Distance(transform.position, targetPos);         //((Vector2)transform.position - targetPos).magnitude;
            if (distance > attackRange) distance = attackRange;
            float timeToReach = distance / projectileSpeed + randomLead;
            Vector2 interceptPos = targetPos + player.GetComponent<Rigidbody2D>().velocity * timeToReach;


            distance = Vector2.Distance(interceptPos, transform.position);
            direction = (interceptPos - (Vector2)transform.position).normalized;


            Vector3 newRotation = Vector3.RotateTowards(transform.up, direction, rotationSpeed * Time.deltaTime, 0f);
            transform.up = newRotation;


            switch (mode)
            {
                case modes.pursuit:
                    Pursuit();
                    break;

                case modes.chargingWeapon:
                    ChargingWeapon();
                    break;
            }



        }

    }

    void Pursuit ()
    {
            //Fienden rör sig vanligtvis mot spelaren tills den kommer inom ett visst avstånd. Då rör den sig istället i sidled runt spelaren
            if (distance < strafeRange) direction = Vector2.Perpendicular(direction);


            // Skjuter på spelaren om den är inom räckhåll. En viss tid måste passera innan fienden kan skjuta igen.
            if (IsInScreen(0.05f) && distance < attackRange && Time.time > nextShotTime)
            {
                mode = modes.chargingWeapon;
                rotationSpeed = chargingRotationSpeed;

                chargingProjectile.enabled = true;
                Color color = chargingProjectile.color;
                color.a = 0;
                chargingProjectile.color = color;
            }
        
        AvoidCollision();
        m_rigidbody.AddForce(direction * acceleration);
    }



    void ChargingWeapon()
    {
        weaponCharge += Time.fixedDeltaTime / chargeTime;
        if (weaponCharge > 1) Fire();

        Color color = chargingProjectile.color;
        color.a = weaponCharge;
        chargingProjectile.color = color;
    }

    void Fire()
    {
        nextShotTime = Time.time + cooldown;
        weaponCharge = 0;

        GameObject newProjectile = Instantiate(projectile, weapon.position, transform.rotation, mastermind.stuffContainer);
        newProjectile.GetComponent<Rigidbody2D>().velocity = projectileSpeed * transform.up;
        newProjectile.GetComponent<ShooterProjectile>().damage = projectileDamage;

        mode = modes.pursuit;
        chargingProjectile.enabled = false;
        rotationSpeed = pursuitRotationSpeed;
        randomLead = Random.Range(0, maxRandomLead);
    }
}
