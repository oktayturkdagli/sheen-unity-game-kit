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
}
