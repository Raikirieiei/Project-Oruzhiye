using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shopkeeper : MonoBehaviour
{
    [Header("Shop")]

    [Header("Detecting Player")]
    [SerializeField] Vector2 lineOfSight;
    [SerializeField] LayerMask playerLayer;
    private bool canSeePlayer;

    [Header("Others")]
    [SerializeField] Transform player;
    private bool facingRight = false;

    void Start() {
        Invoke(nameof(Find_player), 1);
        if (player == null) return;
    }

    void FixedUpdate() {
        canSeePlayer = Physics2D.OverlapBox(transform.position, lineOfSight, 0, playerLayer);

        if (canSeePlayer)
        {
            FlipTowardsPlayer();
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

    void FlipTowardsPlayer()
    {
        float playerPosition = player.position.x - transform.position.x;

        if (playerPosition < 0 && facingRight)
        {
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
        else if (playerPosition > 0 && !facingRight)
        {
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
    }
    private void OnDrawGizmosSelected() 
    {
        // LineOfSight marker
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, lineOfSight);
    }
}
