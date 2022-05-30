using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : MonoBehaviour
{
	[SerializeField] private float m_duration = 1.0f;
	[SerializeField] private ImpactEffects[] m_impactEffects;

	private void Start()
	{
		StartCoroutine(DelayDestroy());
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Enemy"))
		{
			Debug.LogWarning("Is Detecting enemy");
			HitBox hb = other.gameObject.GetComponent<HitBox>();
			if (hb != null)
			{
				if (other.collider.CompareTag("Head"))
					hb.OnBulletHit(100);
				else
					hb.OnBulletHit(10);

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
}
