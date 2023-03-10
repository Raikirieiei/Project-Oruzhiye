using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    private int attackDamage;
    private CharacterStats characterStats;
    private int enemyLayer;
    private int playerLayer;

    void Awake(){
        characterStats = GameObject.FindWithTag("Player").GetComponent<CharacterStats>();
        enemyLayer = LayerMask.NameToLayer("Enemy");
        playerLayer = LayerMask.NameToLayer("Player");
    }

    void Start() {
        attackDamage = characterStats.baseAttack.getValue();
    }

    void Update()
    {
        transform.position += transform.right * Time.deltaTime * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (collision.gameObject.layer == playerLayer){
            Physics2D.IgnoreLayerCollision(collision.gameObject.layer, playerLayer, true);
        }
        else if (collision.gameObject.layer == enemyLayer){
            Debug.Log("wavehit");
            enemy.TakeDamage(attackDamage);
            Destroy(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }
}
