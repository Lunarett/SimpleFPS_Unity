using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
	[SerializeField] private Slider m_fuelBar;
	[SerializeField] private Slider m_healthBar;

	public void UpdateFuelBar(float fuel)
	{
		m_fuelBar.value = fuel;
	}

	public void UpdateHealthBar(float health)
	{
		m_healthBar.value = health;
	}
}
