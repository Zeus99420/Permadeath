using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Character character;
    public Image bar;

    //public Transform characterTransform;
    public Vector3 offset;

    private void Update()
    {

        transform.position = character.transform.position + offset; ;
        //transform.Find("Bar").localScale = new Vector3(character.GetHealthPercent(),1);

        bar.fillAmount = character.GetHealthPercent();
    }
}
