using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Abilities/ChargeSlash")]
public class ChargeSlash : Ability
{
    public override void Activate(GameObject parent){
        Debug.Log("ChargeSlash");
        GreatSwordmanCombat greatSwordmanCombat = parent.GetComponent<GreatSwordmanCombat>();
        CharacterStats stats = parent.GetComponent<CharacterStats>();
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(greatSwordmanCombat.chargeSlashPoint.position, greatSwordmanCombat.chargeSlashRange , 0, greatSwordmanCombat.enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {   
            Debug.Log("Hit with skill"+ enemy.name);
            double damage = stats.baseAttack.getValue() * 4;
            enemy.GetComponent<Enemy>().TakeDamage((int)damage);
        }
    }
}

