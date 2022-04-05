using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{

    void Awake()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {


        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;

        Vector3 pos = Camera.main.WorldToViewportPoint(gameObject.transform.position);
        if (pos.x < 0 || pos.x > 1 || pos.y < 0 || pos.y > 1) Cursor.visible = true;
        else Cursor.visible = false;
    }
}
