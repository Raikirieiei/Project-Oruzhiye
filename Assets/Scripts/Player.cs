using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{    
    [SerializeField]
    private float moveForce = 10f;
    [SerializeField]
    private float jumpForce = 13f;

    private bool isGrounded;
    // public float maxVelocity = 22f;
    [SerializeField]
    private float dashForce = 20f;
    private float activeMoveForce;

    private float dashLength =.1f, dashCooldown = 1f;
    private float dashCounter;
    private float dashCoolCounter;

    // private bool dash = false;


    private float movementX;

    private string GROUND_TAG = "Ground";

    // private string ENEMY_TAG = "Enemy";

    private Rigidbody2D myBody;

    private SpriteRenderer sr;
    private Animator anim;

    private BoxCollider2D myBodyColl;

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
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMoveKeyboard();
        PlayerDash();
        // AnimatePlayer();
        PlayerJump();
        PlayerAttack();
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
                myBodyColl.enabled = false;
                myBody.bodyType = RigidbodyType2D.Kinematic;
            }

        }

        if(dashCounter > 0){
            dashCounter -= Time.deltaTime;

            if(dashCounter <= 0){
                activeMoveForce = moveForce;
                dashCoolCounter = dashCooldown;
                myBodyColl.enabled = true;
                myBody.bodyType = RigidbodyType2D.Dynamic;
            }
        }

        if(dashCoolCounter > 0){
            dashCoolCounter -= Time.deltaTime;
        }     
    }

    void PlayerJump(){
        if (Input.GetButtonDown("Jump") && isGrounded) {
            isGrounded = false;
            myBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }

    }

    void PlayerAttack(){
         if(Input.GetKeyDown(KeyCode.Z)){
            Debug.Log("Attack");
         }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag(GROUND_TAG)) 
        {
            isGrounded = true;
        }
        
    }

    // Map Interaction Section
    void OnLevelWasLoaded(int level)
    {
        FindStartPos();
    }

    void FindStartPos()
    {
        transform.position = GameObject.FindWithTag("StartPos").transform.position;
    }
}