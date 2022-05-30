using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelProgressBar : MonoBehaviour
{
	[SerializeField] private MovementBehavior m_movement;

	private Slider m_slider;

	private void Awake()
	{
		m_slider = GetComponent<Slider>();
	}

	private void LateUpdate()
	{
		if (m_movement.GetCurrentFuel() != m_movement.GetMaxFuel())
			m_slider.value = m_movement.GetCurrentFuel() / 100;
	}
}
