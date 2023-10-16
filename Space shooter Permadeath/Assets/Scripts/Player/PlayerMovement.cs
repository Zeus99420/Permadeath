using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Character
{
    Rigidbody2D m_rigidbody;
    //public Mastermind mastermind;
    //public GameObject healthBarPrefab;

    public float baseAcceleration;
    public float acceleration;
    [HideInInspector] public bool usingEngines;
    public float accelerationMultiplier;

    public List<Vector2> positionRecord;

    //BARRIER
    [HideInInspector] public int maxBarrierHealth;
    [HideInInspector] public float barrierDuration;
    [HideInInspector] public float barrierCooldown;
    [HideInInspector] public Color barrierColorReady;
    [HideInInspector] public Color barrierColorActive;
    [HideInInspector] public Color barrierColorDamaged;
    [HideInInspector] public bool barrierBought;
    bool barrierReady;
    bool barrierActive;
    int barrierHealth;
    Coroutine barrierCorutine;
    [HideInInspector] public Vector2 barrierActiveSize;
    [HideInInspector] public Vector2 barrierReadySize;
    public SpriteRenderer barrierRenderer;

    [Header("Shield")]
    public int maxShieldHealth;
    public float shieldRecharge;
    public SpriteRenderer shieldRenderer;
    public Color shieldColor;
    public GameObject shieldBarPrefab;
    [HideInInspector] public float shieldHealth;
    float shieldDamagedTime;
    [HideInInspector] public CoolHealthBar shieldBar;




    public bool trailEnabled = false;



    public override void Start()
    {
        base.Start();
        //Character start-metod g�r att spelarens health = maxHealth. 
        //healthAlreadySet = true;
        //healthAlreadySet=true g�r att kopior av spelaren som skapas f�r checkpoints beh�ller sin d�varande health.



        if (barrierBought) ReadyBarrier();
        shieldHealth = maxShieldHealth;
        shieldColor = shieldRenderer.color;
        shieldBar = CreateHealthBar(shieldBarPrefab);
        shieldBar.MaxHealthPoints = maxShieldHealth;
        shieldBar.SetSize();

        m_rigidbody = GetComponent<Rigidbody2D>();

        if (sharperMovement) baseDrag = m_rigidbody.drag;
    }

    // Update is called once per frame

    private void Update()
    {
        if (!mastermind.gamePaused)
        {
            // convert mouse position into world coordinates
            Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // get direction you want to point at
            Vector2 direction = (mouseScreenPosition - (Vector2)transform.position).normalized;

            if (focusFireEnabled) FocusFire(direction);
            else transform.up = direction;



            Vector3 pos = Camera.main.WorldToViewportPoint(gameObject.transform.position);
            if (pos.x < 0 || pos.x > 1) m_rigidbody.velocity = m_rigidbody.velocity * new Vector2(0, 1);
            if (pos.y < 0 || pos.y > 1) m_rigidbody.velocity = m_rigidbody.velocity * new Vector2(1, 0);
            pos.x = Mathf.Clamp01(pos.x);
            pos.y = Mathf.Clamp01(pos.y);
            gameObject.transform.position = Camera.main.ViewportToWorldPoint(pos);

            ShieldUpdate();
        }


    }

    [Header("Sharper Movement")]
    public bool sharperMovement;
    public float sharpMoveSpeed;
    float baseDrag;
    public float sharpMoveDrag;
    public float glideDrag;
    void FixedUpdate()
    {
        if (!mastermind.gamePaused)
        {
            acceleration = baseAcceleration * accelerationMultiplier;
            usingEngines = false;
            Vector2 direction = Vector2.zero;
            if (Input.GetKey("w")) { direction += Vector2.up; usingEngines = true; }
            if (Input.GetKey("s")) { direction += Vector2.down; ; usingEngines = true; }
            if (Input.GetKey("a")) { direction += Vector2.left; ; usingEngines = true; }
            if (Input.GetKey("d")) { direction += Vector2.right; ; usingEngines = true; }
            direction.Normalize();

            if (sharperMovement)
            {
                if (usingEngines)
                {
                    acceleration *= sharpMoveSpeed;
                    m_rigidbody.drag = baseDrag * sharpMoveDrag;
                }
                else
                {
                    m_rigidbody.drag = baseDrag * glideDrag;
                }
            }

            m_rigidbody.AddForce(direction * acceleration);

            positionRecord.Add(transform.position);
            if (positionRecord.Count > 60) positionRecord.RemoveAt(0);
        }
    }

    public override void Damage(int damageAmount)
    {
        if (barrierActive) BarrierDamage(damageAmount);
        else if (shieldHealth > 0) ShieldDamage(damageAmount);
        else if (barrierReady) ActivateBarrier(damageAmount);
        else
        {
            base.Damage(damageAmount);
            StartCoroutine(Flicker(Color.red));
        }


        //transform.Find("Shield").gameObject.SetActive(false);

    }

    public override void Die()
    {
        mastermind.SetGameMastermindState(global::Mastermind.GameMastermindState.GameOver);
        PlayExplosion();
        Destroy(healthBar.gameObject);
        Destroy(shieldBar.gameObject);
        Destroy(gameObject);
    }


    //UPPGRADERINGAR
    //Dessa funktioner anv�nds ifall man k�pt vissa uppgraderingar.
    public void LevelComplete()
    {
        Debug.Log("Level Complete");
        if (barrierBought) Invoke("ReadyBarrier",1.5f);
        StartCoroutine(QuickRecharge());
    }

    public IEnumerator QuickRecharge()
    {
        yield return new WaitForSeconds(0.5f);
        for (float t=0;t<1.5; t += Time.deltaTime)
        {
            if (shieldHealth < 0) shieldHealth = 0;
            shieldHealth += maxShieldHealth * Time.deltaTime * 0.7f;
            yield return null;
        }
    }
    public void ReadyBarrier()
    {
        if (barrierCorutine != null) StopCoroutine(barrierCorutine);
        barrierReady = true;
        barrierActive = false;
        barrierRenderer.enabled = true;
        barrierRenderer.color = barrierColorReady;
        barrierRenderer.transform.localScale = barrierReadySize;
    }

    public void ActivateBarrier(int damageAmount)
    {
        barrierActive = true;
        barrierReady = false;
        barrierHealth = maxBarrierHealth;
        barrierHealth -= damageAmount;
        StartCoroutine(BarrierFlicker(Color.white));
        barrierCorutine = StartCoroutine(BarrierDuration());
        StartCoroutine(Flicker(Color.yellow));

        barrierRenderer.transform.localScale = barrierActiveSize;
    }


    public void BarrierDamage(int damageAmount)
    {
        //Ifall skadan �verstiger Barriers health s� inaktiveras barrier och Damage kallas igen med �verskottet av skada.
        if (damageAmount > barrierHealth)
        {
            StopCoroutine(barrierCorutine);
            EndBarrier();
            Damage(damageAmount - barrierHealth);
        }
        else
        {
            barrierHealth -= damageAmount;
            StartCoroutine(BarrierFlicker(Color.red));
        }
    }

    public IEnumerator BarrierDuration()
    {
        yield return new WaitForSeconds(barrierDuration - 1f);

        //Barri�ren blinkar n�r den �r p� v�g att ta slut
        for (int t = 0; t < 5; t++)
        {
            barrierRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            barrierRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        EndBarrier();
    }

    public IEnumerator BarrierCooldown()
    {
        yield return new WaitForSeconds(barrierCooldown);
        ReadyBarrier();
    }

    public void EndBarrier()
    {
        barrierActive = false;
        barrierRenderer.enabled = false;
        barrierCorutine = StartCoroutine(BarrierCooldown());
        //Invoke("ReadyBarrier", barrierCooldown);
    }

    public IEnumerator BarrierFlicker(Color color)
    //Barri�ren blinkar n�r den tar skada
    {
        barrierRenderer.color = color;
        yield return new WaitForSeconds(0.05f);
        barrierRenderer.color = Color.Lerp(barrierColorDamaged, barrierColorActive, barrierHealth / (float)maxBarrierHealth);

    }

    public void ShieldDamage(int damageAmount)
    {
        if (damageAmount > shieldHealth)
        {
            
            //damageAmount -= (int)shieldHealth;
            shieldBar.Damages += shieldHealth;
            shieldBar.Health = 0;
            shieldHealth -= damageAmount;
            StartCoroutine(Flicker(Color.blue));
            //shieldHealth = 0;

            //Damage(damageAmount);
        }

        else
        {
            shieldHealth -= damageAmount;
            shieldBar.Damages += damageAmount;
            shieldBar.Health = shieldHealth;
        }
        
        
    }

    public void ShieldUpdate()
    {
        shieldHealth += Time.deltaTime * shieldRecharge;
        if (shieldHealth > maxShieldHealth) shieldHealth = maxShieldHealth;
        shieldBar.Health = shieldHealth;

        //Sk�ldens blir mindre genomskinlig n�r den f�r mer liv. N�r den absorberar skada blir den tillf�lligt r�d
        //if (Time.time > shieldDamagedTime + 0.08f)
        //{
            Color color = shieldColor;
            color.a = shieldHealth / maxShieldHealth;
            shieldRenderer.color = color;
        //}
        //else shieldRenderer.color = Color.red;

        shieldRenderer.transform.Rotate(0f, 0f, Time.deltaTime * 40f);
    }

    [Header("Wind Shield (inte klar)")]
    public float windShieldChargeRate;
    public float windShieldHealth;
    public void WindShieldUpdate()
    {
        windShieldHealth += Time.deltaTime * m_rigidbody.velocity.sqrMagnitude * windShieldChargeRate;
    }


    //FOCUS FIRE
    [HideInInspector] public bool focusFireEnabled;
    [HideInInspector] public float rotationRate;
    [HideInInspector] public float freeRotationRate;
    [HideInInspector] public float lockedRotationRate;
    [HideInInspector] public float focusFireRecoveryTime;
    public void FocusFire(Vector2 direction)
    {
        if (!GetComponent<Weapons>().readyToFire) rotationRate = lockedRotationRate;
        else
        {
            rotationRate += (freeRotationRate-lockedRotationRate)*Time.deltaTime/focusFireRecoveryTime;
            if (rotationRate > freeRotationRate) rotationRate = freeRotationRate;
        }
        transform.up = Vector3.RotateTowards(transform.up, direction,rotationRate*Time.deltaTime,1f);
    }

}
