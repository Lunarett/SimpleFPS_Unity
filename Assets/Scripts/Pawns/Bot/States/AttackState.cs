using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : IBotState
{
	public BotStateID GetID()
	{
		return BotStateID.Attack;
	}

	public void Enter(AIAgentBehavior agent)
	{
		agent.GetWeaponIK().SetWeight(1);

		// Aim Weapon at enemy
		if (agent.GetEnemyTarget() != null)
		{
			agent.GetWeaponIK().SetTargetTransform(agent.GetEnemyTarget());
		}
	}

	public void Update(AIAgentBehavior agent)
	{
		// Died
		if (agent.GetHealth().CurrentHealth <= 0)
		{
			agent.GetWeapon().StopFire();
			agent.GetNavMeshAgent().destination = agent.transform.position;
			agent.GetStateMachine().ChangeState(BotStateID.Death);
		}

		if (!agent.GetTarget().HasTarget)
		{
			agent.GetStateMachine().ChangeState(BotStateID.CaptureFlag);
			return;
		}

		Vector3 lookAtPos = new Vector3(agent.GetTarget().TargetPosition.x, agent.transform.position.y, agent.GetTarget().TargetPosition.z);
		agent.transform.LookAt(lookAtPos);

		// Check if Enemy is in sight
		if (agent.GetTarget().TargetInSight)
		{
			// Shoot at enemy
			agent.GetWeapon().StartFire();
			DogingMove(agent);
		}
		else
		{
			agent.GetWeapon().StopFire();

			if (agent.GetTarget().HasTarget)
				agent.MoveTo(agent.GetTarget().TargetPosition);
			else
				agent.GetStateMachine().ChangeState(BotStateID.CaptureFlag);
		}
	}

	private void DogingMove(AIAgentBehavior agent)
	{
		float distance = agent.GetTarget().TargetDistance;

		if (distance > 10)
		{
			agent.MoveTo(agent.GetTarget().TargetPosition);
		}
		else
		{
			Vector3 direction = agent.transform.position - agent.GetTarget().TargetPosition;
			Vector3 newPos = agent.transform.position + (direction * UnityEngine.Random.Range(0, 1) + agent.transform.right * UnityEngine.Random.Range(-4, 4));
			agent.MoveTo(newPos);
		}
	}

	public void Exit(AIAgentBehavior agent)
	{
	}
}
