using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordmanCombat : PlayerCombat
{
    public Transform spinningPoint;
    public Vector2 spinningRange;

    public override void OnDrawGizmosSelected() 
    {
        base.OnDrawGizmosSelected();
        Gizmos.DrawWireCube(spinningPoint.position, spinningRange);
    }
}
