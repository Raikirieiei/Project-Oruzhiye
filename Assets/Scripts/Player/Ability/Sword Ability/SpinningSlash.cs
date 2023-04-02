using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/SpinningSlash")]
public class SpinningSlash : Ability
{
    public override void Activate(GameObject parent){
        Debug.Log("Spin");
        SwordmanCombat swordmanCombat = parent.GetComponent<SwordmanCombat>();
        CharacterStats stats = parent.GetComponent<CharacterStats>();
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(swordmanCombat.spinningPoint.position, swordmanCombat.spinningRange , 0, swordmanCombat.enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {   
            Debug.Log("Hit with skill"+ enemy.name);
            enemy.GetComponent<Enemy>().TakeDamage(stats.baseAttack.getValue() * 3);
        }
    }

}
