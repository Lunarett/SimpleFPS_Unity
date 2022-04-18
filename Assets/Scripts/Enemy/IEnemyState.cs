using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStateID
{
	Idle,
	ChaseTarget,
	Death
}

public interface IEnemyState
{
	EnemyStateID GetID();

	void Enter(EnemyAgent agent);
	void Update(EnemyAgent agent);
	void Exit(EnemyAgent agent);
}
