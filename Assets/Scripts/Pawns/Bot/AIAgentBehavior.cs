using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AIAgentBehavior : MonoBehaviour
{
	// Unity Default Methods
	protected virtual void Awake() { }
	protected virtual void Start() { }
	protected virtual void Update() { }
	protected virtual void FixedUpdate() { }
	protected virtual void LateUpdate() { }

	// Methods
	public abstract void SetCharacterID(int id);
	public abstract void SetOpposingTeamFlag(Transform target);
	public abstract void SetTeamFlag(Transform target);
	public abstract void SetEnemyTarget(Transform target);
	public abstract void MoveTo(Transform location);

	// Getters
	public abstract int GetCharacterID();
	public abstract BotStateMachine GetStateMachine();
	public abstract EnemyAgentConfig GetConfig();
	public abstract WeaponBehavior GetWeapon();
	public abstract NavMeshAgent GetNavMeshAgent();
	public abstract Health Gethealth();
	public abstract Transform GetOpposingTeamFlag();
	public abstract Transform GetTeamFlag();
	public abstract Transform GetEnemyTarget();
	public abstract Ragdoll GetRagdoll();
	public abstract SkinnedMeshRenderer GetSkinnedMeshRenderer();
	public abstract BotWeaponIK GetWeaponIK();
	public abstract AISightSensor GetAISightSensor();
	public abstract FlagHolder GetFlagHolder();
}
