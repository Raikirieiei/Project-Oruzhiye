using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Abilities/ChargeSlash2")]
public class ChargeSlash2 : Ability
{
    public GameObject chargeEffect;
    public GameObject frontProjectile;
    public GameObject backProjectile;
    public override void Activate(GameObject parent){
        GreatSwordmanCombat greatSwordmanCombat = parent.GetComponent<GreatSwordmanCombat>();
        CharacterStats stats = parent.GetComponent<CharacterStats>();
        GameObject projectile = Instantiate(chargeEffect, greatSwordmanCombat.chargeEffect.transform.position, greatSwordmanCombat.transform.rotation);
        GameObject projectile2 = Instantiate(frontProjectile, greatSwordmanCombat.frontProjectile.transform.position, parent.transform.rotation);
        GameObject projectile3 = Instantiate(backProjectile, greatSwordmanCombat.backProjectile.transform.position, parent.transform.rotation);
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(greatSwordmanCombat.chargeSlashPoint.position, greatSwordmanCombat.chargeSlashRange , 0, greatSwordmanCombat.enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {   
            Debug.Log("Hit with skill"+ enemy.name);
            double damage = stats.baseAttack.getValue() * 5;
            enemy.GetComponent<Enemy>().TakeDamage((int)damage);
        }
    }
}

