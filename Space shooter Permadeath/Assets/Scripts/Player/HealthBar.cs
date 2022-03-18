using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Character character;

    //public Transform characterTransform;
    public Vector3 offset;

    private void Update()
    {

        transform.position = character.transform.position + offset; ;
        transform.Find("Bar").localScale = new Vector3(character.GetHealthPercent(),1);
    }
}
