using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Player : MonoBehaviour
{    
    public PlayerController controller;
    float horizontalMove = 0f;
    private Vector2 movementInput;
    public float runSpeed;
    [HideInInspector]
    // public float normalRunSpeed = 40f;
    bool jump = false;
    bool dash = false;
    
    [HideInInspector]
    public int currentHealth;

    // private string GROUND_TAG = "Ground";
    private string ENEMY_TAG = "Enemy";

    private Rigidbody2D myBody;
    private SpriteRenderer sr;
    private Animator anim;
    private BoxCollider2D myBodyColl;
    private CharacterStats characterStats;

    public HealthBar healthBar;
    private GameObject playerSet;

    private int PLAYER_LAYER;
    private int ENEMY_LAYER;

    private bool isInvincible = false;

    private void Awake(){

        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        myBodyColl = GetComponent<BoxCollider2D>();
        characterStats = GetComponent<CharacterStats>();
        PLAYER_LAYER = LayerMask.NameToLayer("Player");
		ENEMY_LAYER = LayerMask.NameToLayer("Enemy");
    }

    

    // Start is called before the first frame update
    void Start()
    {   
        runSpeed = (float)characterStats.baseMoveSpeed.getValue();
        currentHealth = characterStats.currentHealth;
        healthBar.SetMaxHealth(currentHealth);
        DontDestroyOnLoad(gameObject);
        gameObject.GetComponent<SpriteRenderer>().flipX = true;
    }

    // Update is called once per frame
    void Update()
    {
        characterStats.currentHealth = currentHealth;
        movementInput.x = Input.GetAxisRaw("Horizontal");
        PlayerMoveKeyboard();
        PlayerDash();
        // AnimatePlayer();
        PlayerJump();
        controller.Move(horizontalMove * Time.fixedDeltaTime, dash, jump);
    }

    void FixedUpdate() {
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

    public Vector2 getMovementInput(){
        return movementInput;
    }

    public void TakeDamage(int damage, Vector2 damageDirection){


        if (!isInvincible)
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                isInvincible = true;
                Physics2D.IgnoreLayerCollision(PLAYER_LAYER, ENEMY_LAYER, true);
                StartCoroutine(VulnerableAgain(1f)); 

                // Apply knockback
                KnockBack(damageDirection);
        }
        }
    }

    public void KnockBack(Vector2 damageDirection){
        myBody.AddForce(damageDirection.normalized * -40f, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {   
        Vector2 damageDirection = (collision.contacts[0].point - (Vector2)transform.position).normalized;
        damageDirection.y = 0f;
  
        if (collision.gameObject.CompareTag(ENEMY_TAG))
        {
            if(!isInvincible){
                TakeDamage(20, damageDirection);
                Debug.Log("-20 HP");
            }
        }
           
    }

    void Die(){
        playerSet = GameObject.FindWithTag("PlayerSet");
        Destroy(playerSet);
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

    IEnumerator VulnerableAgain(float delay)
    {
        yield return new WaitForSeconds(delay);
        Physics2D.IgnoreLayerCollision(PLAYER_LAYER, ENEMY_LAYER, false);
        isInvincible = false;
    }

}
