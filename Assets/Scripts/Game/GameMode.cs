using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
	[SerializeField] private GameObject m_playerPrefab;
	[SerializeField] private Transform[] m_spawnPoints;

	private Health m_health;
	private GameObject m_playerObject;

	private void Start()
	{
		SpawnPlayer();
	}

	private void OnDeath()
	{
		Destroy(m_playerObject);
		SpawnPlayer();
	}

	private void SpawnPlayer()
	{
		m_playerObject = Instantiate(m_playerPrefab, m_spawnPoints[Random.Range(0, m_spawnPoints.Length)].position, m_spawnPoints[Random.Range(0, m_spawnPoints.Length)].rotation);

		m_health = m_playerObject.GetComponentInChildren<Health>();

		if (m_health != null)
		{
			m_health.OnDeath += OnDeath;
		}
	}
}
