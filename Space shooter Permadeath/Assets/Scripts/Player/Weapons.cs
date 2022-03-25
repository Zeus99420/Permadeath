using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapons : MonoBehaviour
{
    public Mastermind mastermind;

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

    [HideInInspector] public string fireMode;

    public struct EnabledMethods
    {
        public EnabledMethods(string name, bool enabled)
        {
            this.name = name;
            this.enabled = enabled;
        }
        public string name;
        public bool enabled;
    }

    public List<EnabledMethods> weaponsSequence = new List<EnabledMethods>
    {
        new EnabledMethods("StandYourGround",false),
        new EnabledMethods("StandardFireCheck",true),
        new EnabledMethods("RapidFireCheck",false),
        new EnabledMethods("ShotgunFire",false),
        new EnabledMethods("StandardFire",true),
    };

    public List<string> methodSequence;
    public List<bool> enabledSequence;
    bool continueSequence;
    int sequenceStep;

    //VARIABLER ANVÄNDA AV UPPGRADERINGAR
    [HideInInspector] public int spreadBulletCount;
    [HideInInspector] public float spread;
    [HideInInspector] public float spreadDamageMultiplier;

    [HideInInspector] public float rapidFireEnergy;
    [HideInInspector] public float rapidFireEnergyMax;
    [HideInInspector] public float rapidFireMultiplier;

    [HideInInspector] float standYourGroundMultiplier;
    [HideInInspector] public float standYourGroundMultiplierMax;
    [HideInInspector] public float standYourGroundChargeTime;
    [HideInInspector] public float standYourGroundUnchargeTime;

    private void Start()
    {
        foreach (EnabledMethods method in weaponsSequence)
        {
            methodSequence.Add(method.name);
            enabledSequence.Add(method.enabled);
        }
    }

    void Update()
    {
        projectileDamage = baseDamage;
        continueSequence = true;
        sequenceStep = 0;
        while (continueSequence && sequenceStep < methodSequence.Count)
        {
            if (enabledSequence[sequenceStep] == true)
            {
                SendMessage(methodSequence[sequenceStep]);
            }
            sequenceStep++;
        }

    }

    public void StandardFireCheck()
    {
        if (Input.GetMouseButton(0) && Time.time > nextShotTime)
        {
            nextShotTime = Time.time + 1 / rateOfFire;    // Sätter en tidpunkt när spelaren kan avfyra igen
        }
        else continueSequence = false;
    }
    public void StandardFire()
    {
        SendMessage(fireMode, (Vector2)transform.up);
    }

    public void FireStandardProjectile(Vector2 fireVector)
    {
        GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation, mastermind.projectilesContainer);
        newProjectile.GetComponent<Rigidbody2D>().velocity = projectileSpeed * fireVector;
        newProjectile.GetComponent<PlayerProjectile>().damage = projectileDamage;
        newProjectile.transform.localScale *= Mathf.Sqrt((float)projectileDamage / (float)baseDamage);
    }




    // UPPGRADERINGARS FUKTIONER
    //Dessa funktioner används om spelaren köpt motsvarande uppgraderingar

    public void StandYourGround()
    {
        if (!GetComponent<PlayerMovement>().usingEngines)
            standYourGroundMultiplier += ((standYourGroundMultiplierMax - 1) / standYourGroundChargeTime) * Time.deltaTime;
        else standYourGroundMultiplier -= ((standYourGroundMultiplierMax - 1) / standYourGroundUnchargeTime) * Time.deltaTime;
        standYourGroundMultiplier = Mathf.Clamp(standYourGroundMultiplier, 1, standYourGroundMultiplierMax);
        projectileDamage = (int)(projectileDamage * standYourGroundMultiplier);
    }

    public void RapidFireCheck()
    {
        rapidFireEnergy += Time.deltaTime;
        if (rapidFireEnergy > rapidFireEnergyMax) rapidFireEnergy = rapidFireEnergyMax;

        if (Input.GetMouseButton(0) && Time.time > nextShotTime && rapidFireEnergy > 1 / rateOfFire)
        {
            rapidFireEnergy -= 1 / rateOfFire;
            nextShotTime = Time.time + 1 / (rateOfFire * rapidFireMultiplier);    // Sätter en tidpunkt när spelaren kan avfyra igen
        }
        else continueSequence = false;
    }

    public void ShotgunFire()
    {
        float angle = -spread / 2;
        float angleIncrement = spread / (spreadBulletCount - 1);

        Vector2 fireVector = transform.up;
        fireVector = Quaternion.Euler(0, 0, angle) * fireVector;

        projectileDamage = (int)(projectileDamage * spreadDamageMultiplier);
        for (int t = 0; t < spreadBulletCount; t++)
        {
            SendMessage(fireMode, fireVector);
            fireVector = Quaternion.Euler(0, 0, angleIncrement) * fireVector;
        }
    }

    public void FireBigSlowBullet(Vector2 fireVector)
    {
        float fireElapsedTime = 0;
        float fireDelay = 0.02f;
    GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation, mastermind.projectilesContainer);
        newProjectile.GetComponent<Rigidbody2D>().velocity = projectileSpeed / 2 * fireVector;
        newProjectile.GetComponent<PlayerProjectile>().damage = projectileDamage * 3;
        newProjectile.transform.localScale *= 5f;
        //SendMessage(fireMode);
        //Vill att den inte ska kunna firea konstant utan typ ett skott per sekund
        /*
            if (Input.GetMouseButton(0) && Time.time > nextShotTime)
            {
                nextShotTime = Time.time - 2 / rateOfFire;    
            }
            else continueSequence = false;
            */

        fireElapsedTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Mouse0) && fireElapsedTime >= fireDelay)
        {
            fireElapsedTime = 0;
        }

    }
}


