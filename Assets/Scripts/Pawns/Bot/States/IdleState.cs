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
	}

	public void Exit(AIAgentBehavior agent)
	{
	}
}
