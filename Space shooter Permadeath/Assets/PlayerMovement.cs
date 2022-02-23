using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D m_rigidbody;
    public float acceleration;
    // Start is called before the first frame update
    void Start()
    {
        
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
        if (Input.GetKey("w")) m_rigidbody.AddForce(Vector2.up * acceleration);
        if (Input.GetKey("s")) m_rigidbody.AddForce(Vector2.down * acceleration);
        if (Input.GetKey("a")) m_rigidbody.AddForce(Vector2.left * acceleration);
        if (Input.GetKey("d")) m_rigidbody.AddForce(Vector2.right * acceleration);
    }

   
}
