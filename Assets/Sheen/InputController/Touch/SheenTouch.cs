using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

public class SheenTouch : MonoBehaviour
{
    [SerializeField] bool useTouch;
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
    [SerializeField] public UnityEvent<Vector2> OnFingerScreen; //TODO: More position options will be added and positions will be explained more accurately
    [SerializeField] public UnityEvent<Vector3> OnFingerScreen3D; //TODO: More position options will be added and positions will be explained more accurately


    void Start()
    {
        LoadValuesFromScriptableObject();
        timeTouchBegan = new float[10];
        touchDidMove = new bool[10];
    }

    void Update()
    {
        if (useTouch)
        {
            Logic();
        }  
    }

    void OnEnable()
    {
        LoadValuesFromScriptableObject();
    }

    void Logic()
    {
        Touch();
        Mouse();
    }

    void Touch()
    {
        foreach (Touch touch in Input.touches)
        {
            int fingerIndex = touch.fingerId;

            if (touch.phase == TouchPhase.Began)
            {
                timeTouchBegan[fingerIndex] = Time.time;
                touchDidMove[fingerIndex] = false;
                OnFingerDown.Invoke(fingerIndex);
                OnFingerScreen.Invoke(Camera.main.ScreenToWorldPoint(touch.position));
            }
            if (touch.phase == TouchPhase.Moved)
            {
                touchDidMove[fingerIndex] = true;
                OnFingerMoved.Invoke(fingerIndex);
                OnFingerScreen.Invoke(Camera.main.ScreenToWorldPoint(touch.position));
            }
            if (touch.phase == TouchPhase.Ended)
            {
                float tapTime = Time.time - timeTouchBegan[fingerIndex];
                OnFingerUp.Invoke(fingerIndex);
                OnFingerScreen.Invoke(Camera.main.ScreenToWorldPoint(touch.position));

                if (tapTime <= tapThreshold && touchDidMove[fingerIndex] == false)
                {
                    float timeSinceLastClick = Time.time - lastClickTime;
                    if (timeSinceLastClick <= tapThreshold)
                    {
                        OnFingerDoubleTap.Invoke(fingerIndex);
                    }
                    else
                    {
                        OnFingerTap.Invoke(fingerIndex);
                    }
                    lastClickTime = Time.time;
                }
            }
        }
    }

    void Mouse()
    {
        if (Input.touchCount == 0)
        {
            int fingerIndex = 0;

            if (Input.GetMouseButtonDown(0))
            {
                timeTouchBegan[fingerIndex] = Time.time;
                touchDidMove[fingerIndex] = false;
                OnFingerDown.Invoke(fingerIndex);
                OnFingerScreen.Invoke(Input.mousePosition);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit raycastHit))
                {
                    OnFingerScreen3D.Invoke(raycastHit.point);
                }
                
            }
            if (Input.GetMouseButton(0))
            {
                if (Input.GetAxis("Mouse X") < 0 || Input.GetAxis("Mouse X") > 0)
                {
                    touchDidMove[fingerIndex] = true;
                    OnFingerMoved.Invoke(fingerIndex);  
                }
                //OnFingerScreen.Invoke(Input.mousePosition);
            }
            if (Input.GetMouseButtonUp(0))
            {
                float tapTime = Time.time - timeTouchBegan[fingerIndex];
                OnFingerUp.Invoke(fingerIndex);
                //OnFingerScreen.Invoke(Input.mousePosition);

                if (tapTime <= tapThreshold && touchDidMove[fingerIndex] == false)
                {
                    float timeSinceLastClick = Time.time - lastClickTime;
                    if (timeSinceLastClick <= tapThreshold)
                    {
                        OnFingerDoubleTap.Invoke(fingerIndex);
                    }
                    else
                    {
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
            EditorUtility.SetDirty(existingSO); //Saves changes made to this file
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
