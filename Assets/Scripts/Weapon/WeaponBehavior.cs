using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBehavior : MonoBehaviour
{
	// Unity Default Methods
	protected virtual void Awake() { }
	protected virtual void Start() { }
	protected virtual void Update() { }
	protected virtual void FixedUpdate() { }
	protected virtual void LateUpdate() { }

	//Methods
	public abstract void Fire();
	public abstract void Reload();

	//Getters
	public abstract int GetCurrentAmmo();
	public abstract int GetMagazineSize();
	public abstract string GetWeaponName();

}
