using System;
using UnityEngine;

public abstract class MovementBehavior : MonoBehaviour
{
	// Unity Default Methods
	protected virtual void Awake() { }
	protected virtual void Start() { }
	protected virtual void Update() { }
	protected virtual void FixedUpdate() { }
	protected virtual void LateUpdate() { }

	// Methods
	protected abstract void Move();
	protected abstract void Fly();

	// Getters
	public abstract float GetCurrentFuel();
	public abstract float GetMaxFuel();

	public abstract bool IsSprinting();
	public abstract bool IsGrounded();
}
