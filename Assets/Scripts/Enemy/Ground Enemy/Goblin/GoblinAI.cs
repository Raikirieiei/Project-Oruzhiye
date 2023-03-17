using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GoblinAI : MonoBehaviour
{
    [Header("For Petrolling")]
    [SerializeField] float moveSpeed;
    [SerializeField] Transform groundCheckPoint;
    [SerializeField] Transform wallCheckPoint;
    [SerializeField] float circleRadius;
    [SerializeField] LayerMask obstaclesLayer;
    private bool checkingGround;
    private bool checkingWall;
    private float moveDirection = 1;
    private bool facingRight = true;

    [Header("For Attacking")]
    [SerializeField] int attackDamage;
    [SerializeField] float attackCooldown;
    private float attackTime;
    private bool canAttack;

    [Header("Attack Pattern 1")]
    [SerializeField] Transform attackHitbox1;
    [SerializeField] Vector2 attackRange1;
    [SerializeField] Vector2 hitboxSize1;
    private bool inRangeAttack1;

    [Header("Attack Pattern 2")]
    [SerializeField] Transform attackHitbox2;
    [SerializeField] Vector2 attackRange2;
    [SerializeField] Vector2 hitboxSize2;
    [SerializeField] private float backFlipDistance;
    [SerializeField] private float dashDistance;
    private bool inRangeAttack2;

    [Header("For Seeing Player")]
    [SerializeField] Transform player;
    [SerializeField] Vector2 lineOfSight;
    [SerializeField] LayerMask playerLayer;
    private bool canSeePlayer;

    [Header("Other")]
    private Animator enemyAnim;
    private Rigidbody2D enemyRB;

    // Start is called before the first frame update
    void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
        Invoke(nameof(Find_player), 1);
        if (player == null) return;
    }

    void FixedUpdate()
    {
        if (player == null) return;
        checkingGround = Physics2D.OverlapCircle(groundCheckPoint.position, circleRadius, obstaclesLayer);
        checkingWall = Physics2D.OverlapCircle(wallCheckPoint.position, circleRadius, obstaclesLayer);

        canSeePlayer = Physics2D.OverlapBox(transform.position, lineOfSight, 0, playerLayer);
        inRangeAttack1 = Physics2D.OverlapBox(transform.position, attackRange1, 0, playerLayer);
        inRangeAttack2 = Physics2D.OverlapBox(transform.position, attackRange2, 0, playerLayer);

        if (Time.time >= attackTime + attackCooldown)
        {
            canAttack = true;
        }

        AnimationController();
        if (!canSeePlayer)
        {
            Petrolling();
        } else if (canSeePlayer & canAttack)
        {
            MoveTowardPlayer();
        }
    }

    private void Find_player()
    {
        try
        {
            player = GameObject.FindWithTag("Player").transform;
        }
        catch (NullReferenceException)
        {
            Debug.Log("target gameObjects is not present in hierarchy ");
        }
    }

    void Petrolling()
    {
        if (!checkingGround || checkingWall)
        {
            if (facingRight)
            {
                Flip();
            } else if (!facingRight)
            {
                Flip();
            }
        }
        enemyRB.AddForce(new Vector2(moveSpeed * moveDirection, 0));
    }

    void MoveTowardPlayer()
    {
        float playerDir = playerDirection();

        if (checkingGround)
        {
            FlipTowardsPlayer();
            enemyRB.AddForce(new Vector2(moveSpeed * playerDir, 0));
        }
    }

    void attackOnCooldown()
    {
        canAttack = false;
        attackTime = Time.time;
    }

    // Attack Pattern 1: Slightly move toward player and slash
    void SlashAttack()
    {
        float playerDir = playerDirection();

        // move toward player
        enemyRB.AddForce(new Vector2(5 * playerDir, 0), ForceMode2D.Impulse);

        // enable attack 1 hitbox
        bool playerHit = Physics2D.OverlapBox(attackHitbox1.position, hitboxSize1, 0, playerLayer);
        if (playerHit)
        {
            Debug.Log("player hit by Attack1: -" + attackDamage + " HP");
            Player playerScript = player.GetComponent<Player>();
            playerScript.TakeDamage(attackDamage);
        }
    }

    // Attack Pattern 2: Backflip and dash forward to the player with fixed amount of distance
    void Dash()
    {
        enemyRB.AddForce(new Vector2(moveDirection * dashDistance, 0), ForceMode2D.Impulse);
    }

    void DashAttack()
    {
        float playerDir = playerDirection();

        // enable attack 2 hitbox
        bool playerHit = Physics2D.OverlapBox(attackHitbox2.position, hitboxSize2, 0, playerLayer);
        if (playerHit)
        {
            Debug.Log("player hit by Attack2: -" + attackDamage + " HP");
            Player playerScript = player.GetComponent<Player>();
            playerScript.TakeDamage(attackDamage);
        }
    }

    void Backflip()
    {
        float jumpDirection = playerDirection() * -1;

        enemyRB.AddForce(new Vector2(backFlipDistance * jumpDirection, 3), ForceMode2D.Impulse);
    }

    void FlipTowardsPlayer()
    {
        float playerPosition = player.position.x - transform.position.x;

        if (playerPosition < 0 && facingRight)
        {
            Flip();
        }
        else if (playerPosition > 0 && !facingRight)
        {
            Flip();
        }
    }

    void Flip() 
    {
        moveDirection *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private float playerDirection()
    {
        float distanceFromPlayer = player.position.x - transform.position.x;
        return distanceFromPlayer/Math.Abs(distanceFromPlayer);
    }

    void AnimationController()
    {
        enemyAnim.SetFloat("speed", Math.Abs(enemyRB.velocity.x));
        enemyAnim.SetBool("canSeePlayer", canSeePlayer);
        enemyAnim.SetBool("canAttack", canAttack);
        enemyAnim.SetBool("inRangeAttack1", inRangeAttack1);
        enemyAnim.SetBool("inRangeAttack2", inRangeAttack2);
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheckPoint.position, circleRadius);
        Gizmos.DrawWireSphere(wallCheckPoint.position, circleRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, lineOfSight);

        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(attackHitbox1.position, hitboxSize1);
        Gizmos.DrawWireCube(attackHitbox2.position, hitboxSize2);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, attackRange1); 
        Gizmos.DrawWireCube(transform.position, attackRange2); 
    }
}
