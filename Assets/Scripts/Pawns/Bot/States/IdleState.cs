using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IBotState
{
	public BotStateID GetID()
	{
		return BotStateID.Idle;
	}
	
	public void Enter(AIAgentBehavior agent)
	{
		
	}
	
	public void Update(AIAgentBehavior agent)
	{
		if(agent.GetTarget().HasTarget)
		{
			agent.GetWeaponIK().SetWeight(1);
			Transform t = agent.GetTarget().Target.transform;
			t.position = agent.GetTarget().TargetPosition;
			agent.GetWeaponIK().SetTargetTransform(t);
		}
		else
		{
			agent.GetWeaponIK().SetWeight(0);
		}
	}

	public void Exit(AIAgentBehavior agent)
	{
	}
}
