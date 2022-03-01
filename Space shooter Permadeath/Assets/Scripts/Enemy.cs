using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public float avoidRadius;
    protected Vector2 direction;

    public bool IsInScreen()
    {
        //Kollar om fienden är en bit inom skärmen, så att inte spelaren ska bli skjuten av en fiende som inte syns
        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
         if (0.05f < viewportPosition.x && viewportPosition.x < 0.95f && 0.05f < viewportPosition.y && viewportPosition.y < 0.95f)
        {
            return true;
        }
        else return false;

    }

    public void AvoidCollision()
    {
        Collider2D avoidCollider = null;
        avoidCollider = Physics2D.OverlapCircle(transform.position, avoidRadius, LayerMask.GetMask("Enemy"));

        if (avoidCollider != null)
        {
            Debug.Log("Evasive maneuvers!");
            direction = -((Vector2)avoidCollider.transform.position - (Vector2)transform.position).normalized;
        }
    }
   

}
