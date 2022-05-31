using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
	[SerializeField] private Rigidbody m_weapon;

	private Rigidbody[] m_rigidBodies;
	private Animator m_anim;
	private Health m_health;

	public Rigidbody[] Rigidbodies { get => m_rigidBodies; }

	private void Awake()
	{
		m_rigidBodies = GetComponentsInChildren<Rigidbody>();
		m_anim = GetComponent<Animator>();
		m_health = GetComponent<Health>();

		if(m_health != null && m_rigidBodies != null)
		{
			foreach (var rb in m_rigidBodies)
			{
				HitBox hb = rb.gameObject.AddComponent<HitBox>();
				hb.Health = m_health;
			}
		}
	}

	private void Start()
	{
		DeactivateRagdoll();
	}

	public void DeactivateRagdoll()
	{
		foreach (Rigidbody rb in m_rigidBodies)
		{
			rb.isKinematic = true;
			rb.gameObject.layer = 0;
		}
		
		m_anim.enabled = true;
		m_weapon.isKinematic = true;
	}

	public void ActivateRagdoll()
	{
		foreach (Rigidbody rb in m_rigidBodies)
		{
			rb.isKinematic = false;
			rb.gameObject.layer = 10;
		}

		m_anim.enabled = false;
		m_weapon.gameObject.SetActive(false);
	}
}
