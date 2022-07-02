using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : ProjectileBehavior
{
	[SerializeField] private float m_duration = 1.0f;
	[SerializeField] private float m_damage = 10;
	[SerializeField] private LayerMask m_damageableMask;
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
		if (m_damageableMask.value == 1 << other.gameObject.layer)
		{
			Health h = other.gameObject.GetComponent<Health>();

			if (h != null)
			{
				h.Damage(m_damage, OwnerObject);
				Instantiate(m_impactEffects[1].Prefab, transform.position, transform.rotation);
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

	// 	private void Dump()
	// 	{
	// 		if (m_damageableMask.value == 1 << other.gameObject.layer)
	// 		{
	// 			HitBox hb = other.gameObject.GetComponent<HitBox>();
	// 			if (hb != null)
	// 			{
	// 				Debug.Log("Dunno what you talking about everything works here :/");
	// 				if (other.collider.CompareTag("Head"))
	// 					hb.OnBulletHit(100, m_ownerObject);
	// 				else
	// 					hb.OnBulletHit(m_damage, m_ownerObject);
	// 
	// 				Instantiate(m_impactEffects[1].Prefab, transform.position, transform.rotation);
	// 			}
	// 			else
	// 			{
	// 				Debug.LogWarning($"No Hitbox Found on Pawn!           Pawn Name: {other.gameObject.name}");
	// 			}
	// 		}
	// 		else
	// 		{
	// 			Instantiate(m_impactEffects[0].Prefab, transform.position, transform.rotation);
	// 		}
	// 
	// 	}
}
