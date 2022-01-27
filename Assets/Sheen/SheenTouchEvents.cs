using UnityEngine;
using System.Collections.Generic;

namespace Sheen.Touch
{
	//This component will hook into every SheenTouch event, and spam the console with the information
	public class SheenTouchEvents : MonoBehaviour
	{
		protected virtual void OnEnable()
		{
			// Hook into the events we need
			SheenTouch.OnFingerDown += HandleFingerDown;
			SheenTouch.OnFingerUpdate += HandleFingerUpdate;
			SheenTouch.OnFingerUp += HandleFingerUp;
			SheenTouch.OnFingerTap += HandleFingerTap;
			SheenTouch.OnFingerSwipe += HandleFingerSwipe;
		}

		protected virtual void OnDisable()
		{
			// Unhook the events
			SheenTouch.OnFingerDown -= HandleFingerDown;
			SheenTouch.OnFingerUpdate -= HandleFingerUpdate;
			SheenTouch.OnFingerUp -= HandleFingerUp;
			SheenTouch.OnFingerTap -= HandleFingerTap;
			SheenTouch.OnFingerSwipe -= HandleFingerSwipe;
		}

		public void HandleFingerDown(SheenFinger finger)
		{
			Debug.Log("Finger " + finger.Index + " began touching the screen");
		}

		public void HandleFingerUpdate(SheenFinger finger)
		{
			Debug.Log("Finger " + finger.Index + " is still touching the screen");
		}

		public void HandleFingerUp(SheenFinger finger)
		{
			Debug.Log("Finger " + finger.Index + " finished touching the screen");
		}

		public void HandleFingerTap(SheenFinger finger)
		{
			Debug.Log("Finger " + finger.Index + " tapped the screen");
		}

		public void HandleFingerSwipe(SheenFinger finger)
		{
			Debug.Log("Finger " + finger.Index + " swiped the screen");
		}

		public void HandleGesture(List<SheenFinger> fingers)
		{
			Debug.Log("Gesture with " + fingers.Count + " finger(s)");
			//Debug.Log("    pinch scale: " + SheenGesture.GetPinchScale(fingers));
			//Debug.Log("    twist degrees: " + SheenGesture.GetTwistDegrees(fingers));
			//Debug.Log("    twist radians: " + SheenGesture.GetTwistRadians(fingers));
			//Debug.Log("    screen delta: " + SheenGesture.GetScreenDelta(fingers));
		}
	}
}