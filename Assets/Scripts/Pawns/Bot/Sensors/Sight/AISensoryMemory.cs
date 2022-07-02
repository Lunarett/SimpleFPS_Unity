using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMemory
{
	public float Age
	{
		get
		{
			return Time.time - lastSeen;
		}
	}

	public GameObject gameObject;
	public Vector3 position;
	public Vector3 direction;

	public float distance;
	public float angle;
	public float lastSeen;
	public float score;
}

public class AISensoryMemory
{
	private List<AIMemory> m_aiMemoryList = new List<AIMemory>();
	private GameObject[] m_pawns;
	private LayerMask m_targetlayer;

	public List<AIMemory> AIMemoryList { get => m_aiMemoryList; }

	public AISensoryMemory(int maxPlayers)
	{
		m_pawns = new GameObject[maxPlayers];
	}

	public void UpdateSensory(AISightSensor sensor)
	{
		int targets = sensor.Filter(m_pawns, "Pawn");

		for (int i = 0; i < targets; i++)
		{
			GameObject target = m_pawns[i];

			RefreshMemory(sensor.gameObject, target);
		}
	}

	public void RefreshMemory(GameObject agent, GameObject target)
	{
		AIMemory memory = FetchMemory(target);
		memory.gameObject = target;
		memory.position = target.transform.position;
		memory.direction = target.transform.position - agent.transform.position;
		memory.distance = memory.direction.magnitude;
		memory.angle = Vector3.Angle(agent.transform.forward, memory.direction);
		memory.lastSeen = Time.time;
	}

	public AIMemory FetchMemory(GameObject gameObject)
	{
		AIMemory memory = m_aiMemoryList.Find(x => x.gameObject == gameObject);

		if (memory == null)
		{
			memory = new AIMemory();
			m_aiMemoryList.Add(memory);
		}

		return memory;
	}

	public void ForgetMemories(float olderThan)
	{
		m_aiMemoryList.RemoveAll(m => m.Age > olderThan);
		m_aiMemoryList.RemoveAll(m => !m.gameObject);
		m_aiMemoryList.RemoveAll(m => m.gameObject.GetComponent<Health>().IsDead);
	}
}
