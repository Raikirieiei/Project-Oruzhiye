using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonEnemy : Enemy
{

    [Header("For Shielding - cyan")]
    [SerializeField] Vector2 guardingRange;
    [SerializeField] float guardCooldown;
    private float guardTime;
    private bool canGuard = true;
    private bool inGuardingRange;

    [Header("For Passive Protection - yellow")]
    [SerializeField] Vector2 protectRange;
    [SerializeField] LayerMask playerLayer;
    private bool inProtectRange;

    void FixedUpdate() {
        inGuardingRange = Physics2D.OverlapBox(transform.position, guardingRange, 0, playerLayer);
        inProtectRange = Physics2D.OverlapBox(transform.position, protectRange, 0, playerLayer);

        if (Time.time >= guardTime + guardCooldown)
        {
            canGuard = true;
        }

        AnimationController();
    }

    public override void TakeDamage(int damage){

      if (!inProtectRange)
      {
        damage = 0;
        enemyAnim.SetTrigger("takeDamage");
      } else if (inGuardingRange & canGuard)
      {
        damage = 0;
        enemyAnim.SetTrigger("takeDamage");
        guardOnCooldown();
      }

      float playerPosition = player.position.x - transform.position.x;
      float knockbackDir = -playerPosition/Math.Abs(playerPosition);
      currentHealth -= damage;

      // Instantiate Damagepopup
      GameObject dmgPopUp = Instantiate(damagePopUp, transform.position, Quaternion.identity);
      dmgPopUp.transform.GetChild(0).GetComponent<TextMesh>().text = damage.ToString();

      enemyRB.velocity = Vector3.zero;
      enemyRB.AddForce(new Vector2(2 * knockbackDir, 1), ForceMode2D.Impulse);

      
      if (currentHealth <= 0)
      {
          Die();
      }
    }

    void guardOnCooldown()
    {
        canGuard = false;
        guardTime = Time.time;
    }

    void AnimationController()
    {
      enemyAnim.SetBool("canGuard", canGuard);
    }

    private void OnDrawGizmosSelected() 
    {
        // Protect & Retreat Range marker
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, protectRange);

        // Guard Range marker
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, guardingRange); 
    }
}
