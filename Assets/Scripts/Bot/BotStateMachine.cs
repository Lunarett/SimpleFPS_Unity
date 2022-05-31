using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotStateMachine
{
	public IBotState[] EnemyStates;
	public BotAgent Agent;
	public BotStateID CurrentState;

	public BotStateMachine(BotAgent agent)
	{
		Agent = agent;
		int numStates = System.Enum.GetNames(typeof(BotStateID)).Length;
		EnemyStates = new IBotState[numStates];
	}

	public void RegisterState(IBotState state)
	{
		int i = (int)state.GetID();
		EnemyStates[i] = state;
	}

	public IBotState GetState(BotStateID stateID)
	{
		int i = (int)stateID;
		return EnemyStates[i];
	}

	public void Update()
	{
		GetState(CurrentState)?.Update(Agent);
	}

	public void ChangeState(BotStateID newSate)
	{
		GetState(CurrentState)?.Exit(Agent);
		CurrentState = newSate;
		GetState(CurrentState)?.Enter(Agent);
	}
}
