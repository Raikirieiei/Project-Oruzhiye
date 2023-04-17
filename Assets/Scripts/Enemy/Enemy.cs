using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Basic Attributes")]
    public int maxHealth = 100;
    public int currentHealth;

    protected Transform player;
    protected Rigidbody2D enemyRB;
    protected Animator enemyAnim;
    public GameObject damagePopUp;

    [Header("Lootdrop Settings")]
    [SerializeField] int dropAmount;
    [SerializeField] GameObject[] itemDropsList;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        enemyRB = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();

        Invoke(nameof(Find_player), 1);
        if (player == null) return;
    }

    protected void Find_player()
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

    public virtual void TakeDamage(int damage){
        float playerPosition = player.position.x - transform.position.x;
        float knockbackDir = -playerPosition/Math.Abs(playerPosition);
        currentHealth -= damage;

        // Instantiate Damagepopup
        GameObject dmgPopUp = Instantiate(damagePopUp, transform.position, Quaternion.identity);
        dmgPopUp.transform.GetChild(0).GetComponent<TextMesh>().text = damage.ToString();

        enemyRB.velocity = Vector3.zero;
        enemyRB.AddForce(new Vector2(2 * knockbackDir, 1), ForceMode2D.Impulse);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected void Die(){
        Debug.Log("Enemy Die");

        GetComponent<Collider2D>().enabled = false;
        enemyAnim.SetBool("isDeath", true);
        enemyRB.simulated = false;
        this.enabled = false;
        RandomDrop();
        Destroy(gameObject, 0.5f);
    }

    protected void RandomDrop()
    {
        if (dropAmount == 0) return;

        int[] numsArr = getUniqueRandomArray(0, itemDropsList.Length, dropAmount);
        // instantiate items from the list
        foreach (int i in numsArr)
        {
            if (itemDropsList[i] == null) continue;

            GameObject gameObject = Instantiate(itemDropsList[i], transform.position, Quaternion.identity);
            Rigidbody2D dropRB = gameObject.GetComponent<Rigidbody2D>();
            dropRB.AddForce(new Vector2(UnityEngine.Random.Range(-0.5f, 0.5f), 1f), ForceMode2D.Impulse);
        }
    }

    public static int[] getUniqueRandomArray(int min, int max, int count) 
    {
        int[] result = new int[count];
        List<int> numbersInOrder = new List<int>();
        for (var x = min; x < max; x++) {
            numbersInOrder.Add(x);
        }
        for (var x = 0; x < count; x++) {
            var randomIndex = UnityEngine.Random.Range(0, numbersInOrder.Count);
            result[x] = numbersInOrder[randomIndex];
            numbersInOrder.RemoveAt(randomIndex);
        }
        return result;
    }
}
