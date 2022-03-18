using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private TrailRenderer _trailRenderer;
	private bool m_Grounded;            // Whether or not the player is grounded.
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	private bool canDoubleJump;
	private bool hasDoubleJumped = false;

	public Animator animator;

	[Header("Toggles")]
	public bool doubleJumpEnabled = true;
	public bool dashEnabled = true;

	[Header("Dashing")]
	[SerializeField] private float _dashingVelocity = 14f;
	[SerializeField] private float _dashingTime = 0.5f;
	[SerializeField] private float _dashingFloatiness = 7.5f;
	private Vector2 _dashingDir;
	private bool _isDashing;
	private bool _canDash = true;
	private Vector2 oldVelocity;

	[Header("Walljumping")]
	[SerializeField] private float wallSlideSpeed = 0;
	[SerializeField] private LayerMask m_WhatIsWall;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_WallCheck;                           // A position marking where to check if the player is grounded.
	const float k_WallRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool isTouchingWall;
	private bool isWallSliding;
	[SerializeField] private float WallJumpForce;
	[SerializeField] private float wallJumpDirection = -1f;
	[SerializeField] private Vector2 wallJumpAngle;

	private bool wallJumping;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Awake()
	{
		canDoubleJump = true;
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		_trailRenderer = GetComponent<TrailRenderer>();
		wallJumpAngle.Normalize();


		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
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
				if (!wasGrounded)
					OnLandEvent.Invoke();
				animator.SetBool("IsJumping", false);
				canDoubleJump = true;
			}
		}

		isTouchingWall = Physics2D.OverlapCircle(m_WallCheck.position, k_WallRadius, m_WhatIsWall);

		if(isTouchingWall && !m_Grounded && m_Rigidbody2D.velocity.y < 0)
        {
			isWallSliding = true;
        }
        else
        {
			isWallSliding = false;
        }

        if (isWallSliding)
        {
			m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, Mathf.Clamp(m_Rigidbody2D.velocity.y, -wallSlideSpeed, float.MaxValue));
        }
	}


	public void Move(float move, float vertical, bool jump, bool dash, float isMoving)
	{
		if (m_Grounded && isMoving != 0)	// Player is moving
        {
			SoundManager.PlaySound(SoundManager.Sound.PlayerMoveGrass);
		}
		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}

		animator.SetFloat("Speed", Mathf.Abs(isMoving)); // ilde and walk animation

		// If the player should jump...
		if (m_Grounded && jump)
		{
			SoundManager.PlaySound(SoundManager.Sound.PlayerJump);

			m_Grounded = false;

			// Add a vertical force to the player.
			m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce / 50);
			StartCoroutine(StartJumpAnimation());
		}
		else if (jump && canDoubleJump && doubleJumpEnabled && !isTouchingWall)
		{
			SoundManager.PlaySound(SoundManager.Sound.PlayerJump);

			canDoubleJump = false;
			hasDoubleJumped = true;

			// Add a vertical force to the player.
			m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce / 50);
			StartCoroutine(StartJumpAnimation());
		}

		if (dash && _canDash && dashEnabled)
		{
			dash = false;
			SoundManager.PlaySound(SoundManager.Sound.PlayerDash);

			oldVelocity = m_Rigidbody2D.velocity;
			_isDashing = true;
			_canDash = false;
			_trailRenderer.emitting = true;
			//_dashingDir = new Vector2(move, Input.GetAxisRaw("Vertical"));
			_dashingDir = new Vector2(move, vertical);
			if (_dashingDir == Vector2.zero)
			{
				_dashingDir = new Vector2(transform.localScale.x, 0);
			}
			StartCoroutine(StopDashing(_dashingDir));
		}
		if (_isDashing)
		{
			canDoubleJump = false;
			m_Rigidbody2D.velocity = _dashingDir.normalized * _dashingVelocity;
		}
		if (m_Grounded && !_isDashing)
		{
			_canDash = true;
			hasDoubleJumped = false;
		}

		if (isWallSliding && jump)
		{
			m_Rigidbody2D.AddForce(new Vector2(WallJumpForce * wallJumpDirection * wallJumpAngle.x, WallJumpForce * wallJumpAngle.y), ForceMode2D.Impulse);
		}
	}

	private IEnumerator StopDashing(Vector2 dashingDir)
    {
		yield return new WaitForSeconds(_dashingTime);
		dashingDir.Normalize();
		//This block of code adds velocity to the character after a dash to gove them some hang time in the air.
		if (dashingDir.y > 0)
		{
			if (dashingDir.x != 0)
			{
				m_Rigidbody2D.velocity = new Vector2(oldVelocity.x, _dashingFloatiness);
			}
			else
            {
				m_Rigidbody2D.velocity = new Vector2(0, _dashingFloatiness);
			}
		}
		else if (dashingDir.y == 0)
        {
			m_Rigidbody2D.velocity = new Vector2(oldVelocity.x + _dashingFloatiness, 0);
		}

        if (!hasDoubleJumped)
        {
			canDoubleJump = true;
        }
		_trailRenderer.emitting = false;
		_isDashing = false;
    }

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		wallJumpDirection *= -1;
	}

	private IEnumerator StartJumpAnimation()
	{
		yield return new WaitForSeconds(0.5f);

		animator.SetBool("IsJumping", true);
	}
}