﻿using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CapsuleCollider2D), typeof(BoxCollider2D))]
public class CharacterController2D : MonoBehaviour
{
	private float m_JumpForce = 150f;                          // Amount of force added when the player jumps.
	private float m_CrouchSpeed = 0;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
	private float m_MovementSmoothing = .01f;  // How much to smooth out the movement
	private bool m_AirControl = true;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private LayerMask _whatIsTPWall;
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
																				//[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
	[SerializeField] private Transform _tpRaycaster;
	[SerializeField] public GameObject Shield;
	private CapsuleCollider2D _defaultCollider;
	private BoxCollider2D _whenUseShieldCollider;


	const float k_GroundedRadius = .01f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
										//const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
	const float _tpDistance = 0.64f;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;


	private void Start()
	{
		_defaultCollider = GetComponent<CapsuleCollider2D>();
		_whenUseShieldCollider = GetComponent<BoxCollider2D>();
		Shield.SetActive(false);
	}

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();


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
			}
		}
	}


	private void TryToDash()
	{
		RaycastHit2D hit = Physics2D.Raycast(_tpRaycaster.transform.position, transform.TransformDirection(Vector2.right), _tpDistance, _whatIsTPWall);

		if (hit.collider != null)
		{
			float direction = transform.TransformDirection(Vector3.right).x;
			transform.position = new Vector2(hit.transform.position.x + direction * 0.32f, transform.position.y);
		}

	}

	private void UseShield(bool useShield)
	{
		if (useShield)
		{
			if (!Shield.activeInHierarchy)
			{
				_whenUseShieldCollider.enabled = true;
				_defaultCollider.enabled = false;
				Shield.SetActive(true);
			}
		}
		else
		{
			if (Shield.activeInHierarchy)
			{
				_defaultCollider.enabled = true;
				_whenUseShieldCollider.enabled = false;
				Shield.SetActive(false);
			}
		}

	}


	public void Move(float move, bool crouch, bool jump, bool dash, bool useShield)
	{
		/////////////////////////fix it(bad work with ladder trigger, may can not to use it)///////////////////////
		// If crouching, check to see if the character can stand up
		//if (!crouch)
		//{
		//	// If the character has a ceiling preventing them from standing up, keep them crouching
		//	if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
		//	{
		//		crouch = true;
		//	}
		//}

		UseShield(useShield);
		if (useShield)
		{
			move = 0.0f;
			dash = false;
		}

		if (dash)
		{
			TryToDash();
		}





		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			// If crouching
			if (crouch)
			{
				//Invoke Crouch Event
				//if (!m_wasCrouching)
				//{
				//	m_wasCrouching = true;
				//	OnCrouchEvent.Invoke(true);
				//}

				// Reduce the speed by the crouchSpeed multiplier
				//move *= m_CrouchSpeed;

				// Change size of collider, if it hasn`t changed
				//if(!IsChanged)
				//	change
			}
			else
			{
				// Unchange size of collider, if it has changed
				//if(IsChanged)
				//	change

				//crouch unInvoke Event
				//if (m_wasCrouching)
				//{
				//	m_wasCrouching = false;
				//	OnCrouchEvent.Invoke(false);
				//}
			}

			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				Flip();
			}
		}
		// If the player should jump...
		if (m_Grounded && jump)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		//Vector3 theScale = transform.localScale;
		//theScale.x *= -1;
		//transform.localScale = theScale;


		transform.Rotate(0f, 180f, 0f);
	}
}
