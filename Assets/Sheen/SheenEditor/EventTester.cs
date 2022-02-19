#if UNITY_EDITOR
using UnityEngine;

public class EventTester : MonoBehaviour
{
	public void HandleFingerDown(int fingerIndex)
	{
		Debug.Log("Finger " + fingerIndex + " began touching the screen");
	}

	public void HandleFingerMoved(int fingerIndex)
	{
		Debug.Log("Finger " + fingerIndex + " is still touching the screen");
	}

	public void HandleFingerUp(int fingerIndex)
	{
		Debug.Log("Finger " + fingerIndex + " finished touching the screen");
	}

	public void HandleFingerTap(int fingerIndex)
	{
		Debug.Log("Finger " + fingerIndex + " tapped the screen");
	}

	public void HandleFingerDoubleTap(int fingerIndex)
	{
		Debug.Log("Finger " + fingerIndex + " double tapped the screen");
	}

	public void HandleFingerSwipe(int fingerIndex)
	{
		Debug.Log("Finger " + fingerIndex + " swiped the screen");
	}

	public void HandleFingerScreen(Vector2 position)
	{
		Debug.Log("Finger touch the screen x:" + position.x + " y:" + position.y);
	}

	public void HandleFingerWorld(Vector3 position)
	{
		Debug.Log("Finger touch the screen x:" + position.x + " y:" + position.y + "z:" + position.z);
	}

	public void HandleJoystick(Vector2 direction)
	{
		Debug.Log("Joystick moved x:" + direction.x + " y:" + direction.y);
	}
}
#endif