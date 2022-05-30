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
		if (agent.PatrolPoints.Length <= 0) { return; }


	}

	public void Exit(EnemyAgent agent)
	{
	}
}
