using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseTargetState : IEnemyState
{
	float timer = 0;

	public EnemyStateID GetID()
	{
		return EnemyStateID.ChaseTarget;
	}

	public void Enter(EnemyAgent agent)
	{
		agent.target = GameObject.FindGameObjectWithTag(agent.Config.TargetTag).transform;

		if (agent.target == null)
			Debug.LogError("Couldn't Find the Target in this scene");
	}

	public void Update(EnemyAgent agent)
	{
		if(agent.target == null) { agent.target = GameObject.FindGameObjectWithTag(agent.Config.TargetTag).transform; }

		if (agent.health.CurrentHealth <= 0)
		{
			agent.weapon.StartFire = false;
			agent.navMeshAgent.destination = agent.transform.position;
			agent.stateMachine.ChangeState(EnemyStateID.Death);
			return;
		}

		RaycastHit hit;
		Vector3 dir = (agent.target.position - agent.firelocation.position);
		bool ray = Physics.Raycast(new Ray((agent.transform.position + Vector3.up * 1.5f), dir), out hit, 100);

		if(ray && InRange(agent, agent.Config.MinShootingRange))
		{

			if (hit.collider.CompareTag("Player"))
			{
				Debug.Log(hit.collider.name);
				agent.weapon.StartFire = true;
			}
			else
			{
				agent.weapon.StartFire = false;
			}
		}


		MoveToTarget(agent);
	}

	public void Exit(EnemyAgent agent)
	{

	}

	private void MoveToTarget(EnemyAgent agent)
	{
		if (!agent.enabled)
			return;

		timer -= Time.deltaTime;
		if (!agent.navMeshAgent.hasPath)
		{
			agent.navMeshAgent.destination = agent.target.position;
		}


		if (timer < 0)
		{
			Vector3 dir = (agent.target.position - agent.navMeshAgent.destination);
			dir.y = 0;

			if (dir.sqrMagnitude > agent.Config.MinDistance * agent.Config.MinDistance)
			{
				if (agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
				{
					agent.navMeshAgent.destination = agent.target.position;
				}
			}

			timer = agent.Config.UpdateTimer;
		}
	}

	private void Flee(EnemyAgent agent)
	{
		float distance = Vector3.Distance(agent.transform.position, agent.target.position);
	}

	private bool InRange(EnemyAgent agent, float distance)
	{
		float dist = Vector3.Distance(agent.target.position, agent.transform.position);
		return dist < distance;
	}
}
