using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : IEnemyState
{
	public EnemyStateID GetID()
	{
		return EnemyStateID.Death;
	}
	
	public void Enter(EnemyAgent agent)
	{
		agent.navMeshAgent.isStopped = true;
		agent.ragdoll.ActivateRagdoll();
		agent.mesh.updateWhenOffscreen = true;
	}

	public void Update(EnemyAgent agent)
	{
	}

	public void Exit(EnemyAgent agent)
	{

	}
}
