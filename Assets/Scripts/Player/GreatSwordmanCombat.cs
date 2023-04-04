using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatSwordmanCombat : PlayerCombat
{
    public Transform chargeSlashPoint;
    public Vector2 chargeSlashRange;
    public CharacterStats stats;
    public Player player;

    public override void OnDrawGizmosSelected() 
    {
        base.OnDrawGizmosSelected();
        Gizmos.DrawWireCube(chargeSlashPoint.position, chargeSlashRange);
    }

    public void GSStopMoving(){
        player.runSpeed = 0;
    }

    public void GSReturnToNormal(){
        player.runSpeed = stats.baseMoveSpeed.getValue();
    }
}
