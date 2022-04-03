using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrades : MonoBehaviour
{
    [HideInInspector] public GameObject player;
    public string upgradeName;
    public int price;
    [TextArea] public string description;
    public bool unique;

    bool firstTimeBuying = true;
    public virtual void Buy()
    {
        if (firstTimeBuying)
        {
            BuyFirst();
            firstTimeBuying = false;
        }
        else BuyAnother();
    }
    public virtual void BuyFirst() {}
    public virtual void BuyAnother() {}

}
