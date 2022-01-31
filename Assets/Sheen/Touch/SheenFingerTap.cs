using UnityEngine;
using UnityEngine.Events;

namespace Sheen.Touch
{
	/// <summary>This component calls the OnFingerTap event when a finger taps the screen.</summary>
	public class SheenFingerTap : MonoBehaviour
	{
		[System.Serializable] public class SheenFingerEvent : UnityEvent<SheenFinger> {}
		[System.Serializable] public class Vector3Event : UnityEvent<Vector3> {}
		[System.Serializable] public class Vector2Event : UnityEvent<Vector2> {}
		[System.Serializable] public class IntEvent : UnityEvent<int> {}

		/// <summary>Ignore fingers with StartedOverGui?</summary>
        [SerializeField] private bool ignoreStartedOverGui = true;
		public bool IgnoreStartedOverGui { set { ignoreStartedOverGui = value; } get { return ignoreStartedOverGui; } }

		/// <summary>Ignore fingers with OverGui?</summary>
		[SerializeField] private bool ignoreIsOverGui;
		public bool IgnoreIsOverGui { set { ignoreIsOverGui = value; } get { return ignoreIsOverGui; } }


		/// <summary>How many times must this finger tap before OnTap gets called?
		/// 0 = Every time (keep in mind OnTap will only be called once if you use this).</summary>
		[SerializeField] private int requiredTapCount;
		public int RequiredTapCount { set { requiredTapCount = value; } get { return requiredTapCount; } }

		/// <summary>How many times repeating must this finger tap before OnTap gets called?
		/// 0 = Every time (e.g. a setting of 2 means OnTap will get called when you tap 2 times, 4 times, 6, 8, 10, etc).</summary>
		[SerializeField] private int requiredTapInterval;
		public int RequiredTapInterval { set { requiredTapInterval = value; } get { return requiredTapInterval; } }

		/// <summary>This event will be called if the above conditions are met when you tap the screen.</summary>
		[SerializeField] private SheenFingerEvent onFinger;
		public SheenFingerEvent OnFinger { get { if (onFinger == null) onFinger = new SheenFingerEvent(); return onFinger; } }

		/// <summary>This event will be called if the above conditions are met when you tap the screen.
		/// Int = The finger tap count.</summary>
		[SerializeField] private IntEvent onCount;
		public IntEvent OnCount { get { if (onCount == null) onCount = new IntEvent(); return onCount; } }

		/// <summary>This event will be called if the above conditions are met when you tap the screen.
		/// Vector3 = Finger position in world space.</summary>
		[SerializeField] private Vector3Event onWorld;
		public Vector3Event OnWorld { get { if (onWorld == null) onWorld = new Vector3Event(); return onWorld; } }

		/// <summary>This event will be called if the above conditions are met when you tap the screen.
		/// Vector2 = Finger position in screen space.</summary>
		[SerializeField] private Vector2Event onScreen;
		public Vector2Event OnScreen { get { if (onScreen == null) onScreen = new Vector2Event(); return onScreen; } } 

		protected virtual void Reset()
		{
			
		}

		protected virtual void Start()
		{
			
		}

		protected virtual void OnEnable()
		{
			SheenTouch.OnFingerTap += HandleFingerTap;
		}

		protected virtual void OnDisable()
		{
			SheenTouch.OnFingerTap -= HandleFingerTap;
		}

		private void HandleFingerTap(SheenFinger finger)
		{
			// Ignore?
			if (ignoreStartedOverGui == true && finger.StartedOverGui == true) return;

			if (ignoreIsOverGui == true && finger.IsOverGui == true) return;

			if (requiredTapCount > 0 && finger.TapCount != requiredTapCount) return;

			if (requiredTapInterval > 0 && (finger.TapCount % requiredTapInterval) != 0) return;

			if (onFinger != null)
				onFinger.Invoke(finger);

			if (onCount != null)
				onCount.Invoke(finger.TapCount);

			if (onWorld != null)
				onWorld.Invoke(Vector3.zero);

			if (onScreen != null)
				onScreen.Invoke(finger.ScreenPosition);
		}
	}
}
