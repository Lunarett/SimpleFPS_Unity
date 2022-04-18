using System.Collections;
using System.Collections.Generic;
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

	// Getters
	public abstract bool IsSprinting();
	public abstract bool IsGrounded();
}
