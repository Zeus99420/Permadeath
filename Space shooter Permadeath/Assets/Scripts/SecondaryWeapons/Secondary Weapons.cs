using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondaryWeapons : MonoBehaviour
{
    public Mastermind mastermind;

    public float rechargeTime;
    public float charges;
    public int maxCharges;

    public float rateOfFire; //Antal skott spelaren kan avfyra per sekund
    float nextShotTime = 0f; // Tiden när spelaren kan skjuta nästa skott

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

            if (Input.GetMouseButton(1) && Time.time > nextShotTime && charges >= 1)
            {
                nextShotTime = Time.time + 1 / rateOfFire;    // Sätter en tidpunkt när spelaren kan avfyra igen
                charges -= 1;
                UseWeapon();
            }

            UpdateAmmoIndicator();
        }
    }

    public virtual void UseWeapon()
    {

    }





    [Header("CHARGE INDICATOR")]
    //The indicator displays the when the weapon is recharging or ready to use
    public GameObject AmmoIndicator;
    public GameObject AmmoImagePrefab;
    public Color readyColor;
    public Color chargingColor;
    public List<Image> AmmoImages;
    public void InitializeAmmoIndicator()
    {
        //Creates a number of game objects with an images to display the current number of charges
        float ammoImageSpace = 0.12f;
        float offset = -ammoImageSpace * (maxCharges - 1) / 2f;
        for (int i = 0; i < maxCharges; i++)
        {
            GameObject newAmmoImage = Instantiate(AmmoImagePrefab, AmmoIndicator.transform);
            newAmmoImage.transform.position += new Vector3(offset, 0f, 0f);
            offset += ammoImageSpace;

            Image image = newAmmoImage.transform.Find("Image").GetComponent<Image>();
            AmmoImages.Add(image);
        }
    }

    public void UpdateAmmoIndicator()
    {
        int i = 0;
        foreach (Image image in AmmoImages)
        {
            if (charges >= i + 1)
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
