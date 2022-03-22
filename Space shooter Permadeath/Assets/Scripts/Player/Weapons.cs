using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapons : MonoBehaviour
{

    public float rateOfFire; //Antal skott spelaren kan avfyra per sekund
    float nextShotTime = 0f; // Tiden när spelaren kan skjuta nästa skott

    public GameObject projectile;
    public float projectileSpeed;
    public int baseDamage;
    int projectileDamage;


    //[System.Serializable] public delegate void StoreFunction();
    //[HideInInspector] public StoreFunction FireCheck;
    //[HideInInspector] public StoreFunction Fire;
    //public List<StoreFunction> preFireEffects = new List<StoreFunction>();

    [HideInInspector] public string fireCheckMode = "StandardFireCheck";
    [HideInInspector] public string fireMode = "StandardFire";
    [HideInInspector] public List<string> updateEffects = new List<string>();


    //VARIABLER ANVÄNDA AV UPPGRADERINGAR
    [HideInInspector] public int spreadBulletCount = 2;
    [HideInInspector] public int maxSpread = 0;
    [HideInInspector] public float spreadDamageMultiplier = 0;

    [HideInInspector] public float rapidFireEnergy = 0;
    [HideInInspector] public float rapidFireEnergyMax = 0;
    [HideInInspector] public float rapidFireMultiplier = 1;

    [HideInInspector] float standYourGroundMultiplier;
    [HideInInspector] public float standYourGroundMultiplierMax;
    [HideInInspector] public float standYourGroundChargeTime;
    [HideInInspector] public float standYourGroundUnchargeTime;



    void Update()
    {
        projectileDamage = baseDamage;
        foreach (string function in updateEffects)
        {
            SendMessage(function);
        }
        SendMessage(fireCheckMode);
    }

    public void StandardFireCheck()
    {
        if (Input.GetMouseButton(0) && Time.time > nextShotTime)
        {
            nextShotTime = Time.time + 1 / rateOfFire;    // Sätter en tidpunkt när spelaren kan avfyra igen
            SendMessage(fireMode);
        }
    }
    public void StandardFire()
    {
        GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
        newProjectile.GetComponent<Rigidbody2D>().velocity = projectileSpeed * transform.up;
        newProjectile.GetComponent<PlayerProjectile>().damage = projectileDamage;
        newProjectile.transform.localScale *= 1f;
    }




    // ALTERNATE MODES FROM UPGRADES

    public void StandYourGround()
    {
        if (!GetComponent<PlayerMovement>().usingEngines)
            standYourGroundMultiplier += ((standYourGroundMultiplierMax-1)/standYourGroundChargeTime) * Time.deltaTime;
        else standYourGroundMultiplier -= ((standYourGroundMultiplierMax-1) / standYourGroundUnchargeTime) * Time.deltaTime;
        standYourGroundMultiplier = Mathf.Clamp(standYourGroundMultiplier,1,standYourGroundMultiplierMax);
        projectileDamage = (int)(projectileDamage * standYourGroundMultiplier);
    }

    public void RapidFireCheck()
    {
        rapidFireEnergy += Time.deltaTime;
        if (rapidFireEnergy > rapidFireEnergyMax) rapidFireEnergy = rapidFireEnergyMax;

        if (Input.GetMouseButton(0) && Time.time > nextShotTime && rapidFireEnergy > 1/rateOfFire)
        {
            rapidFireEnergy -= 1 / rateOfFire;
            nextShotTime = Time.time + 1 / (rateOfFire*rapidFireMultiplier);    // Sätter en tidpunkt när spelaren kan avfyra igen
            //Fire();
            SendMessage(fireMode);
        }
    }

    public void ShotgunFire()
    {
        float angle = -maxSpread / 2;
        float angleIncrement = maxSpread / (spreadBulletCount + 1);
        projectileDamage = (int)(projectileDamage * spreadDamageMultiplier);
        for (int t = 0; t < spreadBulletCount; t++)
        {
            GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
            newProjectile.transform.Rotate(0,0, angle);
            newProjectile.GetComponent<Rigidbody2D>().velocity = projectileSpeed * newProjectile.transform.up;
            //newProjectile.GetComponent<PlayerProjectile>().damage = (int)(projectileDamage * spreadDamageMultiplier);
            newProjectile.GetComponent<PlayerProjectile>().damage = projectileDamage;
            newProjectile.transform.localScale *= Mathf.Sqrt((float)projectileDamage / (float)baseDamage);

            angle += angleIncrement;

        }
    }

    public void SlowBigBullet()
    {
        GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
        newProjectile.transform.Rotate(0, 0, 0);
        newProjectile.GetComponent<PlayerProjectile>().damage = projectileDamage;
        newProjectile.transform.localScale *= 5.2f;
    }
}
