using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;
    // public Transform spinningPoint;

    public LayerMask enemyLayers;

    public Animator animator;
    public CharacterStats characterStats;

    public Vector2 attackRange;
    // public float attackRate = 2f;
    // protected float nextAttackTime = 0f;

    public int attackDamage;

    protected void Awake(){
        characterStats = GetComponent<CharacterStats>();
    }

    protected void Start(){
        GameManager.OnGameStateChanged -= ChangeStatOnGameStageChanged;
        attackDamage = characterStats.baseAttack.getValue();
    }

    protected void Update() { 
        if(Input.GetKeyDown(KeyCode.Z)){
            animator.SetBool("Attack", true);
        }
    }

    protected void SetAttackFalse(){
        animator.SetBool("Attack", false);
    }

    public void ChangeStatOnGameStageChanged(GameState state) {
        Debug.Log("changeStat");
        if(state == GameState.AdjustStat){
            attackDamage = characterStats.baseAttack.getValue();
            GameManager.instance.UpdateGameState(GameState.Normal);
        }
    }

        private void OnDestroy() {
        GameManager.OnGameStateChanged -= ChangeStatOnGameStageChanged;
    }

    public void Attack(){
        Debug.Log("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, attackRange,0, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {   
            Debug.Log("Hit"+ enemy.name + "Atk =" + attackDamage);
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    public virtual void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(attackPoint.position, attackRange);
        // Gizmos.DrawWireCube(spinningPoint.position, spinningRange);
    }
}
