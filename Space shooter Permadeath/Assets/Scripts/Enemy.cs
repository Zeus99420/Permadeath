using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject Mastermind;
    public Transform player;
    Rigidbody2D m_rigidbody;
    public float acceleration;
    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
        m_rigidbody.AddForce(direction * acceleration);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            Mastermind.GetComponent<Mastermind>().SetGameMastermindState(global::Mastermind.GameMastermindState.GameOver);
        }
    }
}
