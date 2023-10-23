using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    public override void Start()
    {
        base.Start();
        healthBar.MaxHealthPoints = maxHealth;
        healthBar.gameObject.SetActive(true);

    }

    private void OnDestroy()
    {
        healthBar.gameObject.SetActive(false);
    }
}
