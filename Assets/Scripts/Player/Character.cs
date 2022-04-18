using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : CharacterBehavior
{
	[SerializeField] private bool m_isPaused = false;
	[SerializeField] private WeaponBehavior m_weapon;

	private bool m_isFiring = false;

	public override bool IsPaused() => m_isPaused;

	protected override void Start()
	{
		SetPaused(false);
	}

	protected override void Update()
	{
		if(m_isFiring) m_weapon.Fire();
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

	public void OnPause(InputAction.CallbackContext context)
	{
		m_isPaused = !m_isPaused;
	}

	public void OnFire(InputAction.CallbackContext context)
	{
		switch(context.phase)
		{
			case InputActionPhase.Started:
				m_isFiring = true;
				break;
			case InputActionPhase.Canceled:
				m_isFiring = false;
				break;
		}
	}

	public void OnReload(InputAction.CallbackContext context)
	{
		if(context.phase == InputActionPhase.Performed) m_weapon.Reload();
	}
}
