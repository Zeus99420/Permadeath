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
    public GameObject button;
    public List<Button> buttons;
    public List<Upgrades> availableUpgrades;

    public void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        for (int t=0; t<3; t++)
        {
            Upgrades randomUpgrade = upgrades[Random.Range(0, upgrades.Count)];
            GameObject newButton = Instantiate(button,transform);
            newButton.transform.Translate(Vector3.right * 2 * t);

            buttons.Add(newButton.GetComponent<Button>());
            availableUpgrades.Add(randomUpgrade);
            newButton.GetComponent<Button>().onClick.AddListener(delegate { BuyButton(randomUpgrade); });

            newButton.transform.Find("NameText").GetComponent<Text>().text = randomUpgrade.upgradeName;
            newButton.transform.Find("PriceText").GetComponent<Text>().text = randomUpgrade.price.ToString() + ":-";




        }
    }

    public void Update ()
    {
        for (int t=0; t<buttons.Count; t++)
        {
            if (mastermind.money >= availableUpgrades[t].price) buttons[t].interactable = true;

            else buttons[t].interactable = false;

        }
    }

    public void BuyButton(Upgrades upgrade)
    {
        upgrade.player = player;
        mastermind.money -= upgrade.price;
        upgrade.Buy();
    }
}
