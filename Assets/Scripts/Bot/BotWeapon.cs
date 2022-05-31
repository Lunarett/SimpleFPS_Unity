using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotWeapon : WeaponBehavior
{
	[Header("Weapon Properties")]
	[SerializeField] private string m_weaponName;
	[SerializeField] private bool m_autoReload = true;
	[SerializeField] private LayerMask m_rayLayerMask;
	[Space]
	[SerializeField] private Transform m_fireTransform;
	[SerializeField] private int m_shotCount = 1;
	[SerializeField] private int m_magazineSize = 30;
	[SerializeField] private float m_shotImpulse = 400.0f;
	[SerializeField] private float m_FireRate = 600.0f;
	[SerializeField] private float m_spread = 0.1f;
	[SerializeField] private float m_fireDistance = 500.0f;
	[SerializeField] private float m_reloadDuration = 0.0f;

	[Header("Effects")]
	[SerializeField] private GameObject m_bulletPrefab;
	[SerializeField] private ParticleSystem[] m_muzzleEffects;

	[Header("Cheats")]
	[SerializeField] private bool m_infiniteAmmo = false;

	private GameObject m_ownerObject;
	private int m_currentAmmo = 0;
	private float m_lastFireTime = 0.0f;

	private bool m_isFiring = false;
	private bool m_canFire = true;
	private bool m_isReloading = false;

	public override GameObject GetOwnerObject() => m_ownerObject;
	public override int GetCurrentAmmo() => m_currentAmmo;
	public override int GetMagazineSize() => m_magazineSize;
	public override string GetWeaponName() => m_weaponName;
	public override bool GetIsFiring() => m_isFiring;
	public override bool GetIsReloading() => m_isReloading;
	public override Transform GetFireTransform() { return m_fireTransform; }

	public override void SetOwnerObject(GameObject owner)
	{
		m_ownerObject = owner;
	}

	protected override void Start()
	{
		m_currentAmmo = m_magazineSize;
	}

	protected override void Update()
	{
		if (m_isFiring && m_canFire) { Fire(); }
	}

	public override void StartFire()
	{
		m_isFiring = true;
	}

	public override void StopFire()
	{
		m_isFiring = false;
	}

	protected override void Fire()
	{
		if (Time.time - m_lastFireTime > 60.0f / m_FireRate)
		{
			m_lastFireTime = Time.time;

			if (m_currentAmmo > 0)
			{
				Vector3 spreadValue = Random.insideUnitSphere * (m_spread);
				spreadValue.z = 0;
				spreadValue = m_fireTransform.TransformDirection(spreadValue);

				PlayMuzzleParticles();
				SpawnProjectile(m_fireTransform.rotation, spreadValue);
			}
			else
			{
				Reload();
			}
		}
	}

	public override void Reload()
	{
		StartCoroutine(ReloadDelay());
	}

	IEnumerator ReloadDelay()
	{
		m_canFire = false;
		m_isReloading = true;

		yield return new WaitForSeconds(m_reloadDuration);
		m_currentAmmo = m_magazineSize;

		m_isReloading = false;
		m_canFire = true;
	}

	private void SpawnProjectile(Quaternion rotation, Vector3 spread)
	{
		GameObject projectile = Instantiate(m_bulletPrefab, m_fireTransform.position, rotation);
		projectile.GetComponent<Projectile>().OwnerObject = m_ownerObject;
		projectile.transform.SetParent(null);
		projectile.GetComponent<Rigidbody>().velocity = (m_fireTransform.forward + spread) * m_shotImpulse;
	}

	private void PlayMuzzleParticles()
	{
		foreach (ParticleSystem p in m_muzzleEffects)
		{
			p.Play();
		}
	}
}