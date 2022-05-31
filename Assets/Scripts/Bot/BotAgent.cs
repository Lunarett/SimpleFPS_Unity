using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotAgent : BotAgentBehavior
{
	[Header("Bot Properties")]
	[SerializeField] private BotStateMachine m_stateMachine;
	[SerializeField] private EnemyAgentConfig m_config;
	[SerializeField] private WeaponBehavior m_weapon;
	[Space]
	[SerializeField] private BotStateID m_initialState;

	private Transform m_target;
	private Health m_health;
	private NavMeshAgent m_navMeshAgent;
	private Ragdoll m_ragdoll;
	private SkinnedMeshRenderer m_skinnedMesh;
	private BotWeaponIK m_weaponIK;

	public override BotStateMachine GetStateMachine() => m_stateMachine;
	public override EnemyAgentConfig GetConfig() => m_config;
	public override WeaponBehavior GetWeapon() => m_weapon;
	public override NavMeshAgent GetNavMeshAgent() => m_navMeshAgent;
	public override Health Gethealth() => m_health;
	public override Transform GetTarget() => m_target;
	public override Ragdoll GetRagdoll() => m_ragdoll;
	public override SkinnedMeshRenderer GetSkinnedMeshRenderer() => m_skinnedMesh;
	public override BotWeaponIK GetWeaponIK() => m_weaponIK;

	protected override void Awake()
	{
		m_weapon.SetOwnerObject(gameObject);
	}

	protected override void Start()
	{
		Time.timeScale = 1;
		m_navMeshAgent = GetComponent<NavMeshAgent>();
		m_ragdoll = GetComponent<Ragdoll>();
		m_skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
		m_health = GetComponent<Health>();
		m_health.OnDeath += DestroyEnemy;

		m_stateMachine = new BotStateMachine(this);
		m_stateMachine.RegisterState(new ChaseTargetState());
		m_stateMachine.RegisterState(new DeathState());
		m_stateMachine.RegisterState(new IdleState());
		m_stateMachine.ChangeState(m_initialState);

		StartCoroutine(LateStart());
		m_target = GameObject.FindGameObjectWithTag("Player").transform;
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

	public override void SetTarget(Transform target)
	{
		m_target = target;
	}

	private void DestroyEnemy()
	{
		StartCoroutine(DestroyDelay());
	}

	IEnumerator DestroyDelay()
	{
		yield return new WaitForSeconds(m_config.DestroyTimer);
		gameObject.SetActive(false);
	}
}
