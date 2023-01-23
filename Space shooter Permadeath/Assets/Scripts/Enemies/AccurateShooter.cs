using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccurateShooter : Enemy
{
    //En fiende som f�rs�ker n�rma sig spelaren och skjuta den med projektiler



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

    public override void Start()
    {
        base.Start();

        chargingProjectile = weapon.Find("chargingProjectile").GetComponent<SpriteRenderer>();
        rotationSpeed = pursuitRotationSpeed;
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
            Vector2 interceptPos = targetPos;
            distance = ((Vector2)transform.position - interceptPos).magnitude;
            float timeToReach = distance / projectileSpeed;
            interceptPos = targetPos + player.GetComponent<Rigidbody2D>().velocity * timeToReach;


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
            //Fienden r�r sig vanligtvis mot spelaren tills den kommer inom ett visst avst�nd. D� r�r den sig ist�llet i sidled runt spelaren
            if (distance < strafeRange) direction = Vector2.Perpendicular(direction);


            // Skjuter p� spelaren om den �r inom r�ckh�ll. En viss tid m�ste passera innan fienden kan skjuta igen.
            if (IsInScreen(0.05f) && distance < attackRange && Time.time > nextShotTime)
            {
                mode = modes.chargingWeapon;
                chargingProjectile.enabled = true;
                rotationSpeed = chargingRotationSpeed;
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
    }
}
