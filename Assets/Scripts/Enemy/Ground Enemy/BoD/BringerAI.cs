using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerAI : MonoBehaviour
{
    [Header("For Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] Transform groundCheckPoint;
    [SerializeField] Transform wallCheckPoint;
    [SerializeField] float circleRadius;
    [SerializeField] LayerMask obstaclesLayer;
    private bool checkingGround;
    private bool checkingWall;
    private float moveDirection = -1;
    private bool facingRight = false;
    private bool isRetreating = false;

    [Header("For Attacking")]
    [SerializeField] int attackDamage;
    [SerializeField] float attackCooldown;
    private float attackTime;
    private bool canAttack = true;

    [Header("For Attack Pattern 1")]
    [SerializeField] Transform attackHitbox;
    [SerializeField] Vector2 attackRange;
    [SerializeField] Vector2 hitboxSize;
    private bool inRangeAttack;

    [Header("For Casting Magic")]
    [SerializeField] float castCooldown;
    [SerializeField] float shadowHandSpawnHeight = 2f;
    [SerializeField] GameObject shadowHand;
    [SerializeField] Transform[] shadowHandSpawnPoints;
    private float castTime;
    private bool canCast = true;
    private int spellPatternValue = 0;

    [Header("For Vanishing")]
    [SerializeField] Transform marker;
    [SerializeField] Vector2 markerSize;
    [SerializeField] float vanishDistance;
    [SerializeField] float vanishedCooldown;
    private float vanishedTime;
    private bool inVanishRange;
    private bool canVanish = false;

    [Header("For Zoning - yellow")]
    [SerializeField] Vector2 retreatRange;
    private bool inRetreatRange;

    [Header("For Player Detecting - red")]
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
        inRangeAttack = Physics2D.OverlapBox(transform.position, attackRange, 0, playerLayer);
        inVanishRange = Physics2D.OverlapBox(marker.position, markerSize, 0, playerLayer);
        inRetreatRange = Physics2D.OverlapBox(transform.position, retreatRange, 0, playerLayer);

        
        CooldownHandler();
        AnimationController();

        if (!canSeePlayer)
        {
            Petrolling();
        } else if (canAttack)
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
            enemyRB.AddForce(new Vector2(moveSpeed * -playerDir, 0));
        }
    }

    void CooldownHandler()
    {
        if (Time.time >= attackTime + attackCooldown)
        {
            canAttack = true;
        }

        if (Time.time >= vanishedTime + vanishedCooldown)
        {
            canVanish = true;
        }

        if (Time.time >= castTime + castCooldown)
        {
            canCast = true;
        }
    }

    void attackOnCooldown()
    {
        canAttack = false;
        attackTime = Time.time;
    }

    void vanishOnCooldown()
    {
        canVanish = false;
        vanishedTime = Time.time;
    }

    void castOnCooldown()
    {
        canCast = false;
        castTime = Time.time;
    }

    void Attack()
    {
        float playerDir = playerDirection();
        enemyRB.velocity = Vector3.zero;

        // enable attack hitbox
        bool playerHit = Physics2D.OverlapBox(attackHitbox.position, hitboxSize, 0, playerLayer);
        if (playerHit)
        {
            Debug.Log("player hit by Attack1: -" + attackDamage + " HP");
            Player playerScript = player.GetComponent<Player>();
            playerScript.TakeDamage(attackDamage, new Vector2(-playerDir, 0f));
        }
    }

    void SingleHandAttack()
    {
        Instantiate(shadowHand, new Vector2(
            player.position.x, 
            gameObject.transform.position.y + shadowHandSpawnHeight
            ), 
            Quaternion.identity
        );
        spellPatternValue += 1;
    }

    void MultipleHandAttack(int index)
    {
        Instantiate(shadowHand, shadowHandSpawnPoints[index].position, Quaternion.identity);
        Instantiate(shadowHand, shadowHandSpawnPoints[index+1].position, Quaternion.identity);
        spellPatternValue = 0;
    }

    void TeleportBehindPlayer()
    {
        float destinationX = player.position.x + (vanishDistance * moveDirection);
        if (true)
        {
            gameObject.transform.position = new Vector3(
                player.position.x + (vanishDistance * moveDirection), 
                gameObject.transform.position.y, 
                gameObject.transform.position.z
            );
        } else {
            gameObject.transform.position = marker.position;
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
        enemyAnim.SetBool("canAttack", canAttack);
        enemyAnim.SetBool("canVanish", canVanish);
        enemyAnim.SetBool("canCast", canCast);
        enemyAnim.SetBool("canSeePlayer", canSeePlayer);
        enemyAnim.SetBool("inRangeAttack", inRangeAttack);
        enemyAnim.SetBool("inVanishRange", inVanishRange);
        enemyAnim.SetBool("isRetreating", isRetreating);
        enemyAnim.SetInteger("spellPatternValue", spellPatternValue);
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
        Gizmos.DrawWireCube(attackHitbox.position, hitboxSize);

        // Attack Range marker
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, attackRange);

        // Stage Marker
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(marker.position, markerSize);
    }
}
