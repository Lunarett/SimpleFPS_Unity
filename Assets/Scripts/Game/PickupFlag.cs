using System.Collections;
using UnityEngine;

public enum EFlagState
{
	Base,
	PickedUp,
	Dropped
}

public class PickupFlag : MonoBehaviour
{
	[SerializeField] private ETeams m_team;
	[SerializeField] private LayerMask m_pawnMask;

	private CapsuleCollider m_poleCollider;
	private SphereCollider m_pickupZoneCollider;
	private Rigidbody m_rigidbody;
	private FlagHolder m_flagHolder;
	private Health m_health;

	private Vector3 m_basePosition;
	private Quaternion m_baseRotation;

	public EFlagState FlagState;

	private void Awake()
	{
		m_poleCollider = GetComponent<CapsuleCollider>();
		m_pickupZoneCollider = GetComponent<SphereCollider>();
		m_rigidbody = GetComponent<Rigidbody>();
	}

	private void Start()
	{
		FlagState = EFlagState.Base;

		m_basePosition = transform.position;
		m_baseRotation = transform.rotation;
	}

	private void OnTriggerEnter(Collider other)
	{

		if (FlagState == EFlagState.Base && m_pawnMask.value == 1 << other.gameObject.layer)
		{
			m_health = other.GetComponent<Health>();
			m_flagHolder = other.GetComponent<FlagHolder>();

			if (m_flagHolder != null && m_health != null)
			{
				// If Opposing Team, pick up this flag
				if (m_health.Team != m_team)
				{
					Pickup(other.gameObject);
				}

				// If holding flag, score and return opposing teams flag to base
				if (m_flagHolder.IsHoldingFlag && m_team == m_health.Team)
				{
					m_flagHolder.ReturnFlagToBase();
				}
			}

			if (m_flagHolder == null) { Debug.LogError("FlagHolder is returning NULL"); }
			if (m_health == null) { Debug.LogError("Health is returning NULL"); }
		}
	}

	public void Pickup(GameObject pawn)
	{
		if (FlagState == EFlagState.Dropped)
		{
			StopCoroutine(ReturnFlag());
		}

		FlagState = EFlagState.PickedUp;

		m_poleCollider.enabled = false;
		m_pickupZoneCollider.enabled = true;
		m_rigidbody.isKinematic = true;

		m_flagHolder.Pickup(gameObject, this);
	}

	public void DropFlag(GameObject pawn)
	{
		FlagHolder fh = pawn.GetComponent<FlagHolder>();

		if (fh != null)
		{
			FlagState = EFlagState.Dropped;

			m_poleCollider.enabled = true;
			m_pickupZoneCollider.enabled = true;
			m_rigidbody.isKinematic = false;

			fh.DropFlag();
		}
		else
		{
			Debug.LogError("NULLLL");
		}

		StartCoroutine(ReturnFlag());
	}

	IEnumerator ReturnFlag()
	{
		yield return new WaitForSeconds(10);

		if (FlagState == EFlagState.Dropped)
		{
			ReturnFlagToBase();
		}
	}

	public void ReturnFlagToBase()
	{
		if (FlagState != EFlagState.Base)
		{
			transform.position = m_basePosition;
			transform.rotation = m_baseRotation;

			FlagState = EFlagState.Base;
		}
	}
}
