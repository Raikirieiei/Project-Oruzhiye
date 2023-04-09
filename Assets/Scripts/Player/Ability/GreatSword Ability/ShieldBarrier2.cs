using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Abilities/ShieldBarrier2")]
public class ShieldBarrier2 : Ability
{   
    public GameObject shieldBarrierEffect;
    public override void Activate(GameObject parent){
        GreatSwordmanCombat greatSwordmanCombat = parent.GetComponent<GreatSwordmanCombat>();
        CharacterStats stats = parent.GetComponent<CharacterStats>();
        GameObject projectile = Instantiate(shieldBarrierEffect, parent.transform.position, parent.transform.rotation);
        Player player = parent.GetComponent<Player>();
        player.defend = stats.baseDefend.getValue() + 70;
    }
}
