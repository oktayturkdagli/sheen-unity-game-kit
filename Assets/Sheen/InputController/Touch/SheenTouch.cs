using UnityEngine;
using System.Collections;
using System;

public class SheenTouch : MonoBehaviour
{
    [SerializeField] bool useTouch = true;
    [SerializeField] [Range(0.01f, 1f)] float tapThreshold = 0.2f;
    string scriptableObjectName = "InputControllerSO";

    float[] timeTouchBegan;
    bool[] touchDidMove;
    float lastClickTime;

    //EVENTS
    public static event Action<int> OnFingerDown; //Gets fired when a finger begins touching the screen 
    public static event Action<int> OnFingerMoved; //Gets fired every frame a finger is touching the screen 											  
    public static event Action<int> OnFingerUp; //Gets fired when a finger stops touching the screen 
    public static event Action<int> OnFingerTap; //Gets fired when a finger taps the screen
    public static event Action<int> OnFingerDoubleTap; //Gets fired when a finger taps the screen 
    public static event Action<int> OnFingerSwipe; //Gets fired when a finger swipes the screen 

    void Start()
    {
        LoadValuesFromScriptableObject();
        timeTouchBegan = new float[10];
        touchDidMove = new bool[10];
    }

    private void Update()
    {
        Touches();
    }

    protected void OnEnable()
    {
        LoadValuesFromScriptableObject();
    }

    //private void OnValidate()
    //{
    //    SaveValuesToScriptableObject();
    //}

    public void Touches()
    {
        foreach (Touch touch in Input.touches)
        {
            int fingerIndex = touch.fingerId;

            if (touch.phase == TouchPhase.Began)
            {
                //Debug.Log("Finger #" + fingerIndex.ToString() + " entered!");
                timeTouchBegan[fingerIndex] = Time.time;
                touchDidMove[fingerIndex] = false;
                OnFingerDown?.Invoke(fingerIndex);
            }
            if (touch.phase == TouchPhase.Moved)
            {
                //Debug.Log("Finger #" + fingerIndex.ToString() + " moved!");
                touchDidMove[fingerIndex] = true;
                OnFingerMoved?.Invoke(fingerIndex);
            }
            if (touch.phase == TouchPhase.Ended)
            {
                float tapTime = Time.time - timeTouchBegan[fingerIndex];
                //Debug.Log("Finger #" + fingerIndex.ToString() + " left. Tap time: " + tapTime.ToString());
                OnFingerUp?.Invoke(fingerIndex);

                if (tapTime <= tapThreshold && touchDidMove[fingerIndex] == false)
                {
                    float timeSinceLastClick = Time.time - lastClickTime;
                    if (timeSinceLastClick <= tapThreshold)
                    {
                        //Debug.Log("Finger #" + fingerIndex.ToString() + "DOUBLE TAP DETECTED at: " + touch.position.ToString());
                        OnFingerDoubleTap?.Invoke(fingerIndex);
                    }
                    else
                    {
                        //Debug.Log("Finger #" + fingerIndex.ToString() + " TAP DETECTED at: " + touch.position.ToString());
                        OnFingerTap?.Invoke(fingerIndex);
                    }
                    lastClickTime = Time.time;
                }
            }
        }
    }
    
    public void LoadValuesFromScriptableObject()
    {
        InputControllerSOC existingSO = (InputControllerSOC)Resources.Load<InputControllerSOC>(scriptableObjectName);
        if (existingSO)
        {
            useTouch = existingSO.useTouch;
            tapThreshold = existingSO.tapThreshold;
        }
    }

    public void SaveValuesToScriptableObject()
    {
        InputControllerSOC existingSO = (InputControllerSOC)Resources.Load<InputControllerSOC>(scriptableObjectName);
        if (existingSO)
        {
            existingSO.useTouch = useTouch;
            existingSO.tapThreshold = tapThreshold;
        }
    }

}
