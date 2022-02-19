using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SheenTouch : MonoBehaviour
{   
    [SerializeField] public bool useTouch;
    [SerializeField] [Range(0.01f, 1f)] public float tapThreshold = 0.2f;
    [SerializeField] bool workOnHalfOfScreen; //Makes the touch work on only half of the screen
    float[] timeTouchBegan;
    bool[] touchDidMove;
    float lastClickTime;
    public string scriptableObjectName = "InputControllerSO";
    
    [SerializeField] public UnityEvent<int> OnFingerDown;
    [SerializeField] public UnityEvent<int> OnFingerMoved;
    [SerializeField] public UnityEvent<int> OnFingerUp;
    [SerializeField] public UnityEvent<int> OnFingerTap;
    [SerializeField] public UnityEvent<int> OnFingerDoubleTap;
    [SerializeField] public UnityEvent<int> OnFingerSwipe;
    [SerializeField] public UnityEvent<Vector2> OnFingerScreen; //TODO: More position options will be added and positions will be explained more accurately
    [SerializeField] public UnityEvent<Vector3> OnFingerWorld; //TODO: More position options will be added and positions will be explained more accurately
    
    private void Awake()
    {
        LoadValuesFromScriptableObject();
    }

    private void OnEnable()
    {
        LoadValuesFromScriptableObject();
    }
    
    void Start()
    {
        timeTouchBegan = new float[10];
        touchDidMove = new bool[10];
    }

    void Update()
    {
        if (useTouch)
        {
            RunInputLogic();
        }  
    }

    void RunInputLogic()
    {
        if (workOnHalfOfScreen && !IsItRightHalfOfScreen())
            return;

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
                OnFingerScreen.Invoke(Input.mousePosition);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit raycastHit))
                {
                    OnFingerWorld.Invoke(raycastHit.point);
                }
            }
            if (touch.phase == TouchPhase.Moved)
            {
                touchDidMove[fingerIndex] = true;
                OnFingerMoved.Invoke(fingerIndex);
                OnFingerScreen.Invoke(Input.mousePosition);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit raycastHit))
                {
                    OnFingerWorld.Invoke(raycastHit.point);
                }
            }
            if (touch.phase == TouchPhase.Ended)
            {
                float tapTime = Time.time - timeTouchBegan[fingerIndex];
                OnFingerUp.Invoke(fingerIndex);
                OnFingerScreen.Invoke(Input.mousePosition);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit raycastHit))
                {
                    OnFingerWorld.Invoke(raycastHit.point);
                }

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
                    OnFingerWorld.Invoke(raycastHit.point);
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

    bool IsItRightHalfOfScreen()
    {
        if (Input.mousePosition.x > Screen.width / 2)
        {
            return true;
        }
        else if (Input.mousePosition.x < Screen.width / 2)
        {
            return false;
        }

        return false;
    }

    public void LoadValuesFromScriptableObject()
    {
        InputControllerSO existingSO = (InputControllerSO)Resources.Load<InputControllerSO>(scriptableObjectName);
        if (existingSO)
        {
            useTouch = existingSO.useTouch;
            tapThreshold = existingSO.tapThreshold;
            workOnHalfOfScreen = existingSO.workOnHalfOfScreenTouch;
        }
    }

    public void SaveValuesToScriptableObject()
    {
        InputControllerSO existingSO = (InputControllerSO)Resources.Load<InputControllerSO>(scriptableObjectName);
        if (existingSO)
        {
            existingSO.useTouch = useTouch;
            existingSO.tapThreshold = tapThreshold;
            existingSO.workOnHalfOfScreenTouch = workOnHalfOfScreen;
            #if UNITY_EDITOR
            EditorUtility.SetDirty(existingSO); //Saves changes made to this file
            #endif
        }
    }
}

#if UNITY_EDITOR
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
    }
}
#endif