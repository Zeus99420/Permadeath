using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KineticRockets : MonoBehaviour
{
    public Mastermind mastermind;
    public GameObject projectile;

    public float rechargeTime;
    public float charges;
    public int maxCharges;

    public float rateOfFire; //Antal skott spelaren kan avfyra per sekund
    float nextShotTime = 0f; // Tiden när spelaren kan skjuta nästa skott

    public float projectileAcceleration;
    public int damageMultiplier;


    void Start()
    {
        
    }

    void Update()
    {
        if (!mastermind.gamePaused)
        {
            charges += Time.deltaTime * 1 / rechargeTime;
            charges = Mathf.Clamp(charges, 0, maxCharges);

            if (Input.GetMouseButton(1) && Time.time > nextShotTime && charges > 1)
            {
                nextShotTime = Time.time + 1 / rateOfFire;    // Sätter en tidpunkt när spelaren kan avfyra igen
                charges -= 1;
                FireRocket();
            }
        }
    }


    void FireRocket()
    {
        GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation, mastermind.stuffContainer);
        //newProjectile.GetComponent<PlayerProjectile>().weapons = this;
        newProjectile.GetComponent<KineticRocketProjectile>().acceleration = projectileAcceleration;
        newProjectile.GetComponent<KineticRocketProjectile>().damageMultiplier = damageMultiplier;
    }




}
