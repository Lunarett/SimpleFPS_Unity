using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagHolder : MonoBehaviour
{
	[SerializeField] private Transform m_flagTransform;

	private bool m_isHoldingFlag = false;

	private Vector3 m_startPosition;
	private Quaternion m_startRotation;
	private PickupFlag m_flag;

	public bool IsHoldingFlag { get => m_isHoldingFlag; }

	public void Pickup(GameObject flag, PickupFlag pf)
	{
		m_isHoldingFlag = true;

		m_flag = pf;
		m_startPosition = flag.transform.position;
		m_startRotation = flag.transform.rotation;

		flag.transform.SetParent(m_flagTransform);

		flag.transform.localPosition = m_flagTransform.localPosition;
		flag.transform.localRotation = m_flagTransform.localRotation;
	}

	public void DropFlag(GameObject flag)
	{
		m_isHoldingFlag = false;

		flag.transform.SetParent(null);
		flag.transform.rotation = Quaternion.identity;
	}

	public void ReturnFlagToBase()
	{
		if (m_flag == null)
		{
			Debug.LogError("Flag is NuLL");
			return;
		}

		m_isHoldingFlag = false;

		m_flag.transform.SetParent(null);
		m_flag.FlagState = EFlagState.Base;

		m_flag.transform.position = m_startPosition;
		m_flag.transform.rotation = m_startRotation;

		Debug.Log("YESSSS");
	}
}
