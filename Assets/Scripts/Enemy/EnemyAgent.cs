using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAgent : MonoBehaviour
{
	public EnemyStateMachine stateMachine;
	public EnemyStateID initialState;
	public EnemyAgentConfig Config;
	public EnemyWeapon weapon;

	[HideInInspector] public Transform target;
	[HideInInspector] public NavMeshAgent navMeshAgent;
	[HideInInspector] public Ragdoll ragdoll;
	[HideInInspector] public SkinnedMeshRenderer mesh;
	[HideInInspector] public EnemyWeaponIK weaponIK;
	[HideInInspector] public Health health;


	private void Start()
	{
		Time.timeScale = 1;
		navMeshAgent = GetComponent<NavMeshAgent>();
		ragdoll = GetComponent<Ragdoll>();
		mesh = GetComponentInChildren<SkinnedMeshRenderer>();
		health = GetComponent<Health>();
		health.OnDeath += DestroyEnemy;

		stateMachine = new EnemyStateMachine(this);
		stateMachine.RegisterState(new ChaseTargetState());
		stateMachine.RegisterState(new DeathState());
		stateMachine.RegisterState(new IdleState());
		stateMachine.ChangeState(initialState);

		StartCoroutine(LateStart());
	}
	
	IEnumerator LateStart()
	{
		yield return new WaitForSeconds(1);
		ragdoll.DeactivateRagdoll();
	}

	private void Update()
	{
		stateMachine.Update();
	}

	private void DestroyEnemy()
	{
		StartCoroutine(DestroyDelay());
	}

	IEnumerator DestroyDelay()
	{
		yield return new WaitForSeconds(Config.DestroyTimer);
		gameObject.SetActive(false);
	}
}
