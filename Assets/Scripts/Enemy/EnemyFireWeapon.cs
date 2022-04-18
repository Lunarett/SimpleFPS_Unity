using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireWeapon : MonoBehaviour
{
	[SerializeField] EnemyWeapon weapon;
	[Space]
	[SerializeField] float reactionDelay;

	private void Awake()
	{

	}

	private void Update()
	{
		weapon.Fire();
	}
}
