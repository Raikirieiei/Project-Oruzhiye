using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Player : MonoBehaviour
{    
    float horizontalMove = 0f;
    private Vector2 movementInput;
    public Animator animator;
    public float runSpeed;


    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask obstaclesLayer;
    [SerializeField] Vector2 boxSize;

    private bool isGrounded;
    
 
    public int currentHealth;

 
    public int defend;

    // private string GROUND_TAG = "Ground";
    private string ENEMY_TAG = "Enemy";

    public Rigidbody2D myBody;
    private SpriteRenderer sr;
    private Animator anim;
    private BoxCollider2D myBodyColl;
    private CharacterStats characterStats;

    public HealthBar healthBar;
    private GameObject playerSet;

    private int PLAYER_LAYER;
    private int ENEMY_LAYER;

    private bool isInvincible = false;

    private bool m_FacingRight = true;
    public int facingDir = 1;
    [SerializeField] private float jumpForce;

	// private float invincibleTime = 0.1f;

    public float DashForce;
    public float StartDashTimer;
    public float DashCoolDown;
    float CurrentDashTimer;
    float DashTime;
    bool canDash = true;
    bool isDashing;

    private Vector3 m_Velocity = Vector3.zero;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;

    
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
        GameManager.OnGameStateChanged += ChangeStatOnGameStageChanged;
        runSpeed = (float)characterStats.baseMoveSpeed.getValue();
        currentHealth = characterStats.currentHealth;
        defend = characterStats.baseDefend.getValue();
        healthBar.SetMaxHealth(currentHealth);
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        characterStats.currentHealth = currentHealth;
        movementInput.x = Input.GetAxisRaw("Horizontal");
        PlayerMoveKeyboard();
        PlayerDash();
        PlayerJump();
    }

    void FixedUpdate() {
        isGrounded = Physics2D.OverlapBox(groundCheck.position, boxSize, 0, obstaclesLayer);

        if(Time.time >= DashTime + DashCoolDown){
            canDash = true;
        }

        AnimationController();
    }

    private void ChangeStatOnGameStageChanged(GameState state) {
        Debug.Log("changeStat");
        if(state == GameState.AdjustStat){
            runSpeed = (float)characterStats.baseMoveSpeed.getValue();
            currentHealth = characterStats.currentHealth;
            defend = characterStats.baseDefend.getValue();
            GameManager.instance.UpdateGameState(GameState.Normal);
        }
    }

    private void OnDestroy() {
        GameManager.OnGameStateChanged -= ChangeStatOnGameStageChanged;
    }

    void PlayerMoveKeyboard(){
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        Vector3 targetVelocity = new Vector2(horizontalMove, myBody.velocity.y);
		myBody.velocity = Vector3.SmoothDamp(myBody.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
        if (horizontalMove < 0 && m_FacingRight)
        {
            Flip();
        }
        else if (horizontalMove > 0 && !m_FacingRight)
        {
            Flip();
        }
    }

    public void PlayerDash(){
        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash){  
            isDashing = true;
            Physics2D.IgnoreLayerCollision(PLAYER_LAYER, ENEMY_LAYER, true);
            CurrentDashTimer = StartDashTimer;
            myBody.velocity = Vector2.zero;
            canDash = false;
            DashTime = Time.time;
        } 

        if (isDashing){
            myBody.velocity = new Vector2(facingDir * DashForce,0);
            CurrentDashTimer -= Time.deltaTime;
            if(CurrentDashTimer <= 0){
                isDashing = false;
                Physics2D.IgnoreLayerCollision(PLAYER_LAYER, ENEMY_LAYER, false);
            }
        } 
    }


    public void PlayerJump(){
        if ((Input.GetButtonDown("Jump") && isGrounded || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded) {
            myBody.AddForce(new Vector2(myBody.velocity.x, jumpForce), ForceMode2D.Impulse);
        }
    }

    public void Flip()
	{
		m_FacingRight = !m_FacingRight;
		transform.Rotate(0f, 180f, 0f);
        facingDir *= -1;
	}

    public Vector2 getMovementInput(){
        return movementInput;
    }

    public void TakeDamage(int damage, Vector2 damageDirection){

        int finalDamage;
        float damageReduction;
        damageReduction = damage * (float)defend/100;
        finalDamage = damage - (int)damageReduction;
        Debug.Log("Damage Receive" + finalDamage);

        if (!isInvincible)
        {
            currentHealth -= finalDamage;
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
        myBody.AddForce(damageDirection.normalized * -30f, ForceMode2D.Impulse);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {   
        Vector2 damageDirection = (collision.contacts[0].point - (Vector2)transform.position).normalized;
        damageDirection.y = 0f;
  
        if (collision.gameObject.CompareTag(ENEMY_TAG))
        {
            if(!isInvincible){
                TakeDamage(20, damageDirection);
            }
        }
           
    }

    
    public void AnimationController()
    {
        animator.SetFloat("Speed", Mathf.Abs(movementInput.x * runSpeed));
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isDashing", isDashing);
    }

    public void Die(){
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

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(groundCheck.position, boxSize);
    }

}
