using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;

    public LayerMask enemyLayers;

    public Animator animator;
    
    public float attackRange = 0.5f;
    public int attackDamage = 50;

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
