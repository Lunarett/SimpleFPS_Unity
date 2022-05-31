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
	public abstract void SetOwnerObject(GameObject owner);
	public abstract void StartFire();
	public abstract void StopFire();
	protected abstract void Fire();
	public abstract void Reload();

	//Getters
	public abstract GameObject GetOwnerObject();
	public abstract int GetCurrentAmmo();
	public abstract int GetMagazineSize();
	public abstract string GetWeaponName();
	public abstract bool GetIsFiring();
	public abstract bool GetIsReloading();
	public abstract Transform GetFireTransform();

}
