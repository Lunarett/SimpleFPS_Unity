using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HumanBone
{
	[SerializeField] private Transform m_bone;
	[Range(0, 1)]
	[SerializeField] private float m_weight = 1;

	public Transform Bone { get => m_bone; }
	public float Weight { get => m_weight; }
}

public class BotWeaponIK : MonoBehaviour
{
	[SerializeField] private Transform m_fireLocation;
	[SerializeField] private int m_iterations = 10;
	[SerializeField] private float m_angleLimit = 90;
	[SerializeField] private float m_distanceLimit = 1.5f;

	[Range(0, 1)]
	[SerializeField] private float m_weight = 1;
	[SerializeField] private HumanBone[] m_humanBones;

	private Transform[] m_boneTransforms;
	private BotAgent m_agent;
	private Transform m_targetTransform;
	private Health m_health;
	private Animator m_anim;

	private void Awake()
	{
		m_health = GetComponent<Health>();
		m_agent = GetComponent<BotAgent>();
		m_anim = GetComponent<Animator>();

	}

	private void Start()
	{
		m_targetTransform = GameObject.FindGameObjectWithTag("RedTeam").transform;
		m_boneTransforms = new Transform[m_humanBones.Length];
		for (int i = 0; i < m_boneTransforms.Length; i++)
		{
			m_boneTransforms[i] = m_humanBones[i].Bone;

			if (m_boneTransforms[i] == null) Debug.LogError($"Failed to add transform at index {i}");
		}

		if (m_targetTransform == null) Debug.LogError("Target is missing");
		if (m_fireLocation == null) Debug.LogError("Fire Location is missing");
	}

	private void LateUpdate()
	{
		if (m_targetTransform == null) return;
		if (m_fireLocation == null) return;

		if (m_health.CurrentHealth > 0)
		{
			Vector3 targetPos = GetTargetPos();
			for (int a = 0; a < m_iterations; a++)
			{
				for (int b = 0; b < m_boneTransforms.Length; b++)
				{
					Transform bone = m_boneTransforms[b];
					float boneWeight = m_humanBones[b].Weight * m_weight;
					AimAtTarget(bone, targetPos, boneWeight);
				}
			}
		}
	}

	private void AimAtTarget(Transform bone, Vector3 tarPos, float weight)
	{
		Vector3 aimDir = m_fireLocation.forward;
		Vector3 targetDir = tarPos - m_fireLocation.position;

		Quaternion aimTowards = Quaternion.FromToRotation(aimDir, targetDir);
		Quaternion blendedRotation = Quaternion.Slerp(Quaternion.identity, aimTowards, weight);

		bone.rotation = blendedRotation * bone.rotation;
	}

	private Vector3 GetTargetPos()
	{
		Vector3 tarDir = m_targetTransform.position - m_fireLocation.position;
		Vector3 fireDir = m_fireLocation.forward;

		float blendOut = 0;

		float tarAngle = Vector3.Angle(tarDir, fireDir);
		if (tarAngle > m_angleLimit)
		{
			blendOut += (tarAngle - m_angleLimit) / 50;
		}

		float tarDist = tarDir.magnitude;
		if (tarDist < m_distanceLimit)
		{
			blendOut += m_distanceLimit - tarDist;
		}

		Vector3 dir = Vector3.Slerp(tarDir, fireDir, blendOut);
		return m_fireLocation.position + dir;
	}

	public void SetTargetTransform(Transform target)
	{
		m_targetTransform = target;
	}

	public void SetFireLocation(Transform pos)
	{
		m_fireLocation = pos;
	}
}
