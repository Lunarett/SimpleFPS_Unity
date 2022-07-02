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

	private List<Transform> m_cachedSpawnPoints = new List<Transform>();

	private int m_redTeamScore = 0;
	private int m_blueTeamScore = 0;
	private int m_aiID = 0;

	private GameObject m_playerObject;

	private void Start()
	{
		switch (m_aiSpawnState)
		{
			case EAISpawnState.SpawnOnlyOne:
				if(!m_disablePlayerSpawn) { SpawnPlayer(); }
				SpawnAIByPostion(m_blueTeamSpawnPoints[1], 1, ETeams.BlueTeam);
				break;
			case EAISpawnState.SpawnWholeTeam:
				Spawn();
				break;
			default:
				break;
		}
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

		if (characterBehavior)
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

		m_playerObject = Instantiate(m_playerPrefab, Vector3.zero, Quaternion.identity);

		Health playerHealth = m_playerObject.GetComponent<Health>();

		if (playerHealth != null)
		{
			playerHealth.OnDeath += OnPlayerDeath;
		}
	}

	private void OnAIDeath(GameObject killedPawn, GameObject killerPawn)
	{
		Debug.Log($"AI has died! {killedPawn.name} has killed {killerPawn.name}");
		AIAgentBehavior agentBehavior = killedPawn.GetComponent<AIAgentBehavior>();
		FlagHolder fh = killedPawn.GetComponent<FlagHolder>();

		if(fh.IsHoldingFlag)
		{
			fh.DropFlag();
		}

		if (agentBehavior != null)
		{
			StartCoroutine(RespawnAIDelay(m_cachedSpawnPoints[agentBehavior.GetCharacterID()], m_respawnDelay, agentBehavior.GetCharacterID(), agentBehavior.GetHealth().Team));
		}
	}

	IEnumerator RespawnAIDelay(Transform spawnTransform, float duration, int id, ETeams team)
	{
		yield return new WaitForSeconds(duration);

		SpawnAIByPostion(spawnTransform, id, team);
	}

	private ETeams SpawnPlayer()
	{
		m_playerObject = Instantiate(m_playerPrefab, Vector3.zero, Quaternion.identity);

		PlayerCharacterBehavior characterBehavior = m_playerObject.GetComponent<PlayerCharacterBehavior>();

		if (characterBehavior != null)
		{
			characterBehavior.SetCharacterID(0);
		}

		Health playerHealth = m_playerObject.GetComponent<Health>();

		if (playerHealth != null)
		{
			playerHealth.OnDeath += OnPlayerDeath;
			return playerHealth.Team;
		}

		return ETeams.RedTeam;
	}

	private void Spawn()
	{
		ETeams playerTeam = SpawnPlayer();

		switch (playerTeam)
		{
			case ETeams.RedTeam:
				SpawnRedTeam(1);
				SpawnBlueTeam(0);
				break;
			case ETeams.BlueTeam:
				SpawnRedTeam(0);
				SpawnBlueTeam(1);
				break;
			default:
				break;
		}
	}

	private void SpawnRedTeam(int i)
	{
		if (i >= 1)
		{
			m_playerObject.transform.position = m_redTeamSpawnPoints[0].position;
			m_playerObject.transform.rotation = m_redTeamSpawnPoints[0].rotation;
		}

		while (i < m_redTeamSpawnPoints.Length)
		{
			SpawnAIByPostion(m_redTeamSpawnPoints[i], m_aiID, ETeams.RedTeam);
			m_cachedSpawnPoints.Add(m_redTeamSpawnPoints[i]);
			++m_aiID;
			++i;
		}
	}

	private void SpawnBlueTeam(int i)
	{
		if (i >= 1)
		{
			m_playerObject.transform.position = m_blueTeamSpawnPoints[0].position;
			m_playerObject.transform.rotation = m_blueTeamSpawnPoints[0].rotation;
		}

		while (i < m_blueTeamSpawnPoints.Length)
		{
			SpawnAIByPostion(m_blueTeamSpawnPoints[i], m_aiID, ETeams.BlueTeam);
			m_cachedSpawnPoints.Add(m_blueTeamSpawnPoints[i]);
			++m_aiID;
			++i;
		}
	}

	private void SpawnAIByPostion(Transform spawnPoint, int ID, ETeams team)
	{
		GameObject bot = Instantiate(m_AIPrefab, spawnPoint.position, spawnPoint.rotation);

		AIAgentBehavior agentBehavior = bot.GetComponent<AIAgentBehavior>();

		if (agentBehavior != null)
		{
			agentBehavior.SetCharacterID(ID);

			switch (team)
			{
				case ETeams.RedTeam:
					agentBehavior.SetOpposingTeamFlag(m_blueTeamFlag.transform);
					agentBehavior.SetTeamFlag(m_redTeamFlag.transform);
					break;
				case ETeams.BlueTeam:
					agentBehavior.SetOpposingTeamFlag(m_redTeamFlag.transform);
					agentBehavior.SetTeamFlag(m_blueTeamFlag.transform);
					break;
				default:
					break;
			}

		}

		Health aiHealth = bot.GetComponent<Health>();

		if (aiHealth != null)
		{
			aiHealth.OnDeath += OnAIDeath;
			aiHealth.SetTeam(team);
		}
	}

	public void AddTeamScore(ETeams team, int score)
	{

	}
}
