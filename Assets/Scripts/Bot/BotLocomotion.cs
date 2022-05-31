using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BotLocomotion : MonoBehaviour
{
	NavMeshAgent agent;
	Animator anim;
	Health health;

	private void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
		health = GetComponent<Health>();
	}

	private void Update()
	{

		if (agent.hasPath)
		{
			float verticalSpeed = Vector3.Dot(agent.velocity.normalized, agent.transform.forward);
			float horizontalSpeed = Vector3.Dot(agent.velocity.normalized, agent.transform.right);

			anim.SetFloat("SpeedX", horizontalSpeed);
			anim.SetFloat("SpeedY", verticalSpeed);
		}
		else
		{
			anim.SetFloat("SpeedX", 0);
			anim.SetFloat("SpeedY", 0);
		}
	}
}
