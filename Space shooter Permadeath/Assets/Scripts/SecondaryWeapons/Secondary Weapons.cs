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

    //MULTIPLIERS
    public float damageMultiplier = 1;
    public float radiusMultiplier = 1;
    public float rechargeRate = 1;
    public float rateOfFireMultiplier = 1;
    public float maxChargeMultiplier = 1;


    [Header("CHARGE INDICATOR")]
    //The indicator displays the when the weapon is recharging or ready to use
    public Transform AmmoIndicator;
    public GameObject AmmoImagePrefab;
    public Color readyColor;
    public Color chargingColor;
    public List<Image> ammoImages = new List<Image>();
    public List<Image> cooldownImages = new List<Image>();
    protected Weapons weapons;


    void Start()
    {
        weapons = GetComponent<Weapons>();
        AmmoIndicator = weapons.ammoIndicator;
        mastermind = weapons.mastermind;
        InitializeAmmoIndicator();
    }

    void Update()
    {
        if (!mastermind.gamePaused)
        {
            charges += Time.deltaTime * rechargeRate / rechargeTime;
            charges = Mathf.Clamp(charges, 0, maxCharges * maxChargeMultiplier);

            if (Input.GetMouseButton(1) && Time.time > nextShotTime && charges >= 1)
            {
                nextShotTime = Time.time + 1 / (rateOfFire * rateOfFireMultiplier);    // Sätter en tidpunkt när spelaren kan avfyra igen
                charges -= 1;
                UseWeapon();
            }

            UpdateAmmoIndicator();
        }
    }

    public virtual void UseWeapon()
    { }

    public virtual void LevelComplete()
    {
        StartCoroutine(QuickRecharge());
    }

    public IEnumerator QuickRecharge()
    {
        yield return new WaitForSeconds(0.5f);
        for (float t = 0; t < 1.5; t += Time.deltaTime)
        {
            charges += maxCharges * maxChargeMultiplier * 0.7f * Time.deltaTime;
            yield return null;
        }
    }






    public void InitializeAmmoIndicator()
    {
        //Creates a number of game objects with images to display the current number of charges
        ClearAmmoIndicator();

        float ammoImageSpace = 0.12f;
        float offset = -ammoImageSpace * ((maxCharges * maxChargeMultiplier) - 1) / 2f;
        for (int i = 0; i < maxCharges*maxChargeMultiplier; i++)
        {
            GameObject newAmmoImage = Instantiate(AmmoImagePrefab, AmmoIndicator);
            newAmmoImage.transform.position += new Vector3(offset, 0f, 0f);
            offset += ammoImageSpace;
            Image image = newAmmoImage.transform.Find("Image").GetComponent<Image>();
            ammoImages.Add(image);
            Image cooldownImage = newAmmoImage.transform.Find("Cooldown Image").GetComponent<Image>();
            cooldownImages.Add(cooldownImage);
        }
    }

    public void UpdateAmmoIndicator()
    {
        foreach (Image CDimage in cooldownImages)
        {

            CDimage.fillAmount = (nextShotTime - Time.time) * (rateOfFire * rateOfFireMultiplier);
        }


        int i = 0;
        foreach (Image image in ammoImages)
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

    public void ClearAmmoIndicator()
    {
        foreach (Transform transform in AmmoIndicator) Destroy(transform.gameObject);
        ammoImages.Clear();
        cooldownImages.Clear();
    }
}
