    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    public Mastermind mastermind;
    public List<Upgrades> upgrades;
    public GameObject buttonPrefab;
    public List<Button> buttons;
    public List<Upgrades> availableUpgrades;

    public RectTransform[] children;
    public void Initialize()
    {
        upgrades.AddRange(GetComponentsInChildren<Upgrades>());
    }

    public void EnterShop()
    {
        StartCoroutine(SlideIn());

        for (int t = 0; t < 3; t++)
        {
            int randomNumber = Random.Range(0, upgrades.Count);
            Upgrades randomUpgrade = upgrades[randomNumber];
            upgrades.RemoveAt(randomNumber);
            GameObject buttonObject = Instantiate(buttonPrefab, transform.Find("ButtonLayout"));
            buttonObject.transform.Translate(Vector3.right * 3 * t);
            Button button = buttonObject.GetComponent<Button>();

            buttons.Add(button);
            availableUpgrades.Add(randomUpgrade);
            button.onClick.AddListener(delegate { BuyButton(button, randomUpgrade); });

            button.transform.Find("NameText").GetComponent<Text>().text = randomUpgrade.upgradeName;
            button.transform.Find("PriceText").GetComponent<Text>().text = randomUpgrade.price.ToString() + ":-";
            button.transform.Find("Tooltip/Description").GetComponent<Text>().text = randomUpgrade.description;

            transform.Find("PickAnUpgradeText").gameObject.SetActive(true);
        }



        //CheckAfford();
    }

    public void Exit()
    {
        ClearUpgrades();
        StartCoroutine(SlideOut());
        //gameObject.SetActive(false);
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

        transform.Find("PickAnUpgradeText").gameObject.SetActive(false);
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
        upgrade.player = mastermind.player;
        //mastermind.UpdateMoney(-upgrade.price);
        //CheckAfford();
        upgrade.Buy();

        if (!upgrade.unique) upgrades.Add(upgrade);
        availableUpgrades.RemoveAt(buttons.IndexOf(button));
        buttons.Remove(button);
        Destroy(button.gameObject);

        ClearUpgrades();
    }

    public IEnumerator SlideIn()
    {
        float slideInTime = 1.5f;
        float offset = 4;
        //RectTransform[] childrensTransforms = GetComponentsInChildren<RectTransform>();

        foreach (RectTransform childTransform in children)
        {
            childTransform.Translate(Vector2.down * offset);

        }

        float lastTime = 0;
        for (float time = 0; time <= 1; time += Time.deltaTime / slideInTime)
        {
            if (time > 1) time = 1;
            float deltaTime = time - lastTime;
            foreach (RectTransform childTransform in children)
            {
                childTransform.Translate(Vector2.up * deltaTime*offset);
            }
            lastTime = time;
            yield return null;
        }
    }

    public IEnumerator SlideOut()
    {
        float slideOutTime = 1.5f;
        float offset = 4;

        float lastTime = 0;
        for (float time = 0; time <= 1; time += Time.deltaTime / slideOutTime)
        {
            if (time > 1) time = 1;
            float deltaTime = time - lastTime;
            foreach (RectTransform childTransform in children)
            {
                childTransform.Translate(Vector2.down * deltaTime * offset);
            }
            lastTime = time;
            yield return null;
        }
        foreach (RectTransform childTransform in children)
        {
            childTransform.Translate(Vector2.up * offset);

        }
        gameObject.SetActive(false);
    }
}
