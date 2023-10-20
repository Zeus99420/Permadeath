using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable] public class ScalingValue
{
    public string name;
    public float minValue;
    public float maxValue;
    public float startsAt;
    public float maxedAt;
}
public class Boss : Enemy
{
    Vector2 moveDirection;
    float damageTaken;
    public float tempAcceleration;
    float shieldRotation;
    float shieldDirection;
    public Transform body;
    float bodyRotation;
    float bodyDirection;

    public List<ScalingValue> scalingValues;
    public Dictionary<string, ScalingValue> valuesDictionary = new Dictionary<string, ScalingValue>();

    [Header("Shield")]
    public PolygonShield shield;
    bool outside;

    [Header("Railgun")]
    public RailGunStats railgunStats;
    float rgFreeFireRate;
    float rgVolleyRate;
    public float rgVolleyWait;
    bool rgVolleyReady = true;
    bool rgFreeFire = true;
    List<RailgunTurret> railguns = new List<RailgunTurret>();

    [Header("PDC")]
    public float pdcAutoFireThreshold;
    public PdcStats pdcStats;
    List<PDC> pdcs = new List<PDC>();

    [Header("Bombs")]
    float bombRate;
    public GameObject bomb;
    public float bombVelocityMin;
    public float bombVelocityMax;
    public int bombDamage;


    public override void Start()
    {
        randomHealth = false;
        base.Start();
        SetDirection();
        InitializeWeapons();
        SetupScaling();
    }

    private void FixedUpdate()
    {
        m_rigidbody.AddForce(moveDirection * acceleration);

        if (!outside && !IsInScreen(-0.21f)) Outside();
        else if (outside && IsInScreen(-0.09f)) Inside();

        if (rgVolleyReady && Random.value < rgVolleyRate * Time.fixedDeltaTime) StartCoroutine(RailgunVolley());
        if (rgFreeFire && Random.value < rgFreeFireRate * Time.fixedDeltaTime) RailGunFire();
        if (Random.value < bombRate * Time.fixedDeltaTime) ReleaseBomb();

        shield.Rotate(shieldRotation * shieldDirection);
        body.Rotate(0f, 0f, bodyDirection * bodyRotation * Time.fixedDeltaTime);
    }

    void Outside()
    {
        outside = true;
        SetDirection();
        shield.Restore();
    }
    void Inside()
    {
        outside = false;
        damageTaken = 0;
        Scaling();
    }

    void SetDirection()
    {
        Vector2 targetPosition;
        targetPosition.x = Random.Range(0.3f, 0.7f);
        targetPosition.y = Random.Range(0.3f, 0.7f);
        targetPosition = Camera.main.ViewportToWorldPoint(targetPosition);
        moveDirection = (targetPosition - (Vector2)transform.position).normalized;
        shieldDirection = Random.Range(0.7f, 1f);
        bodyDirection = Random.Range(0.7f, 1f);
        if (Random.value < 0.5f) shieldDirection *= -1;
        if (Random.value < 0.5f) bodyDirection *= -1;
    }

    IEnumerator RailgunVolley()
    {
        Debug.Log("Railgun Volley!");
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
        int r = Random.Range(0, railguns.Count);
        RailgunTurret railgun = railguns[r];

        if (railgun.TryFire())
        {
            railguns.RemoveAt(r);
            railguns.Add(railgun);
        }
    }

    void ReleaseBomb()
    {
        Vector2 bombDirection = Random.insideUnitCircle.normalized;
        Vector2 bombPosition = (Vector2)transform.position + bombDirection * 1.5f;
        if (IsInScreen(0f,bombPosition))
        {
            GameObject newBomb = Instantiate(bomb, bombPosition, transform.rotation, mastermind.stuffContainer);
            Rigidbody2D bombRigidbody = newBomb.GetComponent<Rigidbody2D>();
            bombRigidbody.velocity = m_rigidbody.velocity + Random.Range(bombVelocityMin,bombVelocityMax) * bombDirection;
            newBomb.GetComponent<EnemyBomb>().damage = bombDamage;
            newBomb.GetComponent<EnemyBomb>().mastermind = mastermind;
        }

    }

    public override void Damage(int damageAmount)
    {
        base.Damage(damageAmount);
        damageTaken += damageAmount;
        Scaling();
    }

    public override void Die()
    {
        base.Die();
        mastermind.BossKilled();
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




    public void SetupScaling()
    {
        foreach (ScalingValue value in scalingValues)
        {
            valuesDictionary.Add(value.name, value);
        }
        Scaling();
    }

    public void Scaling()
    {
        float healthFraction = (float)health / maxHealth;

        acceleration = Scale("acceleration", healthFraction);
        acceleration += damageTaken / maxHealth * tempAcceleration;

        rgVolleyRate = Scale("rgVolleyRate", healthFraction);
        rgFreeFireRate = Scale("rgFreeFireRate", healthFraction);
        bombRate = Scale("bombRate", healthFraction);
        shieldRotation = Scale("shieldRotation", healthFraction);
        bodyRotation = Scale("bodyRotation", healthFraction);

        if (!PdcAutoFire && healthFraction < pdcAutoFireThreshold) PdcAutoFire = true;
    }

    public float Scale(string name,float healthFraction)
    {
        ScalingValue scaler = valuesDictionary[name];

        if (healthFraction > scaler.startsAt) return 0;
        else
        {
            float t = (scaler.startsAt - healthFraction) / (scaler.startsAt - scaler.maxedAt);
            return Mathf.Lerp(scaler.minValue, scaler.maxValue, t);
        }
    }

    bool _pdcAutoFire = false;
    public bool PdcAutoFire
    {
        get { return _pdcAutoFire; }
        set
        {
            _pdcAutoFire = value;
            foreach (PDC pdc in pdcs)
            {
                pdc.autoFire = value;
            }

        }
    }
}


