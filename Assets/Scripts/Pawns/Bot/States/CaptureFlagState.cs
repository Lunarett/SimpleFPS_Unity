using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureFlagState : IBotState
{
	public BotStateID GetID()
	{
		return BotStateID.Idle;
	}
	
	public void Enter(AIAgentBehavior agent)
	{
		agent.GetWeaponIK().SetWeight(0.0f);
	}
	
	public void Update(AIAgentBehavior agent)
	{
		if (agent.Gethealth().CurrentHealth <= 0)
		{
			agent.GetNavMeshAgent().destination = agent.transform.position;
			agent.GetStateMachine().ChangeState(BotStateID.Death);
		}

		if (agent.GetEnemyTarget() == null)
		{
			if(FindEnemy(agent) != null)
			{
				agent.SetEnemyTarget(FindEnemy(agent).transform);
			}
		}
		else
		{
			agent.GetStateMachine().ChangeState(BotStateID.ChaseTarget);
		}

		if(!agent.GetFlagHolder().IsHoldingFlag)
		{
			agent.MoveTo(agent.GetOpposingTeamFlag());
		}
		else
		{
			agent.MoveTo(agent.GetTeamFlag());
		}
	}

	public void Exit(AIAgentBehavior agent)
	{
	}

	private GameObject FindEnemy(AIAgentBehavior agent)
	{
		if(agent.GetAISightSensor().ObjectList.Count > 0)
		{
			for (int i = 0; i < agent.GetAISightSensor().ObjectList.Count; i++)
			{
				if (agent.GetAISightSensor().ObjectList[i].GetComponent<Health>().Team != agent.Gethealth().Team)
				{
					return agent.GetAISightSensor().ObjectList[i];
				}
			}
		}

		return null;
	}
}
