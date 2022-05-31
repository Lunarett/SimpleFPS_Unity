using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileBehavior : MonoBehaviour
{
	protected virtual void Awake() { }
	protected virtual void Start() { }

	public abstract void SetDamage(float damage);
	public abstract void SetDamageMultiplier(float multiplier);

	public abstract float GetDamage();
}
