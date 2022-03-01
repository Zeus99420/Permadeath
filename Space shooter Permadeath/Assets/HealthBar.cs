using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Health health;
    public void Setup(Health health)
    {
        this.health = health;
    }
    private void Update()
    {
        transform.Find("Bar").localScale = new Vector3(health.GetHealthPercent(), 1);
    }
}
