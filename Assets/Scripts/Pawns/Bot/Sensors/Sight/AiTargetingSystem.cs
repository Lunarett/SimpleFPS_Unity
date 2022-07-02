using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AiTargetingSystem : MonoBehaviour
{
	[SerializeField] private float m_forgetTargetTime = 3.0f;

	[Header("Weights")]
	[SerializeField] private float m_distanceWeight = 1.0f;
	[SerializeField] private float m_angleWeight = 1.0f;
	[SerializeField] private float m_ageWeight = 1.0f;

	private AISensoryMemory m_memory = new AISensoryMemory(10);
	private AISightSensor m_sightSensor;
	private AIMemory m_bestMemory;

	public bool HasTarget
	{
		get
		{
			return m_bestMemory != null;
		}
	}

	public GameObject Target
	{
		get
		{
			return m_bestMemory.gameObject;
		}
	}

	public Vector3 TargetPosition
	{
		get
		{
			return m_bestMemory.gameObject.transform.position;
		}
	}

	public bool TargetInSight
	{
		get
		{
			return m_bestMemory.Age < 0.5f; 
		}
	}

	public float TargetDistance
	{
		get
		{
			return m_bestMemory.distance;
		}
	}

	private void Start()
	{
		m_sightSensor = GetComponent<AISightSensor>();
	}

	private void Update()
	{
		m_memory.UpdateSensory(m_sightSensor);
		m_memory.ForgetMemories(m_forgetTargetTime);
		
		EvaluateScores();
	}

 	void EvaluateScores()
 	{
		m_bestMemory = null;

 		foreach (var memory in m_memory.AIMemoryList)
 		{
 			memory.score = CalculateScore(memory);

			if(m_bestMemory == null || memory.score > m_bestMemory.score)
			{
				m_bestMemory = memory;
			}
 		}
 	}
 
 	private float Normalize(float value, float maxValue)
 	{
 		return 1.0f - (value / maxValue);
 	}
 
 	float CalculateScore(AIMemory memory)
 	{
 		float distanceScore = Normalize(memory.distance, m_sightSensor.Distance) * m_distanceWeight;
 		float angleScore = Normalize(memory.angle, m_sightSensor.Angle) * m_angleWeight;
		float ageScore = Normalize(memory.Age, m_forgetTargetTime) * m_ageWeight;

		return distanceScore + angleScore + ageScore;
 	}

	private void OnDrawGizmos()
	{
 		float maxScore = float.MinValue;
 
 		foreach (var mem in m_memory.AIMemoryList)
 		{
 			maxScore = Mathf.Max(maxScore, mem.score);
 		}
 
 		foreach (var mem in m_memory.AIMemoryList)
 		{
 			Color color = Color.red;
 			color.a = mem.score / maxScore;
 
 			if(mem == m_bestMemory)
 			{
 				color = Color.yellow;
 			}
 
 			Gizmos.color = color;
 			Gizmos.DrawSphere(mem.position, 0.2f);
 		}
	}
}
