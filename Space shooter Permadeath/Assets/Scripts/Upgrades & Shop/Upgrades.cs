using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrades : MonoBehaviour
{
    [HideInInspector] public GameObject player;
    public string upgradeName;
    public int price;
    public virtual void Buy() { }
   
}
