using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
	[SerializeField] private float health = 100;

	EnemyAgent enemyAgent;
	float currentHealth;

	public event Action OnDeath;
	public event Action OnHealthChanged;

	public float CurrentHealth { get => currentHealth; }

	private void Awake()
	{
		var rigidBodies = GetComponentsInChildren<Rigidbody>();
		enemyAgent = GetComponent<EnemyAgent>();

		foreach (var rb in rigidBodies)
		{
			HitBox hb = rb.gameObject.AddComponent<HitBox>();
			hb.health = this;
		}
	}

	private void Start()
	{
		currentHealth = health;
	}

	public void Damage(float damageAmount)
	{
		currentHealth = Mathf.Clamp(currentHealth -= damageAmount, 0, health);
		OnHealthChanged?.Invoke();

		if (currentHealth <= 0) Death();
	}

	private void Death()
	{
		OnDeath?.Invoke();
	}
}
