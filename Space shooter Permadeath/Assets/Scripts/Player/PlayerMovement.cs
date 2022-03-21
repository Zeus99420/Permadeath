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

    public bool startOfGame = true;
    public override void Start()
    {
        //Characters start-metod, som gör att health = maxHealth, kallas när spelet börjar.
        //När kopior av spelaren skapas för checkpoints så kallas den inte eftersom spelaren ska behålla sin dåvarande health
        if (startOfGame)
        {
            base.Start();
            startOfGame = false;
        }

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

    public override void Die()
    {
        mastermind.SetGameMastermindState(global::Mastermind.GameMastermindState.GameOver);
        PlayExplosion();
        Destroy(healthBar.gameObject);
        Destroy(gameObject);
    }

}
