using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Character
{
    Rigidbody2D m_rigidbody;
    public Mastermind mastermind;
    public GameObject healthBarPrefab;

    public float acceleration;
    [HideInInspector] public bool usingEngines;
    public bool shieldActive;

    public override void Start()
    {

            base.Start();
            //Character start-metod gör att spelarens health = maxHealth. 
            healthAlreadySet = true;
            //healthAlreadySet=true gör att kopior av spelaren som skapas för checkpoints behåller sin dåvarande health.


        SetupHealthbar(healthBarPrefab);
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame

    private void Update()
    {
        // convert mouse position into world coordinates
        Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // get direction you want to point at
        Vector2 direction = (mouseScreenPosition - (Vector2)transform.position).normalized;

        // set vector of transform directly
        transform.up = direction;

        Vector3 pos = Camera.main.WorldToViewportPoint(gameObject.transform.position);
        if (pos.x < 0 || pos.x > 1) m_rigidbody.velocity = m_rigidbody.velocity*new Vector2(0,1);
        if(pos.y < 0 || pos.y > 1) m_rigidbody.velocity = m_rigidbody.velocity * new Vector2(1, 0);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        gameObject.transform.position = Camera.main.ViewportToWorldPoint(pos);
    }
    void FixedUpdate()
    {
        usingEngines = false;
        if (Input.GetKey("w")) { m_rigidbody.AddForce(Vector2.up * acceleration); usingEngines = true; }
        if (Input.GetKey("s")) { m_rigidbody.AddForce(Vector2.down * acceleration); usingEngines = true; }        
        if (Input.GetKey("a")) { m_rigidbody.AddForce(Vector2.left * acceleration); usingEngines = true; }
        if (Input.GetKey("d")) { m_rigidbody.AddForce(Vector2.right * acceleration); usingEngines = true; }
    }

    public override void Damage(int damageAmount)
    {
        if (shieldActive)
        {
            shieldActive = false;
            transform.Find("Shield").gameObject.SetActive(false);
        }
            else
                {
                    base.Damage(damageAmount);
                    StartCoroutine(Flicker(Color.red));
                }

    }

    public override void Die()
    {
        mastermind.SetGameMastermindState(global::Mastermind.GameMastermindState.GameOver);
        PlayExplosion();
        Destroy(healthBar.gameObject);
        Destroy(gameObject);
    }



}
