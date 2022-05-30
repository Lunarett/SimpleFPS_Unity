using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ImpactEffects
{
	[SerializeField] private string m_name;
	[SerializeField] private GameObject m_impactParticlePrefab;
	[SerializeField] private LayerMask m_impactLayerMask;

	public GameObject Prefab { get => m_impactParticlePrefab; }
	public LayerMask Layer { get => m_impactLayerMask; }
	public string Name { get => m_name; }
}

public class Weapon : WeaponBehavior
{
	[Header("Weapon Properties")]
	[SerializeField] private string m_weaponName;
	[SerializeField] private bool m_autoReload = true;
	[SerializeField] private bool m_raycast = true;
	[SerializeField] private LayerMask m_rayLayerMask;
	[Space]
	[SerializeField] private Transform m_fireLocation;
	[SerializeField] private int m_shotCount = 1;
	[SerializeField] private int m_magazineSize = 30;
	[SerializeField] private float m_shotImpulse = 400.0f;
	[SerializeField] private float m_FireRate = 600.0f;
	[SerializeField] private int m_damage = 10;
	[SerializeField] private float m_spread = 0.1f;
	[SerializeField] private float m_fireDistance = 500.0f;

	[Header("Effects")]
	[SerializeField] private GameObject m_bulletPrefab;
	[SerializeField] private ImpactEffects[] m_impactEffects;
	[SerializeField] private ParticleSystem[] m_muzzleEffects;

	[Header("Cheats")]
	[SerializeField] private bool m_infiniteAmmo = false;

	private int m_currentAmmo = 0;
	private float m_lastFireTime = 0.0f;

	public override int GetCurrentAmmo() => m_currentAmmo;
	public override int GetMagazineSize() => m_magazineSize;
	public override string GetWeaponName() => m_weaponName;

	protected override void Start()
	{
		m_currentAmmo = m_magazineSize;
	}

	public override void Fire()
	{
		if (Time.time - m_lastFireTime > 60.0f / m_FireRate)
		{
			m_lastFireTime = Time.time;

			if (m_currentAmmo > 0)
			{
				m_currentAmmo = m_infiniteAmmo ? m_magazineSize : m_currentAmmo - 1;

				PlayMuzzleParticles();

				Vector3 spreadValue = Random.insideUnitSphere * (m_spread);
				spreadValue.z = 0;
				spreadValue = m_fireLocation.TransformDirection(spreadValue);

				Quaternion rotation = Quaternion.LookRotation(Camera.main.transform.forward * 1000.0f + spreadValue - m_fireLocation.position);

				if (Physics.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward),
					out RaycastHit hit, m_fireDistance, m_rayLayerMask))
					rotation = Quaternion.LookRotation(hit.point + spreadValue - m_fireLocation.position);


				GameObject projectile = Instantiate(m_bulletPrefab, m_fireLocation.position, rotation);
				projectile.transform.SetParent(null);
				projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * m_shotImpulse;

				RaycastHit fireHit;
				Vector3 shootDirection = new Vector3(Camera.main.transform.forward.x + Random.Range(-m_spread, m_spread), Camera.main.transform.forward.y + Random.Range(-m_spread, m_spread), Camera.main.transform.forward.z);
				bool raycast = Physics.Raycast(new Ray(m_fireLocation.position, Camera.main.transform.forward + spreadValue), out fireHit, m_fireDistance, m_rayLayerMask);

				if (raycast)
				{
					PlayImpactParticles(fireHit.collider.gameObject.layer, fireHit.point, Quaternion.LookRotation(fireHit.normal));

// 					// Deal Damage
// 					HitBox hb = fireHit.collider.gameObject.GetComponent<HitBox>();
// 					if (hb != null)
// 					{
// 						if (fireHit.collider.CompareTag("Head"))
// 							hb.OnBulletHit(100);
// 						else
// 							hb.OnBulletHit(m_damage);
// 					}
				}
			}
			else
			{
				Reload();
			}
		}
	}

	private void PlayMuzzleParticles()
	{
		foreach (ParticleSystem p in m_muzzleEffects)
		{
			p.Play();
		}
	}

	private void PlayImpactParticles(LayerMask layer, Vector3 position, Quaternion rotation)
	{
		for (int i = 0; i < m_impactEffects.Length; i++)
		{
			if (m_impactEffects[i].Layer == layer)
			{
				GameObject go = Instantiate(m_impactEffects[i].Prefab, position, rotation);
				go.transform.SetParent(null);

				ParticleSystem impactParticle = go.GetComponent<ParticleSystem>();

				if (impactParticle != null)
				{
					impactParticle.Play();
				}
				else
				{
					Debug.LogError($"Could not find particle system component in prefab (Effect = {m_impactEffects[i].Name})");
				}
				break;
			}
		}
	}

	public override void Reload()
	{
		m_currentAmmo = m_magazineSize;
	}

}
