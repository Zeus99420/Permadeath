using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArmament : MonoBehaviour
{
    [HideInInspector] public Transform player;
    [HideInInspector] public Mastermind mastermind;
    protected List<Vector2> playerPositions;
    // Start is called before the first frame update
    public virtual void Initialize()
    {
        playerPositions = player.GetComponent<PlayerMovement>().positionRecord;
    }

    protected Vector2 interceptPosition;
    protected Vector2 interceptDirection;
    protected float interceptDistance;

    protected void LeadTarget(float reachVelocity, Vector2 offset, float maxDistance = Mathf.Infinity, float extraLeadTime=0)
    {

        Vector2 targetPos = player.position;

        float distance = Vector2.Distance(transform.position, targetPos);
        if (distance > maxDistance) distance = maxDistance;
        float timeToReach = distance / reachVelocity + extraLeadTime;

        Vector2 playerMovement = ((Vector2)player.position - playerPositions[30])*2;

        interceptPosition = targetPos + (playerMovement + offset) * timeToReach;
        interceptDistance = Vector2.Distance(interceptPosition, transform.position);
        interceptDirection = (interceptPosition - (Vector2)transform.position).normalized;
    }

    protected void LeadTarget(float reachVelocity, float maxDistance = Mathf.Infinity, float extraLeadTime = 0)
    {
        LeadTarget(reachVelocity, Vector2.zero, maxDistance, extraLeadTime);
    }


}
