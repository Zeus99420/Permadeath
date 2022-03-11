using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    public Mastermind mastermind;
    public GameObject player;
    public List<Upgrades> upgrades;
    public GameObject buttonPrefab;
    public List<Button> buttons;
    public List<Upgrades> availableUpgrades;

    public void Initialize()
    {
        //upgrades = GetComponentsInChildren<Upgrades>();
        upgrades.AddRange(GetComponentsInChildren<Upgrades>());
        //foreach (Upgrades upgrade in GetComponentsInChildren<Upgrades>())
        //{

        //}
    }

    public void EnterShop()
    {
        for (int t=0; t<5; t++)
        {
            Upgrades randomUpgrade = upgrades[Random.Range(0, upgrades.Count)];
            GameObject buttonObject = Instantiate(buttonPrefab,transform);
            buttonObject.transform.Translate(Vector3.right * 2 * t);
            Button button = buttonObject.GetComponent<Button>();

            buttons.Add(button);
            availableUpgrades.Add(randomUpgrade);
            button.onClick.AddListener(delegate { BuyButton(button,randomUpgrade); });

            button.transform.Find("NameText").GetComponent<Text>().text = randomUpgrade.upgradeName;
            button.transform.Find("PriceText").GetComponent<Text>().text = randomUpgrade.price.ToString() + ":-";




        }

        CheckAfford();
    }

    public void Exit()
    {
        foreach (Button button in buttons)
        {
            Destroy(button.gameObject);
        }
        buttons.Clear();
        availableUpgrades.Clear();

        gameObject.SetActive(false);
    }

    public void CheckAfford ()
    {
        for (int t=0; t<buttons.Count; t++)
        {
            if (mastermind.money >= availableUpgrades[t].price) buttons[t].interactable = true;

            else buttons[t].interactable = false;

        }
    }

    public void BuyButton(Button button,Upgrades upgrade)
    {
        upgrade.player = player;
        mastermind.UpdateMoney(-upgrade.price);
        CheckAfford();
        upgrade.Buy();

        availableUpgrades.RemoveAt(buttons.IndexOf(button));
        buttons.Remove(button);
        Destroy(button.gameObject);
    }
}
