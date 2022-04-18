using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
	public IEnemyState[] EnemyStates;
	public EnemyAgent Agent;
	public EnemyStateID CurrentState;

	public EnemyStateMachine(EnemyAgent agent)
	{
		Agent = agent;
		int numStates = System.Enum.GetNames(typeof(EnemyStateID)).Length;
		EnemyStates = new IEnemyState[numStates];
	}

	public void RegisterState(IEnemyState state)
	{
		int i = (int)state.GetID();
		EnemyStates[i] = state;
	}

	public IEnemyState GetState(EnemyStateID stateID)
	{
		int i = (int)stateID;
		return EnemyStates[i];
	}

	public void Update()
	{
		GetState(CurrentState)?.Update(Agent);
	}

	public void ChangeState(EnemyStateID newSate)
	{
		GetState(CurrentState)?.Exit(Agent);
		CurrentState = newSate;
		GetState(CurrentState)?.Enter(Agent);
	}
}
