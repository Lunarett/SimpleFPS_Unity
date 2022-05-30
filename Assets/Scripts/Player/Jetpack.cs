using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jetpack : MonoBehaviour
{
	[Header("Fuel Properties")]
	[SerializeField] private float m_totalFuel = 100.0f;
	[SerializeField] private float m_fuelConsumptionAmount = 5.0f;
	[Space]
	[SerializeField] private float m_thrustPower = 5.0f;

	private bool m_isHoldingButton = false;

	private Rigidbody m_Rigidbody;

	private void Awake()
	{
		m_Rigidbody = GetComponent<Rigidbody>();
	}

	private void Update()
	{

	}
}
