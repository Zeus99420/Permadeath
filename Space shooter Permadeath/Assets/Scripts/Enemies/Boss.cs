using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [Header("Railgun")]
    public RailGunStats railgunStats;
    public float rgFreeFireRate;
    public float rgVolleyRate;
    public float rgVolleyWait;
    bool rgVolleyReady = true;
    bool rgFreeFire = true;
    public List<RailgunTurret> railguns;
    public override void Start()
    {
        base.Start();

        transform.position = new Vector2(6f, 0);

        RailgunsInitialize();
    }

    private void Update()
    {
        if (rgVolleyReady && Random.value < rgVolleyRate*Time.deltaTime) StartCoroutine(RailgunVolley());

        if (rgFreeFire && Random.value < rgFreeFireRate*Time.deltaTime) RailGunFire();
    }



    IEnumerator RailgunVolley()
    {
        rgVolleyReady = false;
        rgFreeFire = false;

        foreach (RailgunTurret railgun in railguns)
        {
            if (railgun.ready)
            {
                railgun.StartCoroutine(railgun.Fire());
                yield return new WaitForSeconds(rgVolleyWait);
            }
        }

        rgFreeFire = true;
        yield return new WaitForSeconds(railgunStats.cooldown);
        rgVolleyReady = true;
    }

    void RailGunFire()
    {
        //Randomly picks one of the least recently fired weapons to fire from.
        int r = Random.Range(0, railguns.Count / 2);
        RailgunTurret railgun = railguns[r];

        if (railgun.ready)
        {
            railguns.RemoveAt(r);
            railguns.Add(railgun);

            railgun.StartCoroutine(railgun.Fire());

        }
    }

    void RailgunsInitialize()
    {
        railguns.AddRange(GetComponentsInChildren<RailgunTurret>());

        foreach (RailgunTurret railgun in railguns)
        {
            railgun.player = player;
            railgun.mastermind = mastermind;

            //railgun.projectileSpeed = rgVelocity;
            //railgun.projectileSize = rgProjectileSize;
            //railgun.damage = rgDamage;
            //railgun.rotationSpeed = rgRotation;
            //railgun.chargeTime = rgChargeTime;
            //railgun.randomLead = rgLead;
            //railgun.randomOffset = rgOffset;

            railgun.stats = railgunStats;
            railgun.Initialize();
        }
    }
}
