using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Abilities/CrossSlash")]
public class CrossSlash : Ability
{
    public override void Activate(GameObject parent){
        SpearmanCombat spearmanCombat = parent.GetComponent<SpearmanCombat>();
        CharacterStats stats = parent.GetComponent<CharacterStats>();
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(spearmanCombat.slashPoint.position, spearmanCombat.slashRange , 0, spearmanCombat.enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {   
            Debug.Log("Hit with skill"+ enemy.name);
            double damage = stats.baseAttack.getValue() * 1.5;
            enemy.GetComponent<Enemy>().TakeDamage((int)damage);
        }
    }
}
