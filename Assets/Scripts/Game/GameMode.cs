using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
	[SerializeField] private GameObject m_playerPrefab;
	[SerializeField] private GameObject m_botPrefab;
	[Space]
	[SerializeField] private Transform[] m_redTeamSpawnPoints;
	[SerializeField] private Transform[] m_blueTeamSpawnPoints;

	private Health m_health;
	private GameObject m_playerObject;

	private List<GameObject> m_bots = new List<GameObject>();

	private void Start()
	{
		SpawnPlayer();
		SpawnBots();
	}

	private void OnDeath()
	{
		PickupFlag flag = m_playerObject.GetComponentInChildren<PickupFlag>();

		if(flag != null)
		{
			flag.Reset();
		}

		Destroy(m_playerObject);
		SpawnPlayer();
	}

	private void SpawnPlayer()
	{
		m_playerObject = Instantiate(m_playerPrefab, m_redTeamSpawnPoints[Random.Range(0, m_redTeamSpawnPoints.Length)].position, m_redTeamSpawnPoints[Random.Range(0, m_redTeamSpawnPoints.Length)].rotation);

		m_health = m_playerObject.GetComponent<Health>();

		if (m_health != null)
		{
			m_health.OnDeath += OnDeath;
		}
	}

	private void SpawnBots()
	{
		for (int i = 0; i < m_blueTeamSpawnPoints.Length; i++)
		{
			GameObject go = Instantiate(m_botPrefab, m_blueTeamSpawnPoints[i].position, m_blueTeamSpawnPoints[i].rotation);

			m_bots.Add(go);
		}
	}
}
