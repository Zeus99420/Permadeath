using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapons : MonoBehaviour
{

    public float rateOfFire; //Antal skott spelaren kan avfyra per sekund
    float nextShotTime = 0f; // Tiden när spelaren kan skjuta nästa skott

    public GameObject projectile;
    public float projectileSpeed;
    public int projectileDamage;


    public delegate void StoreFunction();
    [HideInInspector] public StoreFunction FireCheck;
    [HideInInspector] public StoreFunction Fire;


    //VARIABLER ANVÄNDA AV UPPGRADERINGAR
    [Header("Upgrade Variables")]
    public int spreadBulletCount = 2;
    public int maxSpread = 0;
    public float spreadDamageMultiplier = 0;
    public float rapidFireEnergy = 0;
    public float rapidFireEnergyMax = 0;
    public float rapidFireMultiplier = 1;



    // Start is called before the first frame update
    void Start()
    {
        Fire = StandardFire;
        FireCheck = StandardFireCheck;
    }

    // Update is called once per frame
    void Update()
    {
        FireCheck();
    }

    public void StandardFireCheck()
    {
        if (Input.GetMouseButton(0) && Time.time > nextShotTime)
        {
            nextShotTime = Time.time + 1 / rateOfFire;    // Sätter en tidpunkt när spelaren kan avfyra igen
            Fire();
        }
    }
    public void StandardFire()
    {
        GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
        newProjectile.GetComponent<Rigidbody2D>().velocity = projectileSpeed * transform.up;
        newProjectile.GetComponent<PlayerProjectile>().damage = projectileDamage;
    }

    // ALTERNATE MODES FROM UPGRADES

    public void RapidFireCheck()
    {
        rapidFireEnergy += Time.deltaTime;
        if (rapidFireEnergy > rapidFireEnergyMax) rapidFireEnergy = rapidFireEnergyMax;

        if (Input.GetMouseButton(0) && Time.time > nextShotTime && rapidFireEnergy > 1/rateOfFire)
        {
            rapidFireEnergy -= 1 / rateOfFire;
            nextShotTime = Time.time + 1 / (rateOfFire*rapidFireMultiplier);    // Sätter en tidpunkt när spelaren kan avfyra igen
            Fire();
        }
    }

    public void ShotgunFire()
    {
        Debug.Log("Shotgun Fired");
        int angle = -maxSpread / 2;
        int angleIncrement = maxSpread / (spreadBulletCount + 1);
        Debug.Log(angleIncrement);
        //int angle = 90;
        for (int t = 0; t < spreadBulletCount; t++)
        {
            GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
            newProjectile.transform.localScale *= 0.5f;
            newProjectile.transform.Rotate(0,0, angle);
            newProjectile.GetComponent<Rigidbody2D>().velocity = projectileSpeed * newProjectile.transform.up;
            newProjectile.GetComponent<PlayerProjectile>().damage = (int)(projectileDamage * spreadDamageMultiplier);

            angle += angleIncrement;

        }
    }
}
