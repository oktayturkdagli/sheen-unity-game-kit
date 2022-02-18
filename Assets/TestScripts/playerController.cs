using UnityEngine;

public class playerController : MonoBehaviour
{
	public void HandleJoystick(Vector2 direction)
	{
		transform.Translate(new Vector3(direction.x, 0, direction.y) * 10 * Time.deltaTime, Space.World);
	}
}
