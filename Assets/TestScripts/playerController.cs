using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
	protected virtual void OnEnable()
	{
		SheenJoystick.OnJoystick += HandleJoystick;
	}

	protected virtual void OnDisable()
	{
		SheenJoystick.OnJoystick += HandleJoystick;
	}

	public void HandleJoystick(Vector2 direction)
	{
		transform.Translate(new Vector3(direction.x, 0, direction.y) * 10 * Time.deltaTime, Space.World);
	}
}
