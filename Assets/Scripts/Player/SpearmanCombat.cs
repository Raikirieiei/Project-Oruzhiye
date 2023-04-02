using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearmanCombat : PlayerCombat
{
    public Transform slashPoint;
    public Vector2 slashRange;

    public override void OnDrawGizmosSelected() 
    {
        base.OnDrawGizmosSelected();
        Gizmos.DrawWireCube(slashPoint.position, slashRange);
    }
}
