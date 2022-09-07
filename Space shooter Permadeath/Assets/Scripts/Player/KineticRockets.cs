using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        InitializeAmmoIndicator();
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

            UpdateAmmoIndicator();
        }
    }


    void FireRocket()
    {
        GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation, mastermind.stuffContainer);
        //newProjectile.GetComponent<PlayerProjectile>().weapons = this;
        newProjectile.GetComponent<KineticRocketProjectile>().acceleration = projectileAcceleration;
        newProjectile.GetComponent<KineticRocketProjectile>().damageMultiplier = damageMultiplier;
    }


    [Header("Ammo Indicator")]
    public GameObject AmmoIndicator;
    public GameObject AmmoImagePrefab;
    public Color readyColor;
    public Color chargingColor;
    public List<Image> AmmoImages;
    void InitializeAmmoIndicator ()
    {
        //Creates a number of game objects with an images to display the current number of charges
        float ammoImageSpace = 0.12f;
        float offset = -ammoImageSpace * (maxCharges-1) / 2f;
        for (int i=0; i<maxCharges; i++)
        {
            GameObject newAmmoImage = Instantiate(AmmoImagePrefab,AmmoIndicator.transform);
            newAmmoImage.transform.position += new Vector3(offset, 0f, 0f);
            offset += ammoImageSpace;

            Image image = newAmmoImage.transform.Find("Image").GetComponent<Image>();
            AmmoImages.Add(image);
        }
    }

    void UpdateAmmoIndicator()
    {
        int i = 0;
        foreach (Image image in AmmoImages)
        {
            if (charges >=i+1)
            {
                image.color = readyColor;
                image.fillAmount = 1;
            }
            else
            {
                image.color = chargingColor;
                image.fillAmount = charges - i;
            }           
            i++;
        }
    }




}
