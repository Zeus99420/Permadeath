using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    /*
    public float speed;
    public bool isMoving;
    Vector2 min;
    Vector2 max;

    void Awake()
    {
        isMoving = false;
        min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        max.y = max.y + GetComponent<SpriteRenderer>().sprite.bounds.extents.y;
        min.y = min.y - GetComponent<SpriteRenderer>().sprite.bounds.extents.y;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
            return;
        Vector2 position = transform.position;
        //position = new Vector2(position.x, position.y + speed * Time.deltaTime);
        transform.Translate(Vector2.right * Time.deltaTime);
        transform.position = position;
        if (transform.position.y < min.y)
        {
            isMoving = false;
        }
    }

    public void ResetPos()
    {
        transform.position = new Vector2(Random.Range(min.x, max.x), max.y);
    }
    */
    
    public float speed;
    void Start()
    {

    }


    void Update()
    {
        transform.Translate(Vector2.right * Time.deltaTime);
        Vector2 position = transform.position;
        position = new Vector2(position.x, position.y + speed * Time.deltaTime);
        transform.position = position;
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        if (transform.position.x < min.x)
       {
            
           transform.position = new Vector2(Random.Range(min.y, max.x), min.x);
        }
         
       
    }
   

}


