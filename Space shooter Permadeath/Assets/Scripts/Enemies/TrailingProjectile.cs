using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailingProjectile : EnemyProjectile
{
    void OnDestroy()
    {
        GetComponentInChildren<TrailRenderer>().transform.parent = null;
        GetComponentInChildren<TrailRenderer>().autodestruct = true;
    }
}
