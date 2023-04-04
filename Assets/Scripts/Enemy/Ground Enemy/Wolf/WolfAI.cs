using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class WolfAI : MonoBehaviour
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
    [SerializeField] float attackCooldown;
    private float attackTime;
    private bool canAttack = true;

    [Header("For Jump Attack")]
    [SerializeField] float jumpHeight;
    [SerializeField] float jumpDistance;
    [SerializeField] Vector2 attackRange;
    private bool inRangeAttack;

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
        inRangeAttack = Physics2D.OverlapBox(transform.position, attackRange, 0, playerLayer);

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
        if (!checkingGround & checkingWall)
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

    void Attack()
    { 
        enemyRB.AddForce(new Vector2(moveDirection + jumpDistance, jumpHeight), ForceMode2D.Impulse);
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
        enemyAnim.SetBool("inRangeAttack", inRangeAttack);
    }

    private void OnDrawGizmosSelected() 
    {
        //groundCheckPoint & wallCheckPoint marker
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(wallCheckPoint.position, circleRadius);

        // LineOfSight marker
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, lineOfSight);

        // Attack Range marker
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, attackRange);
    }
}
