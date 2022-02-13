using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sheen.Touch
{
    [ExecuteInEditMode]
    [DefaultExecutionOrder(-100)]
    [DisallowMultipleComponent]
    public class SheenTouch : MonoBehaviour
    {
        //VARIABLES
        private const int DEFAULT_REFERENCE_DPI = 200;
        private const float DEFAULT_TAP_THRESHOLD = 0.2f;
        private const float DEFAULT_SWIPE_THRESHOLD = 100.0f;
        public static SheenTouch instance; //Contains all the active and enabled SheenTouch instances
        public static List<SheenFinger> fingers = new List<SheenFinger>(10); //Contains all fingers currently touching the screen (or have just stopped touching the screen)
        public static List<SheenFinger> inactiveFingers = new List<SheenFinger>(10); //contains all fingers that were once touching the screen. This is used to manage finger tapping
        string scriptableObjectName = "InputControllerSO";

        //INSTANCE, TRESHOLDS, DEFAULT DPI, DEFAULT GUI LAYER
        //The First active and enabled SheenTouch instance
        public static SheenTouch Instance { get { return instance; } }

        //Allows you to set the default DPI you want the input scaling to be based on
        [SerializeField] private float referenceDpi = DEFAULT_REFERENCE_DPI;
        public float ReferenceDpi { set { referenceDpi = value; } get { return referenceDpi; } }
        public static float CurrentReferenceDpi { get { return instance != null ? instance.referenceDpi : DEFAULT_REFERENCE_DPI; } }

        //Allows you to set how many seconds are required between a finger down/up for a tap to be registered
        [SerializeField] private float tapThreshold = DEFAULT_TAP_THRESHOLD;
        public float TapThreshold { set { tapThreshold = value; } get { return tapThreshold; } }
        public static float CurrentTapThreshold { get { return instance != null ? instance.tapThreshold : DEFAULT_TAP_THRESHOLD; } }

        //Allows you to set how many pixels of movement (relative to the ReferenceDpi) are required within the TapThreshold for a swipe to be triggered
        [SerializeField] private float swipeThreshold = DEFAULT_SWIPE_THRESHOLD;
        public float SwipeThreshold { set { swipeThreshold = value; } get { return swipeThreshold; } }
        public static float CurrentSwipeThreshold { get { return instance != null ? instance.swipeThreshold : DEFAULT_SWIPE_THRESHOLD; } }
        
        //If you disable this then Sheen touch will act as if you stopped touching the screen.
        [SerializeField] private bool useTouch = true;
        public bool UseTouch { set { useTouch = value; } get { return useTouch; } }

        //Should any mouse button press be stored as a finger?
        //NOTE: It will be given a finger <b>Index</b> of MOUSE_FINGER_INDEX = -1.
        [SerializeField] private bool useMouse = true;
        public bool UseMouse { set { useMouse = value; } get { return useMouse; } }

        //If you multiply this value with any other pixel delta (e.g. ScreenDelta), then it will become device resolution independent relative to the device DPI
        public static float ScalingFactor
        {
            get
            {
                var dpi = Screen.dpi; //Get the current screen DPI
                if (dpi <= 0) { return 1.0f; } //If it's 0 or less, it's invalid, so return the default scale of 1.0
                return CurrentReferenceDpi / dpi; //DPI seems valid, so scale it against the reference DPI
            }
        }

        //If you multiply this value with any other pixel delta (e.g. ScreenDelta), then it will become device resolution independent relative to the screen pixel size
        public static float ScreenFactor
        {
            get
            {
                var size = Mathf.Min(Screen.width, Screen.height); //Get shortest size
                if (size <= 0) { return 1.0f; } //If it's 0 or less, it's invalid, so return the default scale of 1.0
                return 1.0f / size; //Return reciprocal for easy multiplication
            }
        }


        //EVENTS
        public static event Action<SheenFinger> OnFingerDown; //Gets fired when a finger begins touching the screen (SheenFinger = The current finger)
        public static event Action<SheenFinger> OnFingerUpdate; //Gets fired every frame a finger is touching the screen (SheenFinger = The current finger)													  
        public static event Action<SheenFinger> OnFingerUp; //Gets fired when a finger stops touching the screen (SheenFinger = The current finger)
        public static event Action<SheenFinger> OnFingerTap; //Gets fired when a finger taps the screen (this is when a finger begins and stops touching the screen within the 'TAPTRESHOLD' time).
        public static event Action<SheenFinger> OnFingerSwipe; //Gets fired when a finger swipes the screen (this is when a finger begins and stops touching the screen within the 'TAPTRESHOLD' time, and also moves more than the 'SwipeThreshold' distance) (SheenFinger = The current finger)
        public static event Action<SheenFinger> OnFingerInactive; //Gets fired the frame after a finger went up
        public static event Action<SheenFinger> OnFingerExpired; //Gets fired after a finger has been touching the screen for longer than TAPTRESHOLD seconds, making it ineligible for a swipe
        public static event System.Action<SheenFinger> OnFingerOld;

        //FUNCTIONS
        protected virtual void OnEnable()
        {
            instance = this;
            LoadValuesFromScriptableObject(scriptableObjectName);
        }

        protected virtual void OnDisable()
        {
            instance = null;
        }

        protected virtual void Update()
        {
            if (instance == this)
            {
                UpdateFingers(Time.unscaledDeltaTime);
            }
        }

        private void UpdateFingers(float deltaTime)
        {
            ResetFinger();
            TakeInputs(deltaTime);
            UpdateEvents();
        }

        private void ResetFinger()
        {
            //When finger up, reset finger
            for (int i = 0; i < fingers.Count; i++)
            {
                var finger = fingers[i];
                if (finger.Up)
                {
                    fingers.Remove(finger);
                    i--;
                }

                if (finger.Down)
                {
                    finger.LastTouch = finger.isTouching;
                    finger.isTouching = true;
                    finger.Tap = false;
                    finger.Swipe = false;
                    finger.LastScreenPosition = finger.ScreenPosition;
                    finger.ScreenPosition = Vector2.zero;
                }
  
            }
        }

        private void TakeInputs(float deltaTime)
        {
            if (SheenInput.GetTouchCount() > 0)
            {
                for (var i = 0; i < SheenInput.GetTouchCount(); i++)
                {
                    var touch = SheenInput.GetTouch(i);
                    var finger = FindFinger(touch.fingerId);
                    if (finger == null) finger = CreateFinger(touch);
                    UpdateFinger(touch, deltaTime, finger);
                    CheckTapOrSwipe(finger);
                }
            }
        }
        
        //Update events of all Fingers
        private void UpdateEvents()
        {
            var fingerCount = fingers.Count;

            if (fingerCount > 0)
            {
                for (var i = 0; i < fingerCount; i++)
                {
                    var finger = fingers[i];
                    if (finger.Tap == true) OnFingerTap?.Invoke(finger);
                    if (finger.Swipe == true) OnFingerSwipe?.Invoke(finger);
                    if (finger.Down == true) OnFingerDown?.Invoke(finger);
                    OnFingerUpdate?.Invoke(finger);
                    if (finger.Up == true) OnFingerUp?.Invoke(finger);
                }

            }
        }

        // Find the finger with the specified index, or return null
        private SheenFinger FindFinger(int index)
        {
            foreach (var finger in fingers)
            {
                if (finger.Index == index) return finger;
            }

            return null;
        }

        public SheenFinger CreateFinger(UnityEngine.Touch touch)
        {
            var finger = new SheenFinger();
            fingers.Add(finger);
            finger.Index = touch.fingerId;
            finger.StartScreenPosition = touch.position;
            finger.LastScreenPosition = touch.position;
            finger.isTouching = false;
            
            return finger;
        }

        public void UpdateFinger(UnityEngine.Touch touch, float deltaTime, SheenFinger finger)
        {
            finger.LastTouch = finger.isTouching;
            finger.isTouching = (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved);
            finger.ScreenPosition = touch.position;
            finger.Age += deltaTime;
        }

        private void CheckTapOrSwipe(SheenFinger finger)
        {
            if (finger.Up)
            {
                if (finger.Age <= tapThreshold)
                {
                    var distance = finger.SwipeScreenDelta.magnitude * ScalingFactor;
                    if (distance < swipeThreshold)
                    {
                        finger.TapCount += 1;
                        finger.Tap = true;
                    }
                    else
                    {
                        finger.TapCount = 0;
                        finger.Swipe = true;
                    }
                }
            }
        }

        void LoadValuesFromScriptableObject(string soName)
        {
            InputControllerSOC existingSO = (InputControllerSOC)Resources.Load<InputControllerSOC>(soName);
            if (existingSO)
            {
                referenceDpi = existingSO.referenceDpi;
                tapThreshold = existingSO.tapThreshold;
                swipeThreshold = existingSO.swipeThreshold;
            }
        }
    }


}

