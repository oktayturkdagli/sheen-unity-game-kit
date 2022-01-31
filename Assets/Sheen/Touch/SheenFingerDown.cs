using UnityEngine;
using UnityEngine.Events;

namespace Sheen.Touch
{
	//This component invokes events when a finger touches the screen that satisfies the specified conditions.
	public class SheenFingerDown : MonoBehaviour
	{
		[System.Serializable] public class SheenFingerEvent : UnityEvent<SheenFinger> { }
		[System.Serializable] public class Vector3Event : UnityEvent<Vector3> { }
		[System.Serializable] public class Vector2Event : UnityEvent<Vector2> { }


		[System.Flags]
		public enum ButtonTypes
		{
			LeftMouse = 1 << 0,
			RightMouse = 1 << 1,
			MiddleMouse = 1 << 2,
			Touch = 1 << 5
		}

		
		//Ignore fingers with StartedOverGui?
		[SerializeField] private bool ignoreStartedOverGui = true;
		public bool IgnoreStartedOverGui { set { ignoreStartedOverGui = value; } get { return ignoreStartedOverGui; } }
		
		//Which inputs should this component react to?
		[SerializeField] private ButtonTypes requiredButtons = (ButtonTypes)~0;
		public ButtonTypes RequiredButtons { set { requiredButtons = value; } get { return requiredButtons; } }
		
		//This event will be called if the above conditions are met when your finger begins touching the screen.
		[SerializeField] private SheenFingerEvent onFinger;
		public SheenFingerEvent OnFinger { get { if (onFinger == null) onFinger = new SheenFingerEvent(); return onFinger; } }
		
		//The method used to find world coordinates from a finger. See SheenScreenDepth documentation for more information.
		//public SheenScreenDepth ScreenDepth = new SheenScreenDepth(SheenScreenDepth.ConversionType.DepthIntercept);

		//This event will be called if the above conditions are met when your finger begins touching the screen.
		//Vector3 = Start point based on the ScreenDepth settings.
		[SerializeField] private Vector3Event onWorld;
		public Vector3Event OnWorld { get { if (onWorld == null) onWorld = new Vector3Event(); return onWorld; } }
		
		//This event will be called if the above conditions are met when your finger begins touching the screen.
		//Vector2 = Finger position in screen space.
		[SerializeField] private Vector2Event onScreen;
		public Vector2Event OnScreen { get { if (onScreen == null) onScreen = new Vector2Event(); return onScreen; } }


		protected virtual void OnEnable()
		{
			SheenTouch.OnFingerDown += HandleFingerDown;
		}

		protected virtual void OnDisable()
		{
			SheenTouch.OnFingerDown -= HandleFingerDown;
		}

		protected virtual void HandleFingerDown(SheenFinger finger)
        {
			if (ignoreStartedOverGui == true && finger.IsOverGui == true) return;

			if (RequiredButtonPressed(finger) == false) return;

			if (onFinger != null)
				onFinger.Invoke(finger);

			if (onWorld != null)
				onWorld.Invoke(Vector3.zero);

			if (onScreen != null)
				onScreen.Invoke(finger.ScreenPosition);
		}

		private bool RequiredButtonPressed(SheenFinger finger)
		{
			if (finger.Index < 0)
			{
				if (SheenInput.GetMouseExists() == true)
				{
					return ((requiredButtons & ButtonTypes.LeftMouse) != 0 && SheenInput.GetMousePressed(0) == true) ||
						((requiredButtons & ButtonTypes.RightMouse) != 0 && SheenInput.GetMousePressed(1) == true)   ||
						((requiredButtons & ButtonTypes.MiddleMouse) != 0 && SheenInput.GetMousePressed(2) == true);
				}
			}
			else if ((requiredButtons & ButtonTypes.Touch) != 0)
			{
				return true;
			}

			return false;
		}
	}
}
