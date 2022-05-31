using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSpawner : MonoBehaviour
{
	[SerializeField] private GameObject enemyPrefab;

	private Health h;

	private void Start()
	{
		SpawnEnemyPrefab();
	}

	private void LateUpdate()
	{
		if(h.CurrentHealth <= 0)
			SpawnEnemyPrefab();
	}

	private void SpawnEnemyPrefab()
	{
		GameObject go = Instantiate(enemyPrefab, transform.localPosition, transform.localRotation);
		h = go.GetComponent<Health>();
	}
}
