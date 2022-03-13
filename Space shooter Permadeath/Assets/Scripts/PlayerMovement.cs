using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D m_rigidbody;
    public GameObject permadeathscreen;
    public Transform HealthBar;
    Transform healthBarTransform;

    public Health health;

    public float acceleration;
    public int maxHealth;
    [HideInInspector] public bool usingEngines;

    // Start is called before the first frame update
    void Start()
    {
        healthBarTransform = Instantiate(HealthBar, new Vector3(0, 0), Quaternion.identity);
        HealthBar healthbar = healthBarTransform.GetComponent<HealthBar>();
        healthbar.player = transform;
        health = new Health(maxHealth, gameObject);
        healthbar.Setup(health);

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

    private void OnDestroy()
    {
        permadeathscreen.GetComponent<Mastermind>().SetGameMastermindState(global::Mastermind.GameMastermindState.GameOver);
        Destroy(healthBarTransform.gameObject);
    }

}
