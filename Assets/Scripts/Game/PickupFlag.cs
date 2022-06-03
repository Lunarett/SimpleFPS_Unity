using UnityEngine;

public class PickupFlag : MonoBehaviour
{
	[SerializeField] private ETeams m_opposingTeam;
	
	private CapsuleCollider m_capsuleCollider;
	private SphereCollider m_sphereCollider;
	private Rigidbody m_rigidbody;

	private void Awake()
	{
		m_capsuleCollider = GetComponent<CapsuleCollider>();
		m_sphereCollider = GetComponent<SphereCollider>();
		m_rigidbody = GetComponent<Rigidbody>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag(m_opposingTeam.ToString()))
		{
			transform.SetParent(other.transform);

			m_capsuleCollider.enabled = false;
			m_sphereCollider.enabled = false;
			m_rigidbody.isKinematic = true;

			transform.localPosition = Vector3.zero;
		}
	}

	public void Reset()
	{
		transform.SetParent(null);

		m_capsuleCollider.enabled = true;
		m_sphereCollider.enabled = true;
		m_rigidbody.isKinematic = false;
	}
}
