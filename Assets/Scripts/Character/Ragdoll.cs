using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
	[SerializeField] Rigidbody weapon;

	Rigidbody[] rigidBodies;
	Animator anim;

	public Rigidbody[] Rigidbodies { get => rigidBodies; }

	private void Awake()
	{
		rigidBodies = GetComponentsInChildren<Rigidbody>();
		anim = GetComponent<Animator>();
	}

	private void Start()
	{
		DeactivateRagdoll();
	}

	public void DeactivateRagdoll()
	{
		foreach (Rigidbody rb in rigidBodies)
		{
			rb.isKinematic = true;
			rb.gameObject.layer = 0;
		}
		
		anim.enabled = true;
		weapon.isKinematic = true;
	}

	public void ActivateRagdoll()
	{
		foreach (Rigidbody rb in rigidBodies)
		{
			rb.isKinematic = false;
			rb.gameObject.layer = 10;
		}

		anim.enabled = false;
		weapon.gameObject.SetActive(false);
	}
}
