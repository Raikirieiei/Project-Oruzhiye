using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordmanCombat : PlayerCombat
{
    public Transform spinningPoint;
    public Vector2 spinningRange;
    public GameObject spinningSlashEffect;

    public Transform spinningPoint2;
    public Vector2 spinningRange2;
    public GameObject spinningSlashEffect2;

    public override void OnDrawGizmosSelected() 
    {
        base.OnDrawGizmosSelected();
        Gizmos.DrawWireCube(spinningPoint.position, spinningRange);
        Gizmos.color = Color.grey;
        Gizmos.DrawWireCube(spinningPoint2.position, spinningRange2);
    }
}
