using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DEPRECATED!! DO NOT USE THIS SCRIPT

public class Spawner : MonoBehaviour
{
	[SerializeField] ETeams m_team;
	[SerializeField] private bool m_isPlayer;
	[SerializeField] private GameObject m_player;
	[SerializeField] private GameObject m_bot;

	private GameObject m_pawn;
	private Health m_health;

	private void Start()
	{

	}

	private void Spawn(GameObject pawnPrefab)
	{
		m_pawn = Instantiate(pawnPrefab, transform.position, transform.rotation);

		m_health = m_pawn.GetComponent<Health>();

		if (m_health != null)
		{
			m_health.SetTeam(m_team);
			m_health.OnDeath += Respawn;
		}
	}

	private void Respawn(GameObject killedPawn, GameObject killerPawn)
	{
		StartCoroutine(Delay());
	}

	private IEnumerator Delay()
	{
		Destroy(m_pawn);
		m_health = null;

		yield return new WaitForSeconds(0);

		if (m_isPlayer)
			Spawn(m_player);
		else
			Spawn(m_bot);
	}


}
