using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BotStateID
{
	Idle,
	CaptureFlag,
	Attack,
	Death
}

public interface IBotState
{
	BotStateID GetID();

	void Enter(AIAgentBehavior agent);
	void Update(AIAgentBehavior agent);
	void Exit(AIAgentBehavior agent);
}
