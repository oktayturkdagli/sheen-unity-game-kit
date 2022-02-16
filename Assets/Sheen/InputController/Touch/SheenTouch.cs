using UnityEngine;
using System.Collections;

public class SheenTouch : MonoBehaviour
{
    [SerializeField] bool useTouch = true;
    [SerializeField] [Range(0.01f, 1f)] float tapThreshold = 0.2f;
    string scriptableObjectName = "InputControllerSO";

    float[] timeTouchBegan;
    bool[] touchDidMove;
    float lastClickTime;

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
                Debug.Log("Finger #" + fingerIndex.ToString() + " entered!");
                timeTouchBegan[fingerIndex] = Time.time;
                touchDidMove[fingerIndex] = false;
            }
            if (touch.phase == TouchPhase.Moved)
            {
                Debug.Log("Finger #" + fingerIndex.ToString() + " moved!");
                touchDidMove[fingerIndex] = true;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                float tapTime = Time.time - timeTouchBegan[fingerIndex];
                Debug.Log("Finger #" + fingerIndex.ToString() + " left. Tap time: " + tapTime.ToString());

                if (tapTime <= tapThreshold && touchDidMove[fingerIndex] == false)
                {
                    float timeSinceLastClick = Time.time - lastClickTime;
                    if (timeSinceLastClick <= tapThreshold)
                    {
                        Debug.Log("Finger #" + fingerIndex.ToString() + "DOUBLE TAP DETECTED at: " + touch.position.ToString());
                    }
                    else
                    {
                        Debug.Log("Finger #" + fingerIndex.ToString() + " TAP DETECTED at: " + touch.position.ToString());
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
