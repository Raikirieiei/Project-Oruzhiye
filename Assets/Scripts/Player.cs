using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{    
    [SerializeField]
    private float moveForce = 10f;
    // [SerializeField]
    // private float jumpForce = 11f;
    // public float maxVelocity = 22f;
    [SerializeField]
    private float dashForce = 20f;
    private float activeMoveForce;

    private float dashLength =.1f, dashCooldown = 1f;
    private float dashCounter;
    private float dashCoolCounter;

    // private bool dash = false;

    private float movementX;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMoveKeyboard();
        PlayerDash();
        // AnimatePlayer();
        // PlayerJump();
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
    
}
