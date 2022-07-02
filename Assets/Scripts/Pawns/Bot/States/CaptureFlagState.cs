using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureFlagState : IBotState
{
	public BotStateID GetID()
	{
		return BotStateID.CaptureFlag;
	}
	
	public void Enter(AIAgentBehavior agent)
	{
		agent.GetWeaponIK().SetWeight(1);
	}
	
	public void Update(AIAgentBehavior agent)
	{
		if (agent.GetHealth().CurrentHealth <= 0)
		{
			agent.GetNavMeshAgent().destination = agent.transform.position;
			agent.GetStateMachine().ChangeState(BotStateID.Death);
		}

		if(agent.GetTarget().HasTarget)
		{
			agent.GetStateMachine().ChangeState(BotStateID.Attack);
		}

		if(!agent.GetFlagHolder().IsHoldingFlag)
		{
			if(agent.GetOpposingTeamFlag() != null)
			{
				agent.MoveTo(agent.GetOpposingTeamFlag());
			}
			else
			{
				agent.MoveTo(agent.transform);
			}
		}
		else
		{
			agent.MoveTo(agent.GetTeamFlag());
		}
	}

	public void Exit(AIAgentBehavior agent)
	{
		agent.GetNavMeshAgent().destination = agent.transform.position;
	}
}
