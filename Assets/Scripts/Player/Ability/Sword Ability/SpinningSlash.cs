using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpinningSlash : Ability
{
    public float dashVelocity;
    public override void Activate(GameObject parent){
        Debug.Log("Spin");
        PlayerCombat playerCombat = parent.GetComponent<PlayerCombat>();
        CharacterStats stats = parent.GetComponent<CharacterStats>();
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(playerCombat.attackPoint.position, 2.5f, playerCombat.enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {   
            Debug.Log("Hit with skill"+ enemy.name);
            enemy.GetComponent<Enemy>().TakeDamage(stats.baseAttack.getValue() * 3);
        }
    }

}
