using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatSwordmanCombat : PlayerCombat
{
    public Transform chargeSlashPoint;
    public Vector2 chargeSlashRange;
    public GameObject chargeEffect;
    public GameObject backProjectile;
    public GameObject frontProjectile;
    public CharacterStats stats;
    public Player player;

    public override void OnDrawGizmosSelected() 
    {
        base.OnDrawGizmosSelected();
        Gizmos.DrawWireCube(chargeSlashPoint.position, chargeSlashRange);
    }

    public void GSStopMoving(){
        player.runSpeed = 0;
        player.canDashGS = false;
    }

    public void GSReturnToNormal(){
        player.runSpeed = stats.baseMoveSpeed.getValue();
        player.canDashGS = true;
    }
}
