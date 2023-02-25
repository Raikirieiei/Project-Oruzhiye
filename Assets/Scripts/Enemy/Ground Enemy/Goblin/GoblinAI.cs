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

    [Header("For Jump Attacking")]
    [SerializeField] float jumpHeight;
    [SerializeField] Transform player;
    [SerializeField] Transform groundCheck;
    [SerializeField] Vector2 boxSize;
    private bool isGrounded;

    [Header("Attack Pattern 1")]
    [SerializeField] Vector2 attackRange;
    private bool playerInRange1;

    [Header("Attack Pattern 2")]
    private float backFlipDistance;
    private float dashDistance;
    private bool playerInRange2;

    [Header("For Seeing Player")]
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
        isGrounded = Physics2D.OverlapBox(groundCheck.position, boxSize, 0, obstaclesLayer);
        canSeePlayer = Physics2D.OverlapBox(transform.position, lineOfSight, 0, playerLayer);
    
        AnimationController();
        if (!canSeePlayer && isGrounded)
        {
            Petrolling();
        } else if (canSeePlayer && isGrounded)
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
        enemyRB.velocity = new Vector2(moveSpeed * moveDirection, enemyRB.velocity.y);
    }

    void MoveTowardPlayer()
    {
        float distanceFromPlayer = player.position.x - transform.position.x;
        float playerDirection = distanceFromPlayer/Math.Abs(distanceFromPlayer);

        if (checkingGround)
        {
            FlipTowardsPlayer();
            enemyRB.velocity = new Vector2(moveSpeed * playerDirection, enemyRB.velocity.y);
        }
    }

    // Attack Pattern 1: Slightly move forward toward player and slash
    void SlashAttack()
    {
        float distanceFromPlayer = player.position.x - transform.position.x;

        enemyRB.AddForce(new Vector2(distanceFromPlayer, 0), ForceMode2D.Impulse);
    }

    // Attack Pattern 2: Backflip and dash forward to the player with fixed amount of distance
    void Dash()
    {
        float distanceFromPlayer = player.position.x - transform.position.x;

        enemyRB.AddForce(new Vector2(distanceFromPlayer, 0), ForceMode2D.Impulse);
    }

    void DashAttack()
    {
      
    }

    void Backflip()
    {
        float distanceFromPlayer = player.position.x - transform.position.x;

        enemyRB.AddForce(new Vector2(distanceFromPlayer, 5), ForceMode2D.Impulse);
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

    void AnimationController()
    {
        enemyAnim.SetFloat("speed", Math.Abs(enemyRB.velocity.x));
        enemyAnim.SetBool("canSeePlayer", canSeePlayer);
        enemyAnim.SetBool("isGrounded", isGrounded);
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheckPoint.position, circleRadius);
        Gizmos.DrawWireSphere(wallCheckPoint.position, circleRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawCube(groundCheck.position, boxSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, lineOfSight);  
    }
}
