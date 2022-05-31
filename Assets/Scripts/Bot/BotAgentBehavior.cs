using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BotAgentBehavior : MonoBehaviour
{
	// Unity Default Methods
	protected virtual void Awake() { }
	protected virtual void Start() { }
	protected virtual void Update() { }
	protected virtual void FixedUpdate() { }
	protected virtual void LateUpdate() { }

	// Methods
	public abstract void SetTarget(Transform target);

	// Getters
	public abstract BotStateMachine GetStateMachine();
	public abstract EnemyAgentConfig GetConfig();
	public abstract WeaponBehavior GetWeapon();
	public abstract NavMeshAgent GetNavMeshAgent();
	public abstract Health Gethealth();
	public abstract Transform GetTarget();
	public abstract Ragdoll GetRagdoll();
	public abstract SkinnedMeshRenderer GetSkinnedMeshRenderer();
	public abstract BotWeaponIK GetWeaponIK();
}
