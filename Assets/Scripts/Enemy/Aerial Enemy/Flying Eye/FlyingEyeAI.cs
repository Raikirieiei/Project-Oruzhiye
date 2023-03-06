using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FlyingEyeAI : MonoBehaviour
{
    [Header("For Flight")]
    [SerializeField] float flightSpeed;
    [SerializeField] float chaseSpeed;
    [SerializeField] List<Transform> waypoints = new List<Transform>();
    private bool canMove = true;
    private Transform nextWaypoint;
    private int waypointNum = 0;
    public float waypointReachedDistance = 0.1f;

    [SerializeField] LayerMask obstaclesLayer;
    private float moveDirection = 1;
    private bool facingRight = true;

    [Header("For Attacking")]
    [SerializeField] int attackDamage;
    [SerializeField] float attackCooldown;
    private float attackTime;
    private bool canAttack;
    private int atkPatternValue = 0;

    [Header("Bite Attack")]
    [SerializeField] Transform attackHitbox1;
    [SerializeField] Vector2 hitboxSize1;
    [SerializeField] Vector2 attackRange1;
    private bool inRangeAttack1;

    [Header("Spin Attack")]
    [SerializeField] float dashDistance;
    [SerializeField] Transform attackHitbox2;
    [SerializeField] float hitboxRadius;
    [SerializeField] Vector2 attackRange2;
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

        nextWaypoint = waypoints[waypointNum];

        Invoke(nameof(Find_player), 1);
        if (player == null) return;
    }

    void FixedUpdate()
    {
        if (player == null) return;
        canSeePlayer = Physics2D.OverlapBox(transform.position, lineOfSight, 0, playerLayer);

        inRangeAttack1 = Physics2D.OverlapBox(transform.position, attackRange1, 0, playerLayer);
        inRangeAttack2 = Physics2D.OverlapBox(transform.position, attackRange2, 0, playerLayer);

        if (Time.time >= attackTime + attackCooldown)
        {
            canAttack = true;
        }

        AnimationController();

        if (!canSeePlayer && canMove)
        {
            Flight();
        } else if (canSeePlayer && canMove)
        {
            MoveTowardPlayer();
        } else if (canAttack)
        {
            canMove = true;
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

    void Flight()
    {
        FlipTowardsWaypoint();
        Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;

        float distance = Vector2.Distance(nextWaypoint.position, transform.position);
        enemyRB.velocity = directionToWaypoint * flightSpeed;

        if (distance <= waypointReachedDistance)
        {
            waypointNum++;

            if (waypointNum >= waypoints.Count)
            {
                waypointNum = 0;
            }
            nextWaypoint = waypoints[waypointNum];
        }
    }

    void MoveTowardPlayer()
    {
        FlipTowardsPlayer();
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        enemyRB.velocity = directionToPlayer * chaseSpeed;
    }

    void attackOnCooldown()
    {
        canAttack = false;
        attackTime = Time.time;
        canMove = false;
    }

    // Attack Pattern 1: Attack enemy in front and deal damage
    void BiteAttack()
    {
        enemyRB.velocity = Vector3.zero;
        atkPatternValue += 1;
    
        bool playerHit = Physics2D.OverlapBox(attackHitbox1.position, hitboxSize1, 0, playerLayer);
        if (playerHit)
        {
            Debug.Log("player hit by bite attack: -" + attackDamage + " HP");
            Player playerScript = player.GetComponent<Player>();
            playerScript.TakeDamage(attackDamage);
        }
    }

    // Attack Pattern 2: Spinning toward player and deal damage
    void SpinAttack()
    {
        enemyRB.velocity = Vector3.zero;
        atkPatternValue = 0;

        float playerDir = playerDirection();
        enemyRB.AddForce(new Vector2(playerDir * dashDistance, 0), ForceMode2D.Impulse);

        bool playerHit = Physics2D.OverlapCircle(attackHitbox2.position, hitboxRadius, playerLayer);
        if (playerHit)
        {
            Debug.Log("player hit by spinning attack: -" + attackDamage + " HP");
            Player playerScript = player.GetComponent<Player>();
            playerScript.TakeDamage(attackDamage + 5);
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

    void FlipTowardsWaypoint()
    {
        float waypointPosition = nextWaypoint.position.x - transform.position.x;

        if (waypointPosition < 0 && facingRight)
        {
            Flip();
        }
        else if (waypointPosition > 0 && !facingRight)
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
        enemyAnim.SetBool("canSeePlayer", canSeePlayer);
        enemyAnim.SetBool("canAttack", canAttack);
        enemyAnim.SetBool("inRangeAttack1", inRangeAttack1);
        enemyAnim.SetBool("inRangeAttack2", inRangeAttack2);
        enemyAnim.SetInteger("atkPatternValue", atkPatternValue);
    }

    private void OnDrawGizmosSelected() 
    {
        // Gizmos.color = Color.green;
        // Gizmos.DrawWireCube(centerCheckPoint.position, petrollingRange);
        // Gizmos.DrawWireSphere(frontCheckPoint.position, circleRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, lineOfSight);

        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(attackHitbox1.position, hitboxSize1);
        Gizmos.DrawWireSphere(attackHitbox2.position, hitboxRadius);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, attackRange1);
        Gizmos.DrawWireCube(transform.position, attackRange2);
    }
}
