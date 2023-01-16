using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{    
    [SerializeField]
    private float moveForce = 10f;
    [SerializeField]
    private float jumpForce = 13f;

    private bool isGrounded;
    // public float maxVelocity = 22f;
    [SerializeField]
    private float dashForce = 30f;
    private float activeMoveForce;

    private float dashLength =.2f, dashCooldown = 1f;
    private float dashCounter;
    private float dashCoolCounter;

  
    public int maxHealth = 100;
    public int currentHealth;

    private float movementX;

    private string GROUND_TAG = "Ground";

    private string ENEMY_TAG = "Enemy";

    private int PLAYER_LAYER = 3;
    private int ENEMY_LAYER = 6;

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
        activeMoveForce = moveForce;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMoveKeyboard();
        PlayerDash();
        // AnimatePlayer();
        PlayerJump();
        RotateAnimation(); //temporary
    }

    private void RotateAnimation() //temporary
    {
        if (Input.GetAxis("Horizontal") > 0.01f)
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        else if (Input.GetAxis("Horizontal") < -0.01f)
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
    }


    void PlayerMoveKeyboard(){

        movementX = Input.GetAxisRaw("Horizontal");

        transform.position += new Vector3(movementX, 0f, 0f) * activeMoveForce * Time.deltaTime ;

    }

    void PlayerDash(){
        
        if(Input.GetKeyDown(KeyCode.LeftShift)){

            if (dashCoolCounter <=0  && dashCounter <= 0){
                activeMoveForce = dashForce;
                dashCounter = dashLength;
                Physics2D.IgnoreLayerCollision(PLAYER_LAYER, ENEMY_LAYER, true);
                // myBody.velocity = Vector2.zero;   
            }

        }

        if(dashCounter > 0){
            dashCounter -= Time.deltaTime;

            if(dashCounter <= 0){
                activeMoveForce = moveForce;
                dashCoolCounter = dashCooldown;
                Physics2D.IgnoreLayerCollision(PLAYER_LAYER, ENEMY_LAYER, false);
            }
        }

        if(dashCoolCounter > 0){
            dashCoolCounter -= Time.deltaTime;
        }     
    }


    void PlayerJump(){
        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded) {
            isGrounded = false;
            myBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }

        // else if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.UpArrow)) && !isGrounded)
        //     Debug.Log("not grounded");
            
        

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
        // Debug.Log("coll tag: " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag(GROUND_TAG)) 
            isGrounded = true;    
        
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
