using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;

    public LayerMask enemyLayers;

    public Animator animator;
    private CharacterStats characterStats;

    [SerializeField]
    public Vector2 attackRange;
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
            animator.SetBool("Attack", false);
            if(Input.GetKeyDown(KeyCode.Z)){
                animator.SetBool("Attack", true);
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void Attack(){
        Debug.Log("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, attackRange,0, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {   
            Debug.Log("Hit"+ enemy.name + "Atk =" + attackDamage);
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

   void OnLevelWasLoaded(){
        attackDamage = characterStats.baseAttack.getValue();
   }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(attackPoint.position, attackRange);
    }
}
