using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;

    public LayerMask enemyLayers;

    public Animator animator;
    private CharacterStats characterStats;

    public float attackRange = 1f;

    private int attackDamage;

    void Awake(){
        characterStats = GetComponent<CharacterStats>();
    }

    void Start(){
        attackDamage = characterStats.baseAttack.getValue();
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Z)){
            Debug.Log("Attack");
            Attack();
        }
    }

    void Attack(){
        animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {   
            Debug.Log("Hit"+ enemy.name);
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }
   
   void OnDrawGizmosSelected() {

        if (attackPoint == null)
            return;    

        Gizmos.DrawWireSphere(attackPoint.position, attackRange); 
   }

   
}
