using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : MonoBehaviour
{
	[SerializeField] private float m_duration = 1.0f;

	private void Start()
	{
		StartCoroutine(DelayDestroy());
	}

	private void OnCollisionEnter(Collision other)
	{
		Destroy(gameObject);
	}

	IEnumerator DelayDestroy()
	{
		yield return new WaitForSeconds(m_duration);
		Destroy(gameObject);
	}
}
