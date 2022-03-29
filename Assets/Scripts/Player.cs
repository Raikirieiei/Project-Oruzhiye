using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{    [SerializeField]
    private float moveForce = 10f;
    [SerializeField]
    // private float jumpForce = 11f;
    // public float maxVelocity = 22f;

    private float movementX;

    private Rigidbody2D myBody;

    private SpriteRenderer sr;
    private Animator anim;

    private void Awake(){

        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMoveKeyboard();
        // AnimatePlayer();
        // PlayerJump();
    }

    void PlayerMoveKeyboard(){

        movementX = Input.GetAxisRaw("Horizontal");

        transform.position += new Vector3(movementX, 0f, 0f) * moveForce * Time.deltaTime ;
    }

}
