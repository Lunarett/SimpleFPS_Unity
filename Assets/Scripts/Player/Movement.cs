using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

public class Movement : MovementBehavior
{
	[Header("Speed Parameters")]
	[SerializeField] private float m_walkSpeed = 5.0f;
	[SerializeField] private float m_sprintSpeed = 9.0f;
	[SerializeField] private float m_jumpForce = 10.0f;
	[SerializeField] private float m_jumpRateCoolDown = 300.0f;
	[SerializeField] private bool m_midAirControl = false;
	[SerializeField] private bool m_toggleSprint = false;

	[Header("Smooth Movement")]
	[SerializeField] private bool m_smoothMovement = true;
	[SerializeField] private float m_smoothAmount = 0.125f;

	[Header("Speed Multipliers")]
	[SerializeField] private float m_forwardMultiplier = 1.0f;
	[SerializeField] private float m_backwardsMultiplier = 0.8f;
	[SerializeField] private float m_sidewaysMultiplier = 0.8f;

	private readonly RaycastHit[] m_groundHits = new RaycastHit[8];

	private CharacterBehavior m_characterBehavior;
	private CapsuleCollider m_capsuleCollider;
	private Rigidbody m_rigidBody;
	private Vector2 m_inputMovementAxis;
	private Vector3 m_moveDirection;
	private Vector3 m_velocity;
	private Vector3 m_smoothValue;
	private Vector3 m_currentVelocity;
	private float lastJumpTime;
	private bool m_isSprinting = false;
	private bool m_isGrounded;

	public override bool IsSprinting() => m_isSprinting;
	public override bool IsGrounded() => m_isGrounded;


	protected override void Awake()
	{
		m_rigidBody = GetComponent<Rigidbody>();
		m_capsuleCollider = GetComponent<CapsuleCollider>();
		m_characterBehavior = GetComponent<CharacterBehavior>();
	}

	protected override void Start()
	{
		if (m_characterBehavior == null) Debug.LogError("Character Behavior is returning Null!!");
		if (m_rigidBody == null) Debug.LogError("RigidBody is returning Null!!");
		if (m_capsuleCollider == null) Debug.LogError("Capsule Collider is returning Null!!");

		m_rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
	}

	protected override void Update()
	{
		if (m_rigidBody == null) return;
		if (m_capsuleCollider == null) return;

		Move();
		m_isGrounded = false;
	}

	// Ground Check
	private void OnCollisionStay()
	{
		Bounds bounds = m_capsuleCollider.bounds;
		Vector3 extents = bounds.extents;
		float radius = extents.x - 0.01f;

		Physics.SphereCastNonAlloc(bounds.center, radius, Vector3.down,
			m_groundHits, extents.y - radius * 0.5f, ~0, QueryTriggerInteraction.Ignore);

		if (!m_groundHits.Any(hit => hit.collider != null && hit.collider != m_capsuleCollider))
			return;

		for (var i = 0; i < m_groundHits.Length; i++)
			m_groundHits[i] = new RaycastHit();

		m_isGrounded = true;
	}

	protected override void Move()
	{
		if (m_isGrounded || m_midAirControl)
		{
			m_moveDirection = new Vector3(m_inputMovementAxis.x, 0.0f, m_inputMovementAxis.y);

			// Speed Multipliers
			m_moveDirection *= m_isSprinting ? m_sprintSpeed : m_walkSpeed;
			m_moveDirection.z *= (m_inputMovementAxis.y > 0) ? m_forwardMultiplier : m_backwardsMultiplier;
			m_moveDirection.x *= m_sidewaysMultiplier;

			m_moveDirection = transform.right * m_moveDirection.x + transform.forward * m_moveDirection.z;
		}

		m_moveDirection = new Vector3(m_moveDirection.x, m_rigidBody.velocity.y, m_moveDirection.z);
		m_rigidBody.velocity = m_smoothMovement ? SmoothMovement(m_moveDirection, m_smoothAmount) : m_moveDirection;
	}

	private Vector3 SmoothMovement(Vector3 target, float amount)
	{
		m_smoothValue = Vector3.SmoothDamp(m_smoothValue, target, ref m_currentVelocity, amount);
		m_smoothValue = new Vector3(m_smoothValue.x, target.y, m_smoothValue.z);
		return m_smoothValue;
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		m_inputMovementAxis = !m_characterBehavior.IsPaused() ? context.ReadValue<Vector2>() : Vector2.zero;
	}

	public void OnSprint(InputAction.CallbackContext context)
	{
		if (m_moveDirection == Vector3.zero)
		{
			m_isSprinting = false;
			return;
		}

		if (m_toggleSprint)
		{
			if (context.phase == InputActionPhase.Performed) m_isSprinting = !m_isSprinting;
		}
		else
		{
			switch (context.phase)
			{
				case InputActionPhase.Started:
					m_isSprinting = true;
					break;
				case InputActionPhase.Canceled:
					m_isSprinting = false;
					break;
			}
		}
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		switch (context.phase)
		{
			case InputActionPhase.Performed:
				if (m_isGrounded) m_rigidBody.velocity += transform.up * m_jumpForce;
				break;
		}
	}
}
