using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/TripleStab")]
public class TripleStab : Ability
{
    public override void Activate(GameObject parent){
        Debug.Log("DashStab");
        SpearmanCombat spearmanCombat = parent.GetComponent<SpearmanCombat>();
        CharacterStats stats = parent.GetComponent<CharacterStats>();
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(spearmanCombat.attackPoint.position, spearmanCombat.attackRange , 0, spearmanCombat.enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {   
            Debug.Log("Hit with skill"+ enemy.name);
            double damage = stats.baseAttack.getValue() * 0.5;
            enemy.GetComponent<Enemy>().TakeDamage((int)damage);
        }
    }
}
