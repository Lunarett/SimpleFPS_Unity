using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BotStateID
{
	Idle,
	ChaseTarget,
	Death
}

public interface IBotState
{
	BotStateID GetID();

	void Enter(BotAgentBehavior agent);
	void Update(BotAgentBehavior agent);
	void Exit(BotAgentBehavior agent);
}
