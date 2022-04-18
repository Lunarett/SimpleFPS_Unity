using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
	public Health health;

	public void OnBulletHit(float damageAmount)
	{
		health.Damage(damageAmount);
	}
}
