using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Weapons : MonoBehaviour
{
    public Mastermind mastermind;
    public float rateOfFireMultiplier;
    public float rateOfFire; //Antal skott spelaren kan avfyra per sekund
    float nextShotTime = 0f; // Tiden när spelaren kan skjuta nästa skott
    public Transform weapon;
    public AudioSource shotaudio;
    public Image Crosshair;
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
    [HideInInspector] public float projectileSize;
    [HideInInspector] public int spreadBulletCount;
    [HideInInspector] public float spread;
    [HideInInspector] public float spreadDamageMultiplier;

   /* [HideInInspector]*/ public float rapidFireEnergy;
    [HideInInspector] public float rapidFireEnergyMax;
    [HideInInspector] public float rapidFireMultiplier;

    [HideInInspector] public float standYourGroundMultiplier;
    [HideInInspector] public float standYourGroundMultiplierMax;
    [HideInInspector] public float standYourGroundChargeTime;
    [HideInInspector] public float standYourGroundUnchargeTime;
    [HideInInspector] public bool standYourGroundTrail;
    /*[HideInInspector]*/ public float movementMultiplier;
    [HideInInspector] public float movementMultiplierMax;
    [HideInInspector] public float movementChargeTime;
    [HideInInspector] public float movementUnchargeTime;

    [HideInInspector] public Sprite bigBulletSprite;
    [HideInInspector] public float canonDamageMultiplier;

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
        if(!mastermind.gamePaused)
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


    }

    public void StandardFireCheck()
    {
        continueSequence = false;
        if (Time.time > nextShotTime)
        {
            Crosshair.color = Color.white;
            Crosshair.fillAmount = 1;
            if (Input.GetMouseButton(0) && Time.time > nextShotTime)
            {
                nextShotTime = Time.time + 1 / rateOfFire;    // Sätter en tidpunkt när spelaren kan avfyra igen
                continueSequence = true;
            }
        }

        else
        {
            Crosshair.color = new Color(0.7f, 0.7f, 0.7f);
            Crosshair.fillAmount = 1 + (Time.time - nextShotTime) * rateOfFire;
        }



    }
    public void StandardFire()
    {
        SendMessage(fireMode, (Vector2)transform.up);
        shotaudio.PlayOneShot(shotaudio.clip);
    }

    public void FireStandardProjectile(Vector2 fireVector)
    {
        GameObject newProjectile = Instantiate(projectile, weapon.position, transform.rotation, mastermind.stuffContainer);
        newProjectile.GetComponent<PlayerProjectile>().weapons = this;
        newProjectile.transform.up = fireVector;
        newProjectile.GetComponent<Rigidbody2D>().velocity = projectileSpeed * fireVector;
        newProjectile.GetComponent<PlayerProjectile>().damage = projectileDamage;
        newProjectile.transform.localScale *= projectileSize;
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

        if (Time.time > nextShotTime && rapidFireEnergy >= 0)
            movementMultiplier += ((movementMultiplierMax - 1) / movementChargeTime) * Time.deltaTime;

        else movementMultiplier -= ((movementMultiplierMax - 1) / movementUnchargeTime) * Time.deltaTime;

        movementMultiplier = Mathf.Clamp(movementMultiplier, 1, movementMultiplierMax);
        GetComponent<PlayerMovement>().accelerationMultiplier = movementMultiplier;

        TrailRenderer trail = GetComponent<TrailRenderer>();
        float power = (movementMultiplier - 1) / (movementMultiplierMax - 1)*0.5f;
        Color color = trail.startColor;
        color.a = power;
        trail.startColor = color;
    }

    public void RapidFireCheck()
    {
        continueSequence = false;
        rapidFireEnergy += Time.deltaTime;
        if (rapidFireEnergy > rapidFireEnergyMax) rapidFireEnergy = rapidFireEnergyMax;

        if (Time.time > nextShotTime && rapidFireEnergy > 0)
        {
            Crosshair.color = Color.white;
            Crosshair.fillAmount = 1;
            if(Input.GetMouseButton(0))
            {
                continueSequence = true;
                rapidFireEnergy -= 1 / rateOfFire;
                nextShotTime = Time.time + 1 / (rateOfFire * rapidFireMultiplier);    // Sätter en tidpunkt när spelaren kan avfyra igen
            }
        }

        else
        {
            Crosshair.color = new Color(0.7f, 0.7f, 0.7f);
            float timeAmount = 1 + (Time.time - nextShotTime) * rateOfFire;
            float energyAmount = 1 + rapidFireEnergy * rateOfFire;

            Crosshair.fillAmount = Mathf.Min(timeAmount,energyAmount);

        }
    }

    public void ShotgunFire()
    {
        shotaudio.PlayOneShot(shotaudio.clip,1f);
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
        //float fireElapsedTime = 0;
        //float fireDelay = 0.02f;
    GameObject newProjectile = Instantiate(projectile, weapon.position, transform.rotation, mastermind.stuffContainer);
        newProjectile.GetComponent<PlayerProjectile>().weapons = this;
        newProjectile.GetComponent<SpriteRenderer>().sprite = bigBulletSprite;
        newProjectile.transform.up = fireVector;
        newProjectile.GetComponent<Rigidbody2D>().velocity = projectileSpeed / 2 * fireVector;
        newProjectile.GetComponent<PlayerProjectile>().damage = (int)(projectileDamage * canonDamageMultiplier);
        newProjectile.transform.localScale *= projectileSize;
       
        //newProjectile.transform.localScale *= 5f;
        //SendMessage(fireMode);
        //Vill att den inte ska kunna firea konstant utan typ ett skott per sekund
        /*
            if (Input.GetMouseButton(0) && Time.time > nextShotTime)
            {
                nextShotTime = Time.time - 2 / rateOfFire;    
            }
            else continueSequence = false;
            */

        //fireElapsedTime += Time.deltaTime;

        //if (Input.GetKeyDown(KeyCode.Mouse0) && fireElapsedTime >= fireDelay)
        //{
        //    fireElapsedTime = 0;
        //}

    }
}


