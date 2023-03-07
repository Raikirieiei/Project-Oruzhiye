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
    public float attackRate = 2f;
    float nextAttackTime = 0f;

    private int attackDamage;

    void Awake(){
        characterStats = GetComponent<CharacterStats>();
    }

    void Start(){
        attackDamage = characterStats.baseAttack.getValue();
    }

    void Update() {
        if (Time.time >= nextAttackTime){
            if(Input.GetKeyDown(KeyCode.Z)){
                Debug.Log("Attack");
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void Attack(){
        animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {   
            Debug.Log("Hit"+ enemy.name + "Atk =" + attackDamage);
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }
   
   void OnDrawGizmosSelected() {

        if (attackPoint == null)
            return;    

        Gizmos.DrawWireSphere(attackPoint.position, attackRange); 
   }

   void OnLevelWasLoaded(){
        attackDamage = characterStats.baseAttack.getValue();
   }

   
}
