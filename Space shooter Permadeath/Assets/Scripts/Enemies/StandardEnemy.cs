using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardEnemy : Enemy
{
    public override void Start()
    {
        maxHealth = (int)(maxHealth * Random.Range(0.7f, 1.3f));
        healthBar = CreateHealthBar(healthBarPrefab);
        base.Start();

        healthBar.gameObject.SetActive(false);

    }

}
