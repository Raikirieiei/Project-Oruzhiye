using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MinotaurAI : MonoBehaviour
{
    [Header("For Petrolling")]
    [SerializeField] float moveSpeed;
    [SerializeField] Transform wallCheckPoint;
    [SerializeField] float circleRadius;
    [SerializeField] LayerMask obstaclesLayer;
    private bool checkingWall;
    private float moveDirection = 1;
    private bool facingRight = true;

    [Header("For Attacking")]
    [SerializeField] int attackDamage;
    [SerializeField] float attackCooldown;
    private float attackTime;
    private bool canAttack = true;
    private int atkPatternValue = 0;

    [Header("Attack Pattern 1")]
    [SerializeField] Transform attackHitbox1;
    [SerializeField] Vector2 attackRange1;
    [SerializeField] Vector2 hitboxSize1;
    private bool inRangeAttack1;

    [Header("Attack Pattern 2")]
    [SerializeField] Transform groundCheck;
    [SerializeField] Vector2 boxSize;
    [SerializeField] float jumpHeight;
    [SerializeField] float dropSpeed;
    [SerializeField] Vector2 attackRange2;
    private bool inRangeAttack2;
    private bool isGrounded;

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
        checkingWall = Physics2D.OverlapCircle(wallCheckPoint.position, circleRadius, obstaclesLayer);
        isGrounded = Physics2D.OverlapBox(groundCheck.position, boxSize, 0, obstaclesLayer);

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
        } else if (canSeePlayer & canAttack & isGrounded)
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
        if (checkingWall)
        {
            if (facingRight)
            {
                Flip();
            } else if (!facingRight)
            {
                Flip();
            }
        }
        enemyRB.AddForce(new Vector2(moveSpeed * 0.5f * moveDirection, 0));
    }

    void MoveTowardPlayer()
    {
        float playerDir = playerDirection();

        if (isGrounded)
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

    // Attack Pattern 1: Swing axe downward at player
    void SlashAttack()
    {
        float playerDir = playerDirection();
        enemyRB.velocity = Vector3.zero;
        atkPatternValue += 1;

        // enable attack 1 hitbox
        bool playerHit = Physics2D.OverlapBox(attackHitbox1.position, hitboxSize1, 0, playerLayer);
        if (playerHit)
        {
            Debug.Log("player hit by Attack1: -" + attackDamage + " HP");
            Player playerScript = player.GetComponent<Player>();
            playerScript.TakeDamage(attackDamage, new Vector2(-playerDir, 0f));
        }
    }

    // Attack Pattern 2: Jump toward player, and Slam the axe downward damaging player hit by it
    void JumpTowardPlayer()
    {
        float distanceFromPlayer = player.position.x - transform.position.x;
        float playerDir = playerDirection();
        atkPatternValue = 0;

        if(isGrounded)
        {
            enemyRB.AddForce(new Vector2(distanceFromPlayer + (playerDir * 10f), jumpHeight), ForceMode2D.Impulse);
        }
    }
    
    void Drop()
    {
        enemyRB.velocity = new Vector2(0, -dropSpeed);
    }

    void Attack()
    {
        float playerDir = playerDirection();
        atkPatternValue = 0;
 
        // enable attack 2 hitbox
        bool playerHit = Physics2D.OverlapBox(attackHitbox1.position, hitboxSize1, 0, playerLayer);
        if (playerHit)
        {
            Debug.Log("player hit by Attack2: -" + attackDamage + " HP");
            Player playerScript = player.GetComponent<Player>();
            playerScript.TakeDamage(attackDamage, new Vector2(-playerDir, 0f));
        }
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
        enemyAnim.SetBool("isGrounded", isGrounded);
        enemyAnim.SetBool("canSeePlayer", canSeePlayer);
        enemyAnim.SetBool("canAttack", canAttack);
        enemyAnim.SetBool("inRangeAttack1", inRangeAttack1);
        enemyAnim.SetBool("inRangeAttack2", inRangeAttack2);
        enemyAnim.SetInteger("atkPatternValue", atkPatternValue);
    }

    private void OnDrawGizmosSelected() 
    {
        //groundCheckPoint & wallCheckPoint marker
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(wallCheckPoint.position, circleRadius);

        // isGrounded marker
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(groundCheck.position, boxSize);

        // LineOfSight marker
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, lineOfSight);

        // Attack Hitbox marker
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(attackHitbox1.position, hitboxSize1);

        // Attack Range marker
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, attackRange1); 
        Gizmos.DrawWireCube(transform.position, attackRange2); 
    }
}
