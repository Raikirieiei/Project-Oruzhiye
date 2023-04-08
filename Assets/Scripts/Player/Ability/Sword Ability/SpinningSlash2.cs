using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/SpinningSlash2")]
public class SpinningSlash2 : Ability
{
    public GameObject slashEffect;
    public override void Activate(GameObject parent){
        Debug.Log("Spin");
        SwordmanCombat swordmanCombat = parent.GetComponent<SwordmanCombat>();
        Player player = parent.GetComponent<Player>();
        CharacterStats stats = parent.GetComponent<CharacterStats>();
        GameObject projectile = Instantiate(slashEffect, swordmanCombat.spinningSlashEffect2.transform.position, Quaternion.Euler(0, 0, -46*player.facingDir));
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(swordmanCombat.spinningPoint2.position, swordmanCombat.spinningRange2 , 0, swordmanCombat.enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {   
            Debug.Log("Hit with skill"+ enemy.name);
            double damage = stats.baseAttack.getValue() * 2.5;
            enemy.GetComponent<Enemy>().TakeDamage((int)damage);
        }
    }

}
