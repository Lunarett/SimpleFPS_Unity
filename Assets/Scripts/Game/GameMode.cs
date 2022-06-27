using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAISpawnState
{
	DontSpawn,
	SpawnOnlyOne,
	SpawnWholeTeam
}

public class GameMode : MonoBehaviour
{
	[SerializeField] private GameObject m_playerPrefab;
	[SerializeField] private GameObject m_AIPrefab;
	[SerializeField] private GameObject m_spectatorPrefab;
	[Space]
	[SerializeField] private Transform[] m_redTeamSpawnPoints;
	[SerializeField] private Transform[] m_blueTeamSpawnPoints;

	[Header("Team Flags")]
	[SerializeField] private GameObject m_redTeamFlag;
	[SerializeField] private GameObject m_blueTeamFlag;

	[Header("Respawn Properties")]
	[SerializeField] private float m_respawnDelay = 5.0f;
	[SerializeField] private float m_flagReturnDelay = 5.0f;

	[Header("Debug Properties")]
	[SerializeField] private EAISpawnState m_aiSpawnState;
	[SerializeField] private bool m_disablePlayerSpawn;

	private int m_redTeamScore = 0;
	private int m_blueTeamScore = 0;

	private GameObject m_playerObject;

	private void Start()
	{
		if (!m_disablePlayerSpawn) SpawnPlayer(Random.Range(0, m_redTeamSpawnPoints.Length));

		SpawnAI(m_aiSpawnState);
	}

	private void OnPlayerDeath(GameObject killedPawn, GameObject killerPawn)
	{
		PickupFlag flag = m_playerObject.GetComponentInChildren<PickupFlag>();

		if (flag != null)
		{
			flag.DropFlag(killedPawn);
			StartCoroutine(StartFlagResetTimer(m_flagReturnDelay, flag));
		}

		PlayerCharacterBehavior characterBehavior = killedPawn.GetComponent<PlayerCharacterBehavior>();

		if(characterBehavior)
		{
			StartCoroutine(EnterSpectatorMode(m_respawnDelay, characterBehavior.GetCharacterID()));
		}
	}

	IEnumerator StartFlagResetTimer(float seconds, PickupFlag pf)
	{
		yield return new WaitForSeconds(seconds);

		pf.ReturnFlagToBase();
	}

	IEnumerator EnterSpectatorMode(float duration, int id)
	{
		Transform playerTransform = m_playerObject.transform;
		Destroy(m_playerObject);

		GameObject go = Instantiate(m_spectatorPrefab, playerTransform.position, playerTransform.rotation);

		yield return new WaitForSeconds(duration);

		Destroy(go);
		SpawnPlayer(id);
	}

	private void OnAIDeath(GameObject killedPawn, GameObject killerPawn)
	{
		Debug.Log($"AI has died! {killedPawn.name} has killed {killerPawn.name}");
		AIAgentBehavior agentBehavior = killedPawn.GetComponent<AIAgentBehavior>();
		
		if(agentBehavior != null)
		{
			StartCoroutine(RespawnAIDelay(m_respawnDelay, agentBehavior.GetCharacterID()));
		}
	}

	IEnumerator RespawnAIDelay(float duration, int id)
	{
		yield return new WaitForSeconds(duration);

		SpawnAIByIndex(id);
	}

	private void SpawnPlayer(int i)
	{
		m_playerObject = Instantiate(m_playerPrefab, m_redTeamSpawnPoints[i].position, m_redTeamSpawnPoints[i].rotation);

		PlayerCharacterBehavior characterBehavior = m_playerObject.GetComponent<PlayerCharacterBehavior>();

		if(characterBehavior != null)
		{
			characterBehavior.SetCharacterID(i);
		}

		Health playerHealth = m_playerObject.GetComponent<Health>();

		if (playerHealth != null)
		{
			playerHealth.OnDeath += OnPlayerDeath;
		}
	}

	private void SpawnAI(EAISpawnState aiSpawnState)
	{
		switch (aiSpawnState)
		{
			case EAISpawnState.SpawnOnlyOne:
				SpawnAIByIndex(0);
				break;
			case EAISpawnState.SpawnWholeTeam:
				for (int i = 0; i < m_blueTeamSpawnPoints.Length; i++)
				{
					SpawnAIByIndex(i);
				}
				break;
			default:
				break;
		}
	}

	private void SpawnAIByIndex(int i)
	{
		Debug.Log($"spawning AI with ID: {i}");
		GameObject go = Instantiate(m_AIPrefab, m_blueTeamSpawnPoints[i].position, m_blueTeamSpawnPoints[i].rotation);

		AIAgentBehavior agentBehavior = go.GetComponent<AIAgentBehavior>();
		
		if( agentBehavior != null)
		{
			agentBehavior.SetCharacterID(i);
			agentBehavior.SetOpposingTeamFlag(m_redTeamFlag.transform);
			agentBehavior.SetTeamFlag(m_blueTeamFlag.transform);
		}

		Health aiHealth = go.GetComponent<Health>();

		if (aiHealth != null)
		{
			aiHealth.OnDeath += OnAIDeath;
		}
	}

	public void AddTeamScore(ETeams team, int score)
	{

	}
}
