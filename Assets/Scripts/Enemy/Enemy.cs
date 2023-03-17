using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    private Transform player;
    private Rigidbody2D enemyRB;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        enemyRB = GetComponent<Rigidbody2D>();
    
        Invoke(nameof(Find_player), 1);
        if (player == null) return;
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void TakeDamage(int damage){
        float playerPosition = player.position.x - transform.position.x;
        float knockbackDir = -playerPosition/Math.Abs(playerPosition);
        currentHealth -= damage;

        enemyRB.velocity = Vector3.zero;
        enemyRB.AddForce(new Vector2(5 * knockbackDir, 1), ForceMode2D.Impulse);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die(){
        Debug.Log("Enemy Die");

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        Destroy(gameObject);
    }

    
}
