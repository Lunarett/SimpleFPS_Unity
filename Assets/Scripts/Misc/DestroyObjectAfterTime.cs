using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectAfterTime : MonoBehaviour
{
	[SerializeField] private float m_delay;

	private void Start()
	{
		StartCoroutine(DestroyObject(m_delay));
	}

	IEnumerator DestroyObject(float delay)
	{
		yield return new WaitForSeconds(delay);
		Destroy(gameObject);
	}
}
