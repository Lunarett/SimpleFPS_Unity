using System;
using UnityEngine;

public enum ETeams
{
	RedTeam = 0,
	BlueTeam = 1
}

public class Health : MonoBehaviour, IDamageable
{
	[Header("References")]
	[SerializeField] private HUD m_HUD;
	
	[Header("Health Properties")]
	[SerializeField] private float m_maxHealth = 100;
	[SerializeField] private ETeams m_team;

	private float m_currentHealth;

	public event Action OnDeath;
	public event Action OnHealthChanged;

	public float CurrentHealth { get => m_currentHealth; }
	public float MaxHealth { get => m_maxHealth; }
	public ETeams Team { get => m_team; }

	private void Start()
	{
		m_currentHealth = m_maxHealth;

		if(m_HUD != null)
		{
			m_HUD.UpdateHealthBar(m_currentHealth, m_maxHealth);
		}
	}

	public void Damage(float damageAmount, GameObject instigator)
	{
		if(IsFriendly(instigator)) { return; }

		OnHealthChanged?.Invoke();
		
		m_currentHealth = Mathf.Clamp(m_currentHealth -= damageAmount, 0, m_maxHealth);

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

	private bool IsFriendly(GameObject instigator)
	{
		// If either are null assume they are friendly
		if (instigator == null)
		{
			Debug.LogWarning("Instigators is Null");
			return true;
		}

		Health instigatorHealth = instigator.GetComponent<Health>();

		if(instigatorHealth != null)
		{
			Debug.Log(gameObject.name);
			return (int)instigatorHealth.Team == (int)m_team ? true : false;
		}
		else
		{
			Debug.LogWarning("Instigators Health is returning null!");
			return true;
		}
	}
}
