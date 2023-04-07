using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAI : MonoBehaviour
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
    private bool isRetreating = false;

    [Header("For Attacking")]
    [SerializeField] private float stepDistance;
    [SerializeField] int attackDamage;
    [SerializeField] float attackCooldown;
    private float attackTime;
    private bool canAttack = true;
    private int atkPatternValue = 0;

    [Header("For Attack Pattern 1")]
    [SerializeField] Transform attackHitbox1;
    [SerializeField] Vector2 attackRange1;
    [SerializeField] Vector2 hitboxSize1;
    private bool inRangeAttack1;

    [Header("For Attack Pattern 2")]
    [SerializeField] Transform attackHitbox2;
    [SerializeField] Vector2 attackRange2;
    [SerializeField] Vector2 hitboxSize2;
    private bool inRangeAttack2;

    [Header("For Zoning - yellow")]
    [SerializeField] Vector2 retreatRange;
    private bool inRetreatRange;

    [Header("For Seeing Player - red")]
    [SerializeField] Transform player;
    [SerializeField] Vector2 lineOfSight;
    [SerializeField] LayerMask playerLayer;
    private bool canSeePlayer;

    [Header("Other")]
    private Animator enemyAnim;
    private Rigidbody2D enemyRB;
    private SkeletonEnemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
        enemy = GetComponent<SkeletonEnemy>();

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
        inRetreatRange = Physics2D.OverlapBox(transform.position, retreatRange, 0, playerLayer);

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
        } else if (inRetreatRange & !canAttack)
        {
            MoveAwayFromPlayer();
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
        isRetreating = false;
        if (checkingWall || !checkingGround)
        {
            if (facingRight)
            {
                Flip();
            } else if (!facingRight)
            {
                Flip();
            }
        }
        enemyRB.AddForce(new Vector2(moveSpeed * 0.75f * moveDirection, 0));
    }

    void Stop()
    {
        enemyRB.velocity = Vector3.zero;
    }

    void MoveTowardPlayer()
    {
        float playerDir = playerDirection();
        isRetreating = false;

        if (checkingGround)
        {
            FlipTowardsPlayer();
            enemyRB.AddForce(new Vector2(moveSpeed * playerDir, 0));
        }
    }

    void MoveAwayFromPlayer()
    {
        float playerDir = playerDirection();
        isRetreating = true;

        if (checkingGround)
        {
            FlipTowardsPlayer();
            enemyRB.AddForce(new Vector2(moveSpeed * 0.75f * -playerDir, 0));
        }
    }

    void attackOnCooldown()
    {
        canAttack = false;
        attackTime = Time.time;
        if (atkPatternValue < 3)
        {
            atkPatternValue += 2;
        } else
        {
            atkPatternValue -=3;
        }
    }

    void Step()
    {
        enemyRB.AddForce(new Vector2(stepDistance * moveDirection, 0), ForceMode2D.Impulse);
    }

    void Attack1()
    {
        float playerDir = playerDirection();
        enemyRB.velocity = Vector3.zero;

        // enable attack hitbox 1
        bool playerHit = Physics2D.OverlapBox(attackHitbox1.position, hitboxSize1, 0, playerLayer);
        if (playerHit)
        {
            Debug.Log("player hit by Attack1: -" + attackDamage + " HP");
            Player playerScript = player.GetComponent<Player>();
            playerScript.TakeDamage(attackDamage, new Vector2(-playerDir, 0f));
        }
    }

    void Attack2()
    {
        float playerDir = playerDirection();
        enemyRB.velocity = Vector3.zero;

        // enable attack hitbox 2
        bool playerHit = Physics2D.OverlapBox(attackHitbox2.position, hitboxSize2, 0, playerLayer);
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
        enemyAnim.SetBool("canSeePlayer", canSeePlayer);
        enemyAnim.SetBool("canAttack", canAttack);
        enemyAnim.SetBool("inRangeAttack1", inRangeAttack1);
        enemyAnim.SetBool("inRangeAttack2", inRangeAttack2);
        enemyAnim.SetBool("isRetreating", isRetreating);
        enemyAnim.SetInteger("atkPatternValue", atkPatternValue);
    }

    private void OnDrawGizmosSelected() 
    {
        //groundCheckPoint & wallCheckPoint marker
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheckPoint.position, circleRadius);
        Gizmos.DrawWireSphere(wallCheckPoint.position, circleRadius);

        // Protect & Retreat Range marker
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, retreatRange); 

        // LineOfSight marker
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, lineOfSight);

        // Attack Hitbox marker
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(attackHitbox1.position, hitboxSize1);
        Gizmos.DrawWireCube(attackHitbox2.position, hitboxSize2);

        // Attack Range marker
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, attackRange1);
        Gizmos.DrawWireCube(transform.position, attackRange2); 
    }
}
