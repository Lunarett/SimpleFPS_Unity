using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseTargetState : IBotState
{
	float timer = 0;

	public BotStateID GetID()
	{
		return BotStateID.ChaseTarget;
	}

	public void Enter(AIAgentBehavior agent)
	{
	}

	public void Update(AIAgentBehavior agent)
	{
		if(agent.GetTarget() == null) 
		{ 
			agent.SetTarget(GameObject.FindGameObjectWithTag("RedTeam").transform);
			return;
		}

		if (agent.Gethealth().CurrentHealth <= 0)
		{
			agent.GetWeapon().StopFire();
			agent.GetNavMeshAgent().destination = agent.transform.position;
			agent.GetStateMachine().ChangeState(BotStateID.Death);
			return;
		}

		RaycastHit hit;
		Vector3 dir = (agent.GetTarget().position - agent.GetWeapon().GetFireTransform().position);
		bool ray = Physics.Raycast(new Ray((agent.transform.position + Vector3.up * 1.5f), dir), out hit, 100);

		if(ray && InRange(agent, agent.GetConfig().MinShootingRange))
		{

			if (hit.collider.CompareTag("RedTeam"))
			{
				agent.GetWeapon().StartFire();
			}
			else
			{
				agent.GetWeapon().StopFire();
			}
		}


		MoveToTarget(agent);
	}

	public void Exit(AIAgentBehavior agent)
	{

	}

	private void MoveToTarget(AIAgentBehavior agent)
	{
		if (!agent.enabled)
			return;

		timer -= Time.deltaTime;
		if (!agent.GetNavMeshAgent().hasPath)
		{
			agent.GetNavMeshAgent().destination = agent.GetTarget().position;
		}


		if (timer < 0)
		{
			Vector3 dir = (agent.GetTarget().position - agent.GetNavMeshAgent().destination);
			dir.y = 0;

			if (dir.sqrMagnitude > agent.GetConfig().MinDistance * agent.GetConfig().MinDistance)
			{
				if (agent.GetNavMeshAgent().pathStatus != NavMeshPathStatus.PathPartial)
				{
					agent.GetNavMeshAgent().destination = agent.GetTarget().position;
				}
			}

			timer = agent.GetConfig().UpdateTimer;
		}
	}

	private bool InRange(AIAgentBehavior agent, float distance)
	{
		float dist = Vector3.Distance(agent.GetTarget().position, agent.transform.position);
		return dist < distance;
	}
}
