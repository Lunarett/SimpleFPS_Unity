using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatHolderBehavior : MonoBehaviour
{
	public abstract void AddKill();
	public abstract void AddDeath();
	public abstract void SetIsHoldingFlag(bool isHolding);

	public abstract int GetKills();
	public abstract int GetDeaths();
	public abstract bool GetIsHoldingFlag();
}
