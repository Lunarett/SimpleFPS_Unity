using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
	[Header("Camera Properties")]
	[SerializeField] private Transform m_camera;
	[SerializeField] private Transform m_body;
	[Space]
	[Range(0, 3)]
	[SerializeField] private float m_sensitivityX = 0.05f;
	[Range(0, 3)]
	[SerializeField] private float m_sensitivityY = 0.05f;
	[SerializeField] private Vector2 m_pitchClamp = new Vector2(-60.0f, 60.0f);
	[SerializeField] private bool m_invertPitch = false;
	[SerializeField] private bool m_invertYAW = false;

	[Header("Smooth Properties")]
	[SerializeField] private bool m_smooth = true;
	[SerializeField] private float m_smoothAmount = 25.0f;

	[Header("Sway")]
	[SerializeField] private float m_swayAmount = 2.5f;
	[SerializeField] private float m_swayThreshold = -5.0f;
	[SerializeField] private float m_swaySmooth = 5.0f;

	private CharacterBehavior m_characterBehavior;
	private Rigidbody m_rigidbody;
	private Vector2 m_inputMouseAxis;
	private Vector2 m_sensitivity;

	private Quaternion m_cameraRotation;
	private Quaternion m_characterRotation;
	private Quaternion m_characterLocalRotation;

	private float m_yaw = 0.0f;
	private float m_pitch = 0.0f;

	private void Awake()
	{
		m_characterBehavior = GetComponent<CharacterBehavior>();
		m_rigidbody = GetComponent<Rigidbody>();

		m_sensitivity = new Vector2(m_sensitivityX, m_sensitivityY);
	}

	private void Start()
	{
		// Null Checks
		//if (m_characterBehavior == null) Debug.LogError("Character Behavior is returning Null");
		//if (m_Rigidbody == null) Debug.LogError("RigidBody is returning Null");

		MyDebug<CharacterBehavior>.NullCheck(m_characterBehavior);
		MyDebug<Rigidbody>.NullCheck(m_rigidbody);

		m_cameraRotation = m_camera.rotation;
		m_characterRotation = m_body.rotation;
		m_characterLocalRotation = m_body.localRotation;
	}

	private void LateUpdate()
	{
		if (m_characterBehavior == null) return;
		if (m_rigidbody == null) return;

		Look();
	}

	private void Look()
	{
		Vector2 mouseInput = m_inputMouseAxis * m_sensitivity;

		Debug.Log(mouseInput.y);

		Quaternion rotationYAW = m_invertYAW ? Quaternion.Euler(0.0f, -mouseInput.x, 0.0f) : Quaternion.Euler(0.0f, mouseInput.x, 0.0f);
		Quaternion rotationPitch = m_invertPitch ? Quaternion.Euler(mouseInput.y, 0.0f, 0.0f) : Quaternion.Euler(-mouseInput.y, 0.0f, 0.0f);
		
		m_cameraRotation *= rotationPitch;
		m_characterRotation *= rotationYAW;

		Quaternion localRotation = m_body.localRotation;

		if (m_smooth)
		{
			localRotation = Quaternion.Slerp(localRotation, m_cameraRotation, Time.deltaTime * m_smoothAmount);
			m_rigidbody.MoveRotation(Quaternion.Slerp(m_rigidbody.rotation, m_characterRotation, Time.deltaTime * m_smoothAmount));
		}
		else
		{
			localRotation *= rotationPitch;
			m_rigidbody.MoveRotation(m_rigidbody.rotation * rotationYAW);
		}

		localRotation = RotationClamp(localRotation);
		m_body.localRotation = localRotation;
	}

	// https://answers.unity.com/questions/1609035/how-to-clamp-rotation-following-unitys-quaternion.html
	private Quaternion RotationClamp(Quaternion q)
	{
		float pitchMin = m_pitchClamp.x;
		float pitchMax = m_pitchClamp.y;

		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1.0f;

		float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
		angleX = Mathf.Clamp(angleX, pitchMin, pitchMax);
		q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

		return q;
	}

	public void OnLook(InputAction.CallbackContext context)
	{
		m_inputMouseAxis = !m_characterBehavior.IsPaused() ? context.ReadValue<Vector2>() : Vector2.zero;
	}
}
