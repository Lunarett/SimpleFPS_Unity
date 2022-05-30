using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAgentBehavior : MonoBehaviour
{
	// Unity Default Methods
	protected virtual void Awake() { }
	protected virtual void Start() { }
	protected virtual void Update() { }
	protected virtual void FixedUpdate() { }
	protected virtual void LateUpdate() { }

	public abstract EnemyStateMachine GetStateMachine();
	public abstract IEnemyState GetStateID();
	public abstract EnemyAgentConfig GetConfig();
	public abstract EnemyWeapon GetWeapon();
}
