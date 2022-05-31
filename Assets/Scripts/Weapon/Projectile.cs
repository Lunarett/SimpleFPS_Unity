using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : ProjectileBehavior
{
	[SerializeField] private float m_duration = 1.0f;
	[SerializeField] private float m_damage = 10;
	[SerializeField] private ImpactEffects[] m_impactEffects;

	private GameObject m_ownerObject;

	public GameObject OwnerObject { get => m_ownerObject; set { m_ownerObject = value; } }

	public override float GetDamage() => m_damage;

	protected override void Start()
	{
		StartCoroutine(DelayDestroy());
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Bot"))
		{
			HitBox hb = other.gameObject.GetComponent<HitBox>();
			if (hb != null)
			{
				if (other.collider.CompareTag("Head"))
					hb.OnBulletHit(100, m_ownerObject);
				else
					hb.OnBulletHit(m_damage, m_ownerObject);

				Instantiate(m_impactEffects[1].Prefab, transform.position, transform.rotation);
			}
		}
		else if (other.collider.CompareTag("Player"))
		{
			Health health = other.gameObject.GetComponent<Health>();

			if (health != null)
			{
				health.Damage(m_damage, m_ownerObject);
			}
		}
		else
		{
			Instantiate(m_impactEffects[0].Prefab, transform.position, transform.rotation);
		}

		Destroy(gameObject);
	}

	IEnumerator DelayDestroy()
	{
		yield return new WaitForSeconds(m_duration);
		Destroy(gameObject);
	}

	public override void SetDamage(float damage)
	{
		m_damage = damage;
	}

	public override void SetDamageMultiplier(float multiplier)
	{
		m_damage *= multiplier;
	}
}
