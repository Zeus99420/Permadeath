using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArmament : MonoBehaviour
{
    [HideInInspector] public Transform player;
    [HideInInspector] public Mastermind mastermind;
    protected List<Vector2> playerPositions;
    protected List<Vector2> positionRecord = new List<Vector2>();
    // Start is called before the first frame update

    void FixedUpdate()
    {
        positionRecord.Add(transform.position);
        if (positionRecord.Count > 50) positionRecord.RemoveAt(0);
    }
    public virtual void Initialize()
    {
        playerPositions = player.GetComponent<PlayerMovement>().positionRecord;
        for (int i=0; i<25; i++)
        {
            positionRecord.Add(transform.position);
        }
    }

    protected Vector2 interceptPosition;
    protected Vector2 interceptDirection;
    protected float interceptDistance;

    protected void LeadTarget(float reachVelocity, Vector2 offset, float maxDistance = Mathf.Infinity, float extraLeadTime=0)
    {
        //Calculates where the player is moving so that guns can aim in front of the player
        Vector2 targetPos = player.position;

        float distance = Vector2.Distance(transform.position, targetPos);
        if (distance > maxDistance) distance = maxDistance;
        float timeToReach = distance / reachVelocity + extraLeadTime;

        Vector2 movement = ((Vector2)player.position - playerPositions[24])*2 + positionRecord[24] - (Vector2)transform.position;

        interceptPosition = targetPos + (movement + offset) * timeToReach;
        interceptDistance = Vector2.Distance(interceptPosition, transform.position);
        interceptDirection = (interceptPosition - (Vector2)transform.position).normalized;
    }

    protected void LeadTarget(float reachVelocity, float maxDistance = Mathf.Infinity, float extraLeadTime = 0)
    {
        //Vector2.zero is used as a default value for offset
        LeadTarget(reachVelocity, Vector2.zero, maxDistance, extraLeadTime);
    }

    public bool IsInScreen(float margin)
    {
        //Kollar om fienden är en bit inom skärmen. Används t ex så att spelaren inte ska bli skjuten av en fiende som inte syns.
        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        if (0 + margin < viewportPosition.x && viewportPosition.x < 1 - margin && 0 + margin < viewportPosition.y && viewportPosition.y < 1 - margin)
        {
            return true;
        }
        else return false;

    }
}
