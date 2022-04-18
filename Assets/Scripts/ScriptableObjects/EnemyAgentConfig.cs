using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class EnemyAgentConfig : ScriptableObject
{
	[Header("Chase Properties")]
	public float MaxSightDistance = 5;
	public float MinShootingRange = 5;
	public float UpdateTimer = 1.0f;
	public float MinDistance = 1.0f;
	public string TargetTag = "Player";

	[Header("Death")]
	public float DestroyTimer = 5;
}
