using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : PlayerCharacterBehavior
{
	[SerializeField] private bool m_isPaused = false;
	[SerializeField] private WeaponBehavior m_weapon;

	private int m_playerID = 0;

	public override bool IsPaused() => m_isPaused;
	public override int GetCharacterID() => m_playerID;

	protected override void Awake()
	{
		m_weapon.SetOwnerObject(gameObject);
	}

	protected override void Start()
	{
		SetPaused(false);
	}

	public override void SetPaused(bool paused)
	{
		m_isPaused = paused;

		if(paused)
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.Confined;
		}
		else
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
	}

	public override void SetCharacterID(int id)
	{
		m_playerID = id;
	}

	public void OnPause(InputAction.CallbackContext context)
	{
		m_isPaused = !m_isPaused;
	}

	public void OnFire(InputAction.CallbackContext context)
	{
		switch(context.phase)
		{
			case InputActionPhase.Started:
				m_weapon.StartFire();
				break;
			case InputActionPhase.Canceled:
				m_weapon.StopFire();
				break;
		}
	}

	public void OnReload(InputAction.CallbackContext context)
	{
		if(context.phase == InputActionPhase.Performed) m_weapon.Reload();
	}
}
