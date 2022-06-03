using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : IBotState
{
	public BotStateID GetID()
	{
		return BotStateID.Death;
	}
	
	public void Enter(BotAgentBehavior agent)
	{
		agent.GetNavMeshAgent().isStopped = true;
		agent.GetRagdoll().ActivateRagdoll();
		agent.GetSkinnedMeshRenderer().updateWhenOffscreen = true;
	}

	public void Update(BotAgentBehavior agent)
	{
	}

	public void Exit(BotAgentBehavior agent)
	{

	}
}
