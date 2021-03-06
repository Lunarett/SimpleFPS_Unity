using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerCharacterBehavior : MonoBehaviour
{
	// Unity Default Methods
	protected virtual void Awake() { }
	protected virtual void Start() { }
	protected virtual void Update() { }
	protected virtual void FixedUpdate() { }
	protected virtual void LateUpdate() { }

	// Methods
	public abstract void SetPaused(bool paused);
	public abstract void SetCharacterID(int id);

	// Getters
	public abstract bool IsPaused();
	public abstract int GetCharacterID();
}
