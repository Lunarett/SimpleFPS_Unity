using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
	[SerializeField] private float m_maxHealth = 100;
	[SerializeField] private HUD m_HUD;

	float m_currentHealth;

	public event Action OnDeath;
	public event Action OnHealthChanged;

	public float CurrentHealth { get => m_currentHealth; }

	private void Awake()
	{
		var rigidBodies = GetComponentsInChildren<Rigidbody>();

		foreach (var rb in rigidBodies)
		{
			HitBox hb = rb.gameObject.AddComponent<HitBox>();
			hb.health = this;
		}
	}

	private void Start()
	{
		m_currentHealth = m_maxHealth;

		if(m_HUD != null)
		{
			m_HUD.UpdateHealthBar(m_currentHealth, m_maxHealth);
		}
	}

	public void Damage(float damageAmount)
	{
		m_currentHealth = Mathf.Clamp(m_currentHealth -= damageAmount, 0, m_maxHealth);
		OnHealthChanged?.Invoke();

		if (m_currentHealth <= 0) Death();

		if(m_HUD != null)
		{
			m_HUD.UpdateHealthBar(m_currentHealth, m_maxHealth);
		}
	}

	private void Death()
	{
		OnDeath?.Invoke();
	}
}
