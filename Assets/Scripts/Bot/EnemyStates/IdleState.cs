using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IBotState
{
	public BotStateID GetID()
	{
		return BotStateID.Idle;
	}
	
	public void Enter(BotAgentBehavior agent)
	{
	}
	
	public void Update(BotAgentBehavior agent)
	{
	}

	public void Exit(BotAgentBehavior agent)
	{
	}
}
