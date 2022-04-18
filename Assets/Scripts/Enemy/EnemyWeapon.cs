using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private Transform m_fireLocation;
	[SerializeField] private GameObject m_projectilePrefab;
	[Space]
	[SerializeField] private ParticleSystem m_muzzleParticle;

	[Header("Weapon Behavior")]
	[SerializeField] private float m_spread;
	[SerializeField] private float m_fireRate = 650;
	[SerializeField] private float m_bulletImpulse = 400;
	[SerializeField] private float m_reloadDelay = 0.25f;

	[Header("Damage Properties")]
	[SerializeField] private float m_damage;

	[Header("Ammo Properties")]
	[SerializeField] int clipAmmo;

	[HideInInspector] public int CurrentAmmo;
	[HideInInspector] public bool StartFire;

	public float Damage { get => m_damage; }
	
	public Transform FireLocation { get => m_fireLocation; }

	private bool canFire = true;
	private float lastFireTime;

	private void Start()
	{
		CurrentAmmo = clipAmmo;
	}

	private void Update()
	{
		if (StartFire)
		{
			Fire();
		}
	}

	public void Fire()
	{
		if (canFire && Time.time - lastFireTime > 60.0f / m_fireRate)
		{
			lastFireTime = Time.time;
			CurrentAmmo--;

			m_muzzleParticle.Play();

			Vector3 spreadValue = Random.insideUnitSphere * (m_spread);
			spreadValue.z = 0;
			spreadValue = m_fireLocation.TransformDirection(spreadValue);

			GameObject projectile = Instantiate(m_projectilePrefab, m_fireLocation.position, m_fireLocation.rotation);
			projectile.GetComponent<Rigidbody>().velocity = (m_fireLocation.forward + spreadValue) * m_bulletImpulse;

			RaycastHit hit;
			bool ray = Physics.Raycast(new Ray(m_fireLocation.position, (m_fireLocation.forward + spreadValue)), out hit, 100);

			if(ray)
			{
				if (hit.collider.CompareTag("Player"))
				{
					Health h = hit.collider.GetComponent<Health>();
					if (h != null)
						h.Damage(m_damage);
				}
			}
		}
		else if (canFire && CurrentAmmo <= 0)
		{
			StartCoroutine(Reload());
		}
	}

	IEnumerator Reload()
	{
		canFire = false;
		yield return new WaitForSeconds(m_reloadDelay);
		CurrentAmmo = clipAmmo;
		canFire = true;
	}
}
