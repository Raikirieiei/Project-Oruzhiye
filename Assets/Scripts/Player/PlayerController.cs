using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 750f;							// Amount of force added when the player jumps.
	private bool jumpOnCooldown = false;
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.

	[SerializeField]
    private float dashForce = 80f;
	private int facingDir = 0;
	private bool dashOnCooldown = false;
	private float dashCooldownTime = 1f;
	private float invincibleTime = 0.1f;

	private int PLAYER_LAYER;
    private int ENEMY_LAYER;

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	private Rigidbody2D m_Rigidbody2D;
	private Vector3 m_Velocity = Vector3.zero;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		PLAYER_LAYER = LayerMask.NameToLayer("Player");
		ENEMY_LAYER = LayerMask.NameToLayer("Enemy");

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
	}

	private void Update(){
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;
		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded){
					OnLandEvent.Invoke();
					StartCoroutine(JumpCoolDown());
				}
			}
		}
	}

	private void HandleDirection(){
			// Left facing
		if (Input.GetKeyDown (KeyCode.A))
			facingDir = -1;
		
		// Right facing
		if (Input.GetKeyDown (KeyCode.D))
			facingDir = 1;
	}


	public void Move(float move, bool dash, bool jump)
	{
		// If crouching, check to see if the character can stand up
		//only control the player if grounded or airControl is turned on
		

		if (m_Grounded || m_AirControl)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);


			// If the input is moving the player right and the player is facing left...
			if (move > 0)
			{
				// ... flip the player.
				facingDir = 1;
				gameObject.GetComponent<SpriteRenderer>().flipX = true; // need to change when using good sprite

			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0)
			{
				// ... flip the player.
				facingDir = -1;
				gameObject.GetComponent<SpriteRenderer>().flipX = false; // need to change when using good sprite
			}
		}
		// If the player should jump...
		if (m_Grounded && jump)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			if(!jumpOnCooldown){
				m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
			}
		}

		if (dash){
		    if (!dashOnCooldown){
				Physics2D.IgnoreLayerCollision(PLAYER_LAYER, ENEMY_LAYER, true);
				m_Rigidbody2D.AddForce(new Vector2(facingDir, 0) * dashForce, ForceMode2D.Impulse);
				StartCoroutine(DashCoolDown ());
     		} 
		}
	}

	IEnumerator DashCoolDown () {
		dashOnCooldown = true;
		yield return new WaitForSeconds (invincibleTime);
		Physics2D.IgnoreLayerCollision(PLAYER_LAYER, ENEMY_LAYER, false);
		yield return new WaitForSeconds (dashCooldownTime);
		dashOnCooldown = false;
 	}

	IEnumerator JumpCoolDown (){
		jumpOnCooldown = true;
		yield return new WaitForSeconds(0.01f);
		jumpOnCooldown = false;
	}



	// private void Flip()
	// {
	// 	// Switch the way the player is labelled as facing.
	// 	m_FacingRight = !m_FacingRight;

	// 	// Multiply the player's x local scale by -1.
	// 	Vector3 theScale = transform.localScale;
	// 	theScale.x *= -1;
	// 	transform.localScale = theScale;
	// }
}