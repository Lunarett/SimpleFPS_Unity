using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AISightSensor : MonoBehaviour
{
	[Header("Sight Properties")]
	[SerializeField] private float m_distance = 10.0f;
	[SerializeField] private float m_angle = 30.0f;
	[SerializeField] private float m_height = 1.0f;
	[SerializeField] private Vector3 m_offset;
	[Space]
	[SerializeField] private LayerMask m_targetLayerMask;
	[SerializeField] private LayerMask m_occlusionLayerMask;
	[Space]
	[SerializeField] private int m_scanFrequency = 30;
	
	[Header("Gizmos Properties")]
	[SerializeField] private Color m_sightZoneMeshColor = Color.cyan;
	[SerializeField] private Color m_detectedColor = Color.green;

	private Mesh m_sightZoneMesh;
	private List<GameObject> m_objectList = new List<GameObject>();
	private Collider[] m_colliders = new Collider[50];
	private int m_count;
	private float m_scanInterval;
	private float m_scanTimer;

	public List<GameObject> ObjectList
	{
		get
		{
			m_objectList.RemoveAll(obj => !obj);
			return m_objectList;
		}
	}

	private void Start()
	{
		m_scanInterval = 1.0f / m_scanFrequency;
	}

	private void Update()
	{
		m_scanTimer -= Time.deltaTime;

		if(m_scanTimer < 0)
		{
			m_scanTimer += m_scanInterval;
			Scan();
		}
	}

	private void Scan()
	{
		m_count = Physics.OverlapSphereNonAlloc(transform.position, m_distance, m_colliders, m_targetLayerMask, QueryTriggerInteraction.Collide);

		m_objectList.Clear();

		for (int i = 0; i < m_count; ++i)
		{
			GameObject go = m_colliders[i].gameObject;

			if(IsInSight(go))
			{
				m_objectList.Add(go);
			}
		}
	}

	public bool IsInSight(GameObject obj)
	{
		Vector3 origin = transform.position + m_offset;
		Vector3 destination = obj.transform.position;
		Vector3 direction = destination - origin;

		if(direction.y < 0 || direction.y > m_height)
		{
			return false;
		}

		direction.y = 0;
		float deltaAngle = Vector3.Angle(direction, transform.forward);
		
		if(deltaAngle > m_angle)
		{
			return false;
		}

		origin.y += m_height / 2;
		destination.y = origin.y;

		if(Physics.Linecast(origin, destination, m_occlusionLayerMask))
		{
			return false;
		}

		return true;
	}

	private Mesh CreateSightZoneMesh()
	{
		Mesh mesh = new Mesh();

		int segments = 10;
		int numberOfTriangles = (segments * 4) + 2 + 2;
		int numberOfVertices = numberOfTriangles * 3;

		Vector3[] vertices = new Vector3[numberOfVertices];
		int[] triangles = new int[numberOfVertices];

		Vector3 bottomCenter = Vector3.zero;
		Vector3 bottomLeft = Quaternion.Euler(0, -m_angle, 0) * Vector3.forward * m_distance;
		Vector3 bottomRight = Quaternion.Euler(0, m_angle, 0) * Vector3.forward * m_distance;

		Vector3 topCenter = bottomCenter + Vector3.up * m_height;
		Vector3 topLeft = bottomLeft + Vector3.up * m_height;
		Vector3 topRight = bottomRight + Vector3.up * m_height;

		int vert = 0;

		// Left Side
		vertices[vert++] = bottomCenter;
		vertices[vert++] = bottomLeft;
		vertices[vert++] = topLeft;

		vertices[vert++] = topLeft;
		vertices[vert++] = topCenter;
		vertices[vert++] = bottomCenter;


		//Right Side
		vertices[vert++] = bottomCenter;
		vertices[vert++] = topCenter;
		vertices[vert++] = topRight;

		vertices[vert++] = topRight;
		vertices[vert++] = bottomRight;
		vertices[vert++] = bottomCenter;

		float currentAngle = -m_angle;
		float deltaAngle = (m_angle * 2) / segments;

		for (int i = 0; i < segments; ++i)
		{
			bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * m_distance;
			bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * m_distance;

			topLeft = bottomLeft + Vector3.up * m_height;
			topRight = bottomRight + Vector3.up * m_height;

			// Far Side
			vertices[vert++] = bottomLeft;
			vertices[vert++] = bottomRight;
			vertices[vert++] = topRight;

			vertices[vert++] = topRight;
			vertices[vert++] = topLeft;
			vertices[vert++] = bottomLeft;


			// Top
			vertices[vert++] = topCenter;
			vertices[vert++] = topLeft;
			vertices[vert++] = topRight;

			// Bottom
			vertices[vert++] = bottomCenter;
			vertices[vert++] = bottomRight;
			vertices[vert++] = bottomLeft;

			currentAngle += deltaAngle;
		}

		for (int i = 0; i < numberOfVertices; ++i)
		{
			triangles[i] = i;
		}

		mesh.vertices = vertices;
		mesh.triangles = triangles;

		mesh.RecalculateNormals();

		return mesh;
	}

	private void OnValidate()
	{
		m_sightZoneMesh = CreateSightZoneMesh();
		m_sightZoneMeshColor = new Color(m_sightZoneMeshColor.r, m_sightZoneMeshColor.g, m_sightZoneMeshColor.b, 0.5f);
		m_scanInterval = 1.0f / m_scanFrequency;
	}

	private void OnDrawGizmos()
	{
		if(m_sightZoneMesh)
		{
			Gizmos.color = m_sightZoneMeshColor;
			Gizmos.DrawMesh(m_sightZoneMesh, transform.position + m_offset, transform.rotation);
		}

		Gizmos.DrawWireSphere(transform.position, m_distance);

		for (int i = 0; i < m_count; ++i)
		{
			Gizmos.DrawSphere(m_colliders[i].transform.position, 0.2f);
		}

		Gizmos.color = m_detectedColor;
		foreach(var obj in m_objectList)
		{
			Gizmos.DrawSphere(obj.transform.position, 0.2f);
		}
	}
}
