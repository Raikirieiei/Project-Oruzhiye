using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatSwordmanCombat : PlayerCombat
{
    public Transform chargeSlashPoint;
    public Vector2 chargeSlashRange;

    public override void OnDrawGizmosSelected() 
    {
        base.OnDrawGizmosSelected();
        Gizmos.DrawWireCube(chargeSlashPoint.position, chargeSlashRange);
    }
}
