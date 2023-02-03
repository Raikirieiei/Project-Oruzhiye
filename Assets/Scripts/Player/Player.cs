using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Player : MonoBehaviour
{    
    public PlayerController controller;
    float horizontalMove = 0f;
    public float runSpeed = 40f;
    bool jump = false;
    bool dash = false;
    

    public int maxHealth = 100;
    public int currentHealth;

    // private string GROUND_TAG = "Ground";
    private string ENEMY_TAG = "Enemy";

    private Rigidbody2D myBody;
    private SpriteRenderer sr;
    private Animator anim;
    private BoxCollider2D myBodyColl;

    public HealthBar healthBar;

    private void Awake(){

        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        myBodyColl = GetComponent<BoxCollider2D>();
    }

    

    // Start is called before the first frame update
    void Start()
    {   
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        DontDestroyOnLoad(gameObject);
        gameObject.GetComponent<SpriteRenderer>().flipX = true;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMoveKeyboard();
        PlayerDash();
        // AnimatePlayer();
        PlayerJump();
    }

    void FixedUpdate() {
        controller.Move(horizontalMove * Time.fixedDeltaTime, dash, jump);
        jump = false;
        dash = false;
    }

    void PlayerMoveKeyboard(){

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

    }

    void PlayerDash(){

        if(Input.GetKeyDown(KeyCode.LeftShift)){  
            Debug.Log("dashed");  
            dash = true;       
        }  
    }


    void PlayerJump(){
        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.UpArrow))) {
            jump = true;
        }
    }

    public void TakeDamage(int damage){

        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // // Debug.Log("coll tag: " + collision.gameObject.tag);
        // if (collision.gameObject.CompareTag(GROUND_TAG)) 
        //     isGrounded = true;    
        
        if (collision.gameObject.CompareTag(ENEMY_TAG))
        {
            TakeDamage(20);
            Debug.Log("-20 HP");
        }
           
    }

    void Die(){
        Destroy(gameObject);
        SceneManager.LoadScene("EndMenu");
    }
    
    private void OnLevelWasLoaded(int level)
    {
        FindStartPos();
    }

    void FindStartPos()
    {
        transform.position = GameObject.FindWithTag("StartPos").transform.position;
    }

}
