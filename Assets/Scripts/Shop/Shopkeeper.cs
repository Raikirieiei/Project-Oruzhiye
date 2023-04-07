using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shopkeeper : MonoBehaviour
{
    [Header("Shop")]
    [SerializeField] int cost;
    [SerializeField] string itemDescription;
    [SerializeField] ItemType itemType;
    [SerializeField] GameObject items;
    [SerializeField] StatReward stats;
    [SerializeField] List<StatReward> statusPools;

    [Header("Detecting Player")]
    [SerializeField] Transform player;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] Vector2 lineOfSight;
    private bool canSeePlayer;
    private bool facingRight = false;

    [Header("Others")]
    [SerializeField] AudioSource buyAudio;
    [SerializeField] AudioSource buyFailAudio;
    [SerializeField] GameObject shopDialogBox;
    [SerializeField] GameObject name;
    [SerializeField] GameObject description;
    [SerializeField] GameObject priceTag;
    public GameObject textPopUp;
    public GameObject itemImage;
    private ItemCollector itemCollector;
    private CharacterStats characterStats;

    enum ItemType
    {
        stats,
        health
    };

    void Start() 
    {
        if (itemType == ItemType.stats)
        {
            List<StatReward> statsPool = new List<StatReward>(statusPools);
            stats = statsPool[UnityEngine.Random.Range(0,statusPools.Count)];
        }

        Invoke(nameof(Find_player), 1);
        if (player == null) return;
    }

    void Update()
    {
        if(canSeePlayer & Input.GetKeyUp(KeyCode.F)){
            BuyItems();
        }
    }

    void FixedUpdate() 
    {
        canSeePlayer = Physics2D.OverlapBox(transform.position, lineOfSight, 0, playerLayer);

        ShopUIUpdate();

        if (canSeePlayer)
        {
            shopDialogBox.SetActive(true);
            FlipTowardsPlayer();
        } else {
            shopDialogBox.SetActive(false);
        }
    }

    private void BuyItems()
    {
        itemCollector = player.GetComponent<ItemCollector>();

        if (itemCollector.coins < cost) 
        {   
            GameObject warningText = Instantiate(textPopUp, transform.position, Quaternion.identity);
            warningText.transform.GetChild(0).GetComponent<TextMesh>().text = "NOT ENOUGH MONEY!";
            buyFailAudio.Play();
            return;
        }

        if (itemType == ItemType.health)
        {
            Instantiate(items, player.position, Quaternion.identity);
        } else if (itemType == ItemType.stats)
        {
            characterStats = player.GetComponent<CharacterStats>();
            stats.Selected(characterStats);
        }

        buyAudio.Play();
        GameObject purchaseText = Instantiate(textPopUp, transform.position, Quaternion.identity);
        purchaseText.transform.GetChild(0).GetComponent<TextMesh>().text = "Thank you for the business!";
        purchaseText.transform.GetChild(0).GetComponent<TextMesh>().color = Color.green;
        itemCollector.coins -= cost;
    }

    void ShopUIUpdate()
    {
        priceTag.GetComponent<TextMesh>().text = cost.ToString();
        
        if (itemType == ItemType.health)
        {
            name.GetComponent<TextMesh>().text = "Health Potion";
            description.GetComponent<TextMesh>().text = itemDescription;
        } else if (itemType == ItemType.stats)
        {
            name.GetComponent<TextMesh>().text = stats.name;
            description.GetComponent<TextMesh>().text = stats.desc;
            itemImage.GetComponent<SpriteRenderer>().sprite = stats.img;
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
