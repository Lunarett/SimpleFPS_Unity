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

	[Header("Regeneration Properties")]
	[SerializeField] private bool m_regenerate;
	[SerializeField] private float m_regenerationSpeed = 5.0f;

	private float m_currentHealth;
	private bool m_dead = false;

	public event Action<GameObject, GameObject> OnDeath;
	public event Action OnHealthChanged;

	public float CurrentHealth { get => m_currentHealth; }
	public float MaxHealth { get => m_maxHealth; }
	public ETeams Team { get => m_team; }
	public bool IsDead { get => m_dead; }

	private void Start()
	{
		m_currentHealth = m_maxHealth;

		if (m_HUD != null)
		{
			m_HUD.UpdateHealthBar(m_currentHealth, m_maxHealth);
		}
	}

	private void Update()
	{
		if (m_regenerate)
		{
			Heal(m_regenerationSpeed * Time.deltaTime);
		}
	}

	public void Damage(float damageAmount, GameObject instigator)
	{
		if (IsFriendly(instigator)) { return; }

		if (m_currentHealth <= 0)
		{
			if (!m_dead)
			{
				m_dead = true;
				OnDeath?.Invoke(gameObject, instigator);
			}
		}
		else
		{
			m_currentHealth = Mathf.Clamp(m_currentHealth -= damageAmount, 0, m_maxHealth);

			OnHealthChanged?.Invoke();
		}

		if (m_HUD != null)
		{
			m_HUD.UpdateHealthBar(m_currentHealth, m_maxHealth);
		}
	}

	public void Heal(float healAmount)
	{
		OnHealthChanged?.Invoke();

		m_currentHealth = Mathf.Clamp(m_currentHealth += healAmount, 0, m_maxHealth);
	}

	private bool IsFriendly(GameObject instigator)
	{
		// If either are null assume they are friendly
		if (instigator == null)
		{
			return true;
		}

		Health instigatorHealth = instigator.GetComponent<Health>();

		if (instigatorHealth != null)
		{
			return (int)instigatorHealth.Team == (int)m_team ? true : false;
		}
		else
		{
			Debug.LogWarning("Instigators Health is returning null!");
			return true;
		}
	}

	public void SetTeam(ETeams team)
	{
		m_team = team;
	}
}
