using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public AsteroidSpawner asteroid_spawner;
    public GameObject Background;
    public float speed;
    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.position += transform.up * (Time.deltaTime * speed);
        float distance = Vector3.Distance(transform.position, Background.transform.position);
        
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
        asteroid_spawner.asteroid_count -= 1;
    }
    /*
    public float speed;
    bool directionRight;
   // public float acceleration;
   // protected Rigidbody2D m_rigidbody;
    // Start is called before the first frame update

    void Start()
    {
        var randomNumber = Random.Range(2, 5); //Generates number between 1 & 2
        if (randomNumber > 1)
            directionRight = true;
        else
            directionRight = false;
    
    Vector2 targetPosition;
        targetPosition.x = Random.Range(0.2f, 0.8f);
        targetPosition.y = Random.Range(0.2f, 0.8f);
        targetPosition = Camera.main.ViewportToWorldPoint(targetPosition);
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        transform.up = direction;
    }

    // Update is called once per frame
    void Update()
        
    {
        if (directionRight)
            transform.TransformDirection(Vector3.right * Time.deltaTime);
        else
            transform.TransformDirection(Vector3.left * Time.deltaTime);

        
       // transform.Translate((Vector2.left + Vector2.down)  * Time.deltaTime/2);
        Vector2 position = transform.position;
        position = new Vector2(position.x, position.y + speed * Time.deltaTime);
        transform.position = position;
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
    }


    public void FixedUpdate()
    {
        
        //m_rigidbody.AddForce(transform.up * acceleration);
        
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    */
}
