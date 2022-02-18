using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

public class SheenTouch : MonoBehaviour
{
    [SerializeField] bool useTouch = true;
    [SerializeField] [Range(0.01f, 1f)] float tapThreshold = 0.2f;
    string scriptableObjectName = "InputControllerSO";

    float[] timeTouchBegan;
    bool[] touchDidMove;
    float lastClickTime;

    //EVENTS
    [SerializeField] public UnityEvent<int> OnFingerDown;
    [SerializeField] public UnityEvent<int> OnFingerMoved;
    [SerializeField] public UnityEvent<int> OnFingerUp;
    [SerializeField] public UnityEvent<int> OnFingerTap;
    [SerializeField] public UnityEvent<int> OnFingerDoubleTap;
    [SerializeField] public UnityEvent<int> OnFingerSwipe;

    void Start()
    {
        LoadValuesFromScriptableObject();
        timeTouchBegan = new float[10];
        touchDidMove = new bool[10];
    }

    private void Update()
    {
        Logic();
    }

    protected void OnEnable()
    {
        LoadValuesFromScriptableObject();
    }

    public void Logic()
    {
        foreach (Touch touch in Input.touches)
        {
            int fingerIndex = touch.fingerId;

            if (touch.phase == TouchPhase.Began)
            {
                //Debug.Log("Finger #" + fingerIndex.ToString() + " entered!");
                timeTouchBegan[fingerIndex] = Time.time;
                touchDidMove[fingerIndex] = false;
                OnFingerDown.Invoke(fingerIndex);
            }
            if (touch.phase == TouchPhase.Moved)
            {
                //Debug.Log("Finger #" + fingerIndex.ToString() + " moved!");
                touchDidMove[fingerIndex] = true;
                OnFingerMoved.Invoke(fingerIndex);
            }
            if (touch.phase == TouchPhase.Ended)
            {
                float tapTime = Time.time - timeTouchBegan[fingerIndex];
                //Debug.Log("Finger #" + fingerIndex.ToString() + " left. Tap time: " + tapTime.ToString());
                OnFingerUp.Invoke(fingerIndex);

                if (tapTime <= tapThreshold && touchDidMove[fingerIndex] == false)
                {
                    float timeSinceLastClick = Time.time - lastClickTime;
                    if (timeSinceLastClick <= tapThreshold)
                    {
                        //Debug.Log("Finger #" + fingerIndex.ToString() + "DOUBLE TAP DETECTED at: " + touch.position.ToString());
                        OnFingerDoubleTap.Invoke(fingerIndex);
                    }
                    else
                    {
                        //Debug.Log("Finger #" + fingerIndex.ToString() + " TAP DETECTED at: " + touch.position.ToString());
                        OnFingerTap.Invoke(fingerIndex);
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


[CustomEditor(typeof(SheenTouch))]
public class SheenTouchEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUILayout.Space(5);
        SheenTouch sheenTouchScript = (SheenTouch)target;
        if (GUILayout.Button("Save"))
        {
            sheenTouchScript.SaveValuesToScriptableObject();
        }
        GUILayout.Space(10);
        //EditorGUILayout.HelpBox("You must save your changes for them to take effect.", MessageType.Info);
    }
}
