using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
	[SerializeField] private Health m_health;

	public Health Health { get => m_health; }
}
