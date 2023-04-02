using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Abilities/ShieldBarrier")]
public class ShieldBarrier : Ability
{
    public override void Activate(GameObject parent){
        Debug.Log("ShieldBarrier");
        GreatSwordmanCombat greatSwordmanCombat = parent.GetComponent<GreatSwordmanCombat>();
        CharacterStats stats = parent.GetComponent<CharacterStats>();
        Player player = parent.GetComponent<Player>();
        player.defend = stats.baseDefend.getValue() + 50;
        Debug.Log("player def" + player.defend);
    }
}
