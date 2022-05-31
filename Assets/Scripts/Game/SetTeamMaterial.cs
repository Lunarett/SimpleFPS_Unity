using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTeamMaterial : MonoBehaviour
{
	[SerializeField] private SkinnedMeshRenderer m_body;
	[Space]
	[SerializeField] private Material m_redTeammaterial;
	[SerializeField] private Material m_blueTeammaterial;

	private Health m_health;

	private void Awake()
	{
		m_health = GetComponent<Health>();
	}

	private void Start()
	{
		if (m_health != null) { SetMaterial(m_health.Team); }
	}

	public void SetMaterial(ETeams teams)
	{
		switch (teams)
		{
			case ETeams.RedTeam:
				m_body.material = m_redTeammaterial;
				break;
			case ETeams.BlueTeam:
				m_body.material = m_blueTeammaterial;
				break;
			default:
				break;
		}
	}
}
