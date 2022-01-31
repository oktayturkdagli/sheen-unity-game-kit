using UnityEngine;
using System.Collections.Generic;

namespace Sheen.Touch
{
	//This class stores information about a single touch (or simulated touch).
	public class SheenFinger
	{
		//VARIABLES
		#region
		//This is the hardware ID of the finger.
		/// NOTE: Simulated fingers will use hardware ID -1 and -2.
		public int Index;

		//This tells you how long this finger has been active (or inactive) in seconds.
		public float Age;

		//Is this finger currently touching the screen?
		public bool isTouching;

		//This tells you the 'isTouching' value of the last frame.
		public bool LastTouch;

		//Did this finger just tap the screen?
		public bool Tap;

		//This tells you how many times this finger has been tapped.
		public int TapCount;

		//Did this finger just swipe the screen?
		public bool Swipe;

		//If this finger has been inactive for more than TapThreshold, this will be true.
		public bool Expired;

		//If this finger has been touching the screen for more than TapThreshold, this will be true.
		public bool Old;

		//This tells you the 'ScreenPosition' value of this finger when it began touching the screen.
		public Vector2 StartScreenPosition;

		//This tells you the last screen position of the finger.
		public Vector2 LastScreenPosition;

		//This tells you the current screen position of the finger in pixels, where 0,0 = bottom left.
		public Vector2 ScreenPosition;

		//This tells you if the current finger had 'IsOverGui' set to true when it began touching the screen.
		public bool StartedOverGui;
		#endregion

		//FUNCTIONS
		#region
		//This will return true if this finger is currently touching the screen.
		public bool IsActive
		{
			get
			{
				return SheenTouch.fingers.Contains(this);
			}
		}

		//This will return true if the current finger is over any Unity GUI elements.
		public bool IsOverGui
		{
			get
			{
				return false;
			}
		}

		//Did this finger begin touching the screen this frame?
		public bool Down
		{
			get
			{
				return isTouching == true && LastTouch == false;
			}
		}

		//Did the finger stop touching the screen this frame?
		public bool Up
		{
			get
			{
				return isTouching == false && LastTouch == true;
			}
		}

		//This will return how far in pixels the finger has moved since the last frame.
		public Vector2 ScreenDelta
		{
			get
			{
				return ScreenPosition - LastScreenPosition;
			}
		}

		//This returns a resolution-independent 'ScreenDelta' value.
		public Vector2 ScaledDelta
		{
			get
			{
				return ScreenDelta * SheenTouch.ScalingFactor;
			}
		}

		//This tells you how far this finger has moved since it began touching the screen.
		public Vector2 SwipeScreenDelta
		{
			get
			{
				return ScreenPosition - StartScreenPosition;
			}
		}

		//This returns a resolution-independent 'SwipeScreenDelta' value.
		public Vector2 SwipeScaledDelta
		{
			get
			{
				return SwipeScreenDelta * SheenTouch.ScalingFactor;
			}
		}

		//This will return the ray of the finger's current position relative to the specified camera (none/null = Main Camera).
		public Ray GetRay(Camera camera = null)
		{
			// Make sure the camera exists
			camera = GetCamera(camera);

			if (camera != null)
			{
				return camera.ScreenPointToRay(ScreenPosition);
			}
			else
			{
				Debug.LogError("Failed to find camera. Either tag your cameras MainCamera, or set one in this component.");
			}

			return default(Ray);
		}

		//This will return the ray of the finger's start position relative to the specified camera (none/null = Main Camera).
		public Ray GetStartRay(Camera camera = null)
		{
			// Make sure the camera exists
			camera = GetCamera(camera);

			if (camera != null)
			{
				return camera.ScreenPointToRay(StartScreenPosition);
			}
			else
			{
				Debug.LogError("Failed to find camera. Either tag your cameras MainCamera, or set one in this component.");
			}

			return default(Ray);
		}

		//This will return the angle between the finger and the reference point, relative to the screen.
		public float GetRadians(Vector2 referencePoint)
		{
			return Mathf.Atan2(ScreenPosition.x - referencePoint.x, ScreenPosition.y - referencePoint.y);
		}

		//This will return the angle between the finger and the reference point, relative to the screen.
		public float GetDegrees(Vector2 referencePoint)
		{
			return GetRadians(referencePoint) * Mathf.Rad2Deg;
		}

		//This will return the angle between the last finger position and the reference point, relative to the screen.
		public float GetLastRadians(Vector2 referencePoint)
		{
			return Mathf.Atan2(LastScreenPosition.x - referencePoint.x, LastScreenPosition.y - referencePoint.y);
		}

		//This will return the angle between the last finger position and the reference point, relative to the screen.
		public float GetLastDegrees(Vector2 referencePoint)
		{
			return GetLastRadians(referencePoint) * Mathf.Rad2Deg;
		}

		//This will return the delta angle between the last and current finger position relative to the reference point.
		public float GetDeltaRadians(Vector2 referencePoint)
		{
			return GetDeltaRadians(referencePoint, referencePoint);
		}

		//This will return the delta angle between the last and current finger position relative to the reference point and the last reference point.
		public float GetDeltaRadians(Vector2 referencePoint, Vector2 lastReferencePoint)
		{
			var a = GetLastRadians(lastReferencePoint);
			var b = GetRadians(referencePoint);
			var d = Mathf.Repeat(a - b, Mathf.PI * 2.0f);

			if (d > Mathf.PI)
			{
				d -= Mathf.PI * 2.0f;
			}

			return d;
		}

		//This will return the delta angle between the last and current finger position relative to the reference point.
		public float GetDeltaDegrees(Vector2 referencePoint)
		{
			return GetDeltaRadians(referencePoint, referencePoint) * Mathf.Rad2Deg;
		}

		//This will return the delta angle between the last and current finger position relative to the reference point and the last reference point.
		public float GetDeltaDegrees(Vector2 referencePoint, Vector2 lastReferencePoint)
		{
			return GetDeltaRadians(referencePoint, lastReferencePoint) * Mathf.Rad2Deg;
		}

		//This will return the distance between the finger and the reference point.
		public float GetScreenDistance(Vector2 point)
		{
			return Vector2.Distance(ScreenPosition, point);
		}

		//This returns a resolution-independent 'GetScreenDistance' value.
		public float GetScaledDistance(Vector2 point)
		{
			return GetScreenDistance(point) * SheenTouch.ScalingFactor;
		}

		//This will return the distance between the last finger and the reference point.
		public float GetLastScreenDistance(Vector2 point)
		{
			return Vector2.Distance(LastScreenPosition, point);
		}

		//This returns a resolution-independent 'GetLastScreenDistance' value.
		public float GetLastScaledDistance(Vector2 point)
		{
			return GetLastScreenDistance(point) * SheenTouch.ScalingFactor;
		}

		//This will return the distance between the start finger and the reference point.
		public float GetStartScreenDistance(Vector2 point)
		{
			return Vector2.Distance(StartScreenPosition, point);
		}

		//This returns a resolution-independent 'GetStartScreenDistance' value.
		public float GetStartScaledDistance(Vector2 point)
		{
			return GetStartScreenDistance(point) * SheenTouch.ScalingFactor;
		}

		//This will return the start world position of this finger based on the distance from the camera.
		public Vector3 GetStartWorldPosition(float distance, Camera camera = null)
		{
			// Make sure the camera exists
			camera = GetCamera(camera);

			if (camera != null)
			{
				var point = new Vector3(StartScreenPosition.x, StartScreenPosition.y, distance);

				return camera.ScreenToWorldPoint(point);
			}
			else
			{
				Debug.LogError("Failed to find camera. Either tag your cameras MainCamera, or set one in this component.");
			}

			return default(Vector3);
		}

		//This will return the last world position of this finger based on the distance from the camera.
		public Vector3 GetLastWorldPosition(float distance, Camera camera = null)
		{
			// Make sure the camera exists
			camera = GetCamera(camera);

			if (camera != null)
			{
				var point = new Vector3(LastScreenPosition.x, LastScreenPosition.y, distance);

				return camera.ScreenToWorldPoint(point);
			}
			else
			{
				Debug.LogError("Failed to find camera. Either tag your cameras MainCamera, or set one in this component.");
			}

			return default(Vector3);
		}

		//This will return the world position of this finger based on the distance from the camera.
		public Vector3 GetWorldPosition(float distance, Camera camera = null)
		{
			// Make sure the camera exists
			camera = GetCamera(camera);

			if (camera != null)
			{
				var point = new Vector3(ScreenPosition.x, ScreenPosition.y, distance);

				return camera.ScreenToWorldPoint(point);
			}
			else
			{
				Debug.LogError("Failed to find camera. Either tag your cameras MainCamera, or set one in this component.");
			}

			return default(Vector3);
		}

		//This will return the change in world position of this finger based on the distance from the camera.
		public Vector3 GetWorldDelta(float distance, Camera camera = null)
		{
			return GetWorldDelta(distance, distance, camera);
		}

		//This will return the change in world position of this finger based on the last and current distance from the camera.
		public Vector3 GetWorldDelta(float lastDistance, float distance, Camera camera = null)
		{
			// Make sure the camera exists
			camera = GetCamera(camera);

			if (camera != null)
			{
				return GetWorldPosition(distance, camera) - GetLastWorldPosition(lastDistance, camera);
			}
			else
			{
				Debug.LogError("Failed to find camera. Either tag your cameras MainCamera, or set one in this component.");
			}

			return default(Vector3);
		}
		#endregion

		//If currentCamera is null, this will return the camera attached to gameObject, or return Camera.main
		public Camera GetCamera(Camera currentCamera, GameObject gameObject = null)
		{
			if (currentCamera == null)
			{
				if (gameObject != null)
				{
					currentCamera = gameObject.GetComponent<Camera>();
				}

				if (currentCamera == null)
				{
					currentCamera = Camera.main;
				}
			}

			return currentCamera;
		}
	}
}