using UnityEngine;

namespace Sheen.Touch
{
    //This class wraps Input and InputSystem so they can both be used from the same interface.
    public static class SheenInput
	{
		public static int GetTouchCount()
		{
			return Input.touchCount;
		}

		public static UnityEngine.Touch GetTouch(int index = 0)
		{
			return Input.GetTouch(index);
		}

		public static Vector2 GetMousePosition()
		{
			return Input.mousePosition;
		}

		public static bool GetDown(KeyCode oldKey)
		{
			return Input.GetKeyDown(oldKey);
		}

		public static bool GetPressed(KeyCode oldKey)
		{
			return Input.GetKey(oldKey);
		}

		public static bool GetUp(KeyCode oldKey)
		{
			return Input.GetKeyUp(oldKey);
		}

		public static bool GetMouseDown(int index)
		{
			return Input.GetMouseButtonDown(index);
		}

		public static bool GetMousePressed(int index)
		{
			return Input.GetMouseButton(index);
		}

		public static bool GetMouseUp(int index)
		{
			return Input.GetMouseButtonUp(index);
		}

		public static float GetMouseWheelDelta()
		{
			return Input.mouseScrollDelta.y;
		}

		public static bool GetMouseExists()
		{
			return Input.mousePresent;
		}

		public static bool GetKeyboardExists()
		{
			return true;
		}
	}
}