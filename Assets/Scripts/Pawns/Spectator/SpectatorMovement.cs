using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpectatorMovement : MonoBehaviour
{
	[SerializeField] private float m_movementSpeed = 50.0f;
	[SerializeField] private Transform m_cameraTransform;
	
	private Rigidbody m_rigidbody;
	private Vector2 m_inputMovementAxis;
	private float m_verticalAxis;

	private void Awake()
	{
		m_rigidbody = GetComponent<Rigidbody>();
	}

	private void Start()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void Update()
	{
		Vector3 input = new Vector3(m_inputMovementAxis.x, m_verticalAxis, m_inputMovementAxis.y);
		Vector3 direction = transform.right * input.x + transform.up * input.y + m_cameraTransform.forward * input.z;

		m_rigidbody.MovePosition(transform.position + direction * m_movementSpeed * Time.deltaTime);
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		m_inputMovementAxis = context.ReadValue<Vector2>();
	}

	public void OnVerticalMovement(InputAction.CallbackContext context)
	{
		m_verticalAxis = context.ReadValue<float>();
	}
}
