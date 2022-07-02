using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : AIAgentBehavior
{
	[Header("Bot Properties")]
	[SerializeField] private BotStateMachine m_stateMachine;
	[SerializeField] private EnemyAgentConfig m_config;
	[SerializeField] private WeaponBehavior m_weapon;
	[Space]
	[SerializeField] private BotStateID m_initialState;

	private Transform m_opposingTeamFlag;
	private Transform m_teamFlag;
	private Transform m_enemyTarget;
	private Health m_health;
	private NavMeshAgent m_navMeshAgent;
	private Ragdoll m_ragdoll;
	private SkinnedMeshRenderer m_skinnedMesh;
	private BotWeaponIK m_weaponIK;
	private AISightSensor m_sightSensor;
	private FlagHolder m_flagHolder;
	private Transform m_spawnTransform;
	private AiTargetingSystem m_target;

	private int m_charcterID = 0;
	float m_timer = 0;

	public override BotStateMachine GetStateMachine() => m_stateMachine;
	public override EnemyAgentConfig GetConfig() => m_config;
	public override WeaponBehavior GetWeapon() => m_weapon;
	public override NavMeshAgent GetNavMeshAgent() => m_navMeshAgent;
	public override Health GetHealth() => m_health;
	public override Transform GetOpposingTeamFlag() => m_opposingTeamFlag;
	public override Transform GetTeamFlag() => m_teamFlag;
	public override Transform GetEnemyTarget() => m_enemyTarget;
	public override Ragdoll GetRagdoll() => m_ragdoll;
	public override SkinnedMeshRenderer GetSkinnedMeshRenderer() => m_skinnedMesh;
	public override BotWeaponIK GetWeaponIK() => m_weaponIK;
	public override AISightSensor GetAISightSensor() => m_sightSensor;
	public override FlagHolder GetFlagHolder() => m_flagHolder;
	public override Transform GetSpawnTransform() => m_spawnTransform;
	public override AiTargetingSystem GetTarget() => m_target;

	protected override void Awake()
	{
		m_weapon.SetOwnerObject(gameObject);

		m_navMeshAgent = GetComponent<NavMeshAgent>();
		m_ragdoll = GetComponent<Ragdoll>();
		m_skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
		m_sightSensor = GetComponent<AISightSensor>();
		m_health = GetComponent<Health>();
		m_flagHolder = GetComponent<FlagHolder>();
		m_weaponIK = GetComponent<BotWeaponIK>();
		m_target = GetComponent<AiTargetingSystem>();

		m_health.OnDeath += DestroyEnemy;

	}

	protected override void Start()
	{
		m_spawnTransform = transform;

		m_stateMachine = new BotStateMachine(this);
		m_stateMachine.RegisterState(new AttackState());
		m_stateMachine.RegisterState(new DeathState());
		m_stateMachine.RegisterState(new CaptureFlagState());
		m_stateMachine.RegisterState(new IdleState());
		m_stateMachine.ChangeState(m_initialState);

		StartCoroutine(LateStart());
	}

	IEnumerator LateStart()
	{
		yield return new WaitForSeconds(1);
		m_ragdoll.DeactivateRagdoll();
	}

	protected override void Update()
	{
		m_stateMachine.Update();
	}

	public override void SetCharacterID(int id)
	{
		m_charcterID = id;
	}

	public override int GetCharacterID()
	{
		return m_charcterID;
	}

	public override void SetOpposingTeamFlag(Transform target)
	{
		m_opposingTeamFlag = target;
	}

	public override void SetTeamFlag(Transform target)
	{
		m_teamFlag = target;
	}

	public override void SetEnemyTarget(Transform target)
	{
		m_enemyTarget = target;
	}

	public override void MoveTo(Transform location)
	{
		m_timer -= Time.deltaTime;
		if (!m_navMeshAgent.hasPath)
		{
			m_navMeshAgent.destination = location.position;
		}

		if (m_timer < 0)
		{
			Vector3 dir = (location.position - m_navMeshAgent.destination);
			dir.y = 0;

			if (dir.sqrMagnitude > m_config.MinDistance * m_config.MinDistance)
			{
				if (m_navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
				{
					m_navMeshAgent.destination = location.position;
				}
			}

			m_timer = m_config.UpdateTimer;
		}
	}


	public override void MoveTo(Vector3 position)
	{
		m_timer -= Time.deltaTime;
		if (!m_navMeshAgent.hasPath)
		{
			m_navMeshAgent.destination = position;
		}

		if (m_timer < 0)
		{
			Vector3 dir = (position - m_navMeshAgent.destination);
			dir.y = 0;

			if (dir.sqrMagnitude > m_config.MinDistance * m_config.MinDistance)
			{
				if (m_navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
				{
					m_navMeshAgent.destination = position;
				}
			}

			m_timer = m_config.UpdateTimer;
		}
	}

	private void DestroyEnemy(GameObject killedPawn, GameObject killerPawn)
	{
		StartCoroutine(DestroyDelay());
	}

	IEnumerator DestroyDelay()
	{
		yield return new WaitForSeconds(m_config.DestroyTimer);
		gameObject.SetActive(false);
	}
}
