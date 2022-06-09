using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatHolder : StatHolderBehavior
{
	private int m_kills;
	private int m_deaths;
	private bool m_isHoldingFlag;

	public override void AddKill()
	{
		m_kills++;
	}

	public override void AddDeath()
	{
		m_deaths++;
	}

	public override void SetIsHoldingFlag(bool isHolding)
	{
		m_isHoldingFlag = isHolding;
	}

	public override int GetKills()
	{
		return m_kills;
	}

	public override int GetDeaths()
	{
		return m_deaths;
	}

	public override bool GetIsHoldingFlag()
	{
		return m_isHoldingFlag;
	}
}
