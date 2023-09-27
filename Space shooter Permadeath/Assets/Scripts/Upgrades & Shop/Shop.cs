    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class UIelement
{
    public RectTransform transform;
    public Vector2 offset;
}

public class Shop : MonoBehaviour
{

    public Mastermind mastermind;
    public GameObject buttonPrefab;
    public UIScreen RMBInstructions;
    public List<Upgrades> standardUpgrades;
    public List<Upgrades> secondaryWeapons;
    public List<Button> buttons;
    public List<Upgrades> availableUpgrades;

    public Text tooltip;

    bool secWeaponPicked = false;
    List<Upgrades> upgrades;

    public UIelement[] UIelements;
    public void Initialize()
    {
        //upgrades.AddRange(GetComponentsInChildren<Upgrades>());
        standardUpgrades.AddRange(transform.Find("Upgrades").GetComponents<Upgrades>());
        secondaryWeapons.AddRange(transform.Find("Secondary Weapons").GetComponents<Upgrades>());
    }

    public void EnterShop()
    {
        StartCoroutine(SlideIn());

        //Determine whether the player should get to pick between a secondary weapon or a standard upgrade.
        if (!secWeaponPicked && Random.value < 0.5)
        {
            upgrades = secondaryWeapons;
            transform.Find("PickText").GetComponent<Text>().text = "Pick a Secondary Weapon:";
            StartCoroutine(RMBInstructions.FadeIn());
            
        }
        else
        {
            upgrades = standardUpgrades;
            transform.Find("PickText").GetComponent<Text>().text = "Pick an Upgrade:";
        }
        

        for (int t = 0; t < 3; t++)
        {



            int randomNumber = Random.Range(0, upgrades.Count);
            Upgrades randomUpgrade = upgrades[randomNumber];
            randomUpgrade.player = mastermind.player;
            upgrades.RemoveAt(randomNumber);
            GameObject buttonObject = Instantiate(buttonPrefab, transform.Find("ButtonLayout"));
            //buttonObject.transform.Translate(Vector3.right * 1 * t);
            Button button = buttonObject.GetComponent<Button>();

            buttons.Add(button);
            availableUpgrades.Add(randomUpgrade);
            button.onClick.AddListener(delegate { BuyButton(button, randomUpgrade); });

            button.transform.Find("NameText").GetComponent<Text>().text = randomUpgrade.upgradeName;
            button.transform.Find("PriceText").GetComponent<Text>().text = randomUpgrade.price.ToString() + ":-";
            //button.transform.Find("Tooltip/Description").GetComponent<Text>().text = randomUpgrade.GetDescription();
            button.GetComponent<ShopButton>().description = randomUpgrade.GetDescription();
            button.GetComponent<ShopButton>().tooltipWindow = tooltip;


        }
        transform.Find("PickText").gameObject.SetActive(true);
    }

    public void Exit()
    {
        ClearUpgrades();
        StartCoroutine(SlideOut());
        //gameObject.SetActive(false);

        if (upgrades==secondaryWeapons) StartCoroutine(RMBInstructions.FadeOut());
    }

    public void ClearUpgrades()
    {
        foreach (Button button in buttons)
        {
            Destroy(button.gameObject);
        }
        buttons.Clear();
        upgrades.AddRange(availableUpgrades);
        availableUpgrades.Clear();

        transform.Find("PickText").gameObject.SetActive(false);
    }

    public void BuyButton(Button button,Upgrades upgrade)
    {
        //upgrade.player = mastermind.player;
        //mastermind.UpdateMoney(-upgrade.price);
        //CheckAfford();
        upgrade.Buy();

        if (!upgrade.unique) upgrades.Add(upgrade);
        if (upgrades == secondaryWeapons) secWeaponPicked = true;               
        availableUpgrades.RemoveAt(buttons.IndexOf(button));
        buttons.Remove(button);
        Destroy(button.gameObject);
        tooltip.enabled = false;

        ClearUpgrades();
    }

    public IEnumerator SlideIn()
    {
        float slideInTime = 1.5f;

        foreach (UIelement element in UIelements)
        {
            element.transform.Translate(element.offset);
        }

        float lastTime = 0;
        for (float time = 0; time <= 1; time += Time.deltaTime / slideInTime)
        {
            if (time > 1) time = 1;
            float deltaTime = time - lastTime;
            foreach (UIelement element in UIelements)
            {
                element.transform.Translate(-element.offset * deltaTime);
            }
            lastTime = time;
            yield return null;
        }
    }

    public IEnumerator SlideOut()
    {
        float slideOutTime = 1.5f;

        float lastTime = 0;
        for (float time = 0; time <= 1; time += Time.deltaTime / slideOutTime)
        {
            if (time > 1) time = 1;
            float deltaTime = time - lastTime;
            foreach (UIelement element in UIelements)
            {
                element.transform.Translate(element.offset * deltaTime);
            }
            lastTime = time;
            yield return null;
        }
        foreach (UIelement element in UIelements)
        {
            element.transform.Translate(-element.offset);
        }
        gameObject.SetActive(false);
    }
}
