using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
	private Health m_health;

	public Health Health { get => m_health; set { m_health = value; } }

	public void OnBulletHit(float damageAmount, GameObject instigator)
	{
		if(instigator == null) { return; }

		m_health.Damage(damageAmount, instigator);
	}
}
