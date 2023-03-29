using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;
    public Transform spinningPoint;

    public LayerMask enemyLayers;

    public Animator animator;
    private CharacterStats characterStats;

    public Vector2 attackRange;
    public Vector2 spinningRange;
    public float attackRate = 2f;
    float nextAttackTime = 0f;

    public int attackDamage;

    void Awake(){
        characterStats = GetComponent<CharacterStats>();
    }

    void Start(){
        GameManager.OnGameStateChanged -= ChangeStatOnGameStageChanged;
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

    private void ChangeStatOnGameStageChanged(GameState state) {
        Debug.Log("changeStat");
        if(state == GameState.AdjustStat){
            attackDamage = characterStats.baseAttack.getValue();
            GameManager.instance.UpdateGameState(GameState.Normal);
        }
    }

        private void OnDestroy() {
        GameManager.OnGameStateChanged -= ChangeStatOnGameStageChanged;
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
        Gizmos.DrawWireCube(spinningPoint.position, spinningRange);
    }
}
