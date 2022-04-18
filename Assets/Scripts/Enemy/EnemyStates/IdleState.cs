using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyState
{
	public EnemyStateID GetID()
	{
		return EnemyStateID.Idle;
	}
	
	public void Enter(EnemyAgent agent)
	{
	}
	
	public void Update(EnemyAgent agent)
	{
		//Vector3 targetDir = agent.target.position - agent.gameObject.transform.position;
		//
		//if (targetDir.magnitude > agent.Config.MaxSightDistance)
		//	return;
		//
		//Vector3 agentDir = agent.gameObject.transform.forward;
		//agentDir.Normalize();
		//float dotProduct = Vector3.Dot(targetDir, agentDir);
		//
		//if(dotProduct > 0)
		//{
		//	agent.stateMachine.ChangeState(EnemyStateID.ChaseTarget);
		//}
	}

	public void Exit(EnemyAgent agent)
	{
	}
}
