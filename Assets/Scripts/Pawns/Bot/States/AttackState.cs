using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : IBotState
{
	public BotStateID GetID()
	{
		return BotStateID.ChaseTarget;
	}

	public void Enter(AIAgentBehavior agent)
	{
		agent.GetWeaponIK().SetWeight(1);

		// Aim Weapon at enemy
		if(agent.GetEnemyTarget() != null)
		{
			agent.GetWeaponIK().SetTargetTransform(agent.GetEnemyTarget());
		}
	}

	public void Update(AIAgentBehavior agent)
	{
		// Died
		if(agent.Gethealth().CurrentHealth <= 0)
		{
			agent.GetWeapon().StopFire();
			agent.GetNavMeshAgent().destination = agent.transform.position;
			agent.GetStateMachine().ChangeState(BotStateID.Death);
		}

		// Check if Enemy is in sight
		if (agent.GetAISightSensor().IsInSight(agent.GetEnemyTarget().gameObject))
		{
			// Move to enemy
			agent.MoveTo(agent.GetEnemyTarget().transform);

			// Shoot at enemy
			agent.GetWeapon().StartFire();
		}
		else
		{
// 			// Stop Fire and return to previous state
// 			agent.GetWeapon().StopFire();
// 			agent.GetStateMachine().ChangeState(BotStateID.Idle);
		}
	}

	public void Exit(AIAgentBehavior agent)
	{
	}
}
