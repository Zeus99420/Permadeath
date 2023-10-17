using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    Vector2 moveDirection;

    [Header("Shield")]
    public PolygonShield shield;
    bool outside;

    [Header("Railgun")]
    public RailGunStats railgunStats;
    public float rgFreeFireRate;
    public float rgVolleyRate;
    public float rgVolleyWait;
    bool rgVolleyReady = true;
    bool rgFreeFire = true;
    List<RailgunTurret> railguns = new List<RailgunTurret>();

    [Header("PDC")]
    public PdcStats pdcStats;
    List<PDC> pdcs = new List<PDC>();


    public override void Start()
    {
        base.Start();

        SetDirection();

        InitializeWeapons();
    }

    private void Update()
    {
        m_rigidbody.AddForce(moveDirection * acceleration);

        if (!outside && !IsInScreen(-0.25f)) Outside();
        else if (outside && IsInScreen(0)) outside = false;

        if (rgVolleyReady && Random.value < rgVolleyRate*Time.deltaTime) StartCoroutine(RailgunVolley());
        if (rgFreeFire && Random.value < rgFreeFireRate*Time.deltaTime) RailGunFire();
    }

    void Outside()
    {
        outside = true;
        SetDirection();
        shield.Restore();
    }

    void SetDirection()
    {
        Vector2 targetPosition;
        targetPosition.x = Random.Range(0.2f, 0.8f);
        targetPosition.y = Random.Range(0.2f, 0.8f);
        targetPosition = Camera.main.ViewportToWorldPoint(targetPosition);
        moveDirection = (targetPosition - (Vector2)transform.position).normalized;
    }



    IEnumerator RailgunVolley()
    {
        rgVolleyReady = false;
        rgFreeFire = false;

        foreach (RailgunTurret railgun in railguns)
        {
            if (railgun.TryFire())
            {
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

        if (railgun.TryFire())
        {
            railguns.RemoveAt(r);
            railguns.Add(railgun);

            //railgun.StartCoroutine(railgun.Fire());
        }
    }

    void InitializeWeapons()
    {
        railguns.AddRange(GetComponentsInChildren<RailgunTurret>());
        foreach (RailgunTurret railgun in railguns)
        {
            railgun.player = player;
            railgun.mastermind = mastermind;
            railgun.stats = railgunStats;
            railgun.Initialize();
        }

        pdcs.AddRange(GetComponentsInChildren<PDC>());
        foreach (PDC pdc in pdcs)
        {
            pdc.player = player;
            pdc.mastermind = mastermind;
            pdc.s = pdcStats;
            pdc.Initialize();
        }
    }
}
