using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class SheenJoystick : MonoBehaviour
{
    [SerializeField] public bool useJoystick; //Can the joystick component be used?
    [SerializeField] public RectTransform center; //Outer circle
    [SerializeField] public RectTransform knob; //Inner circle
    [SerializeField] public float outRange; //Determines how far the knob can be from the center
    [SerializeField] public bool fixedJoystick; //Joystick pins to a default point
    [SerializeField] public bool alwaysDisplay; //If, false makes the joystick disappear from the screen when not in use
    public Vector2 direction;
    public Vector2 fixedJoystickPosition;
    public string scriptableObjectName = "InputControllerSO";

    [SerializeField] public UnityEvent<Vector2> OnJoystick;

    private void Awake()
    {
        AccessJoystick();
        LoadValuesFromScriptableObject();
    }

    private void OnEnable()
    {
        AccessJoystick();
        LoadValuesFromScriptableObject();
    }

    void Start()
    {
        fixedJoystickPosition = center.position;
        if (useJoystick)
        {
            DisplayJoystick(alwaysDisplay);
        }
        else
        {
            DisplayJoystick(false);
        }
    }
       
    void Update()
    {
        if (useJoystick)
        {
            RunJoystickLogic();
        } 
    }

    void RunJoystickLogic()
    {
        Vector2 touchPosition = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            DisplayJoystick(true);
            if (fixedJoystick)
            {
                knob.position = fixedJoystickPosition;
                center.position = fixedJoystickPosition;
            }
            else
            {
                knob.position = touchPosition;
                center.position = touchPosition;
            }
        }
        else if (Input.GetMouseButton(0))
        {
            knob.position = touchPosition;
            knob.position = center.position + Vector3.ClampMagnitude(knob.position - center.position, center.sizeDelta.x * outRange);
            direction = (knob.position - center.position).normalized;
            OnJoystick.Invoke(direction);
        }
        else
        {
            if (!alwaysDisplay)
                DisplayJoystick(false);
            knob.position = center.position;
            direction = Vector2.zero;
            OnJoystick.Invoke(direction);
        }

    }

    void DisplayJoystick(bool state)
    {
        center.gameObject.SetActive(state);
        knob.gameObject.SetActive(state);
    }

    public void AccessJoystick()
    {
        Transform joystick = transform.GetChild(1);
        if (joystick)
        {
            center = (RectTransform)joystick.GetChild(0).gameObject.transform;
            knob = (RectTransform)joystick.GetChild(0).GetChild(0).gameObject.transform;
        }
    }

    public void LoadValuesFromScriptableObject()
    {
        InputControllerSO existingSO = (InputControllerSO)Resources.Load<InputControllerSO>(scriptableObjectName);
        if (existingSO)
        {
            useJoystick = existingSO.useJoystick;
            outRange = existingSO.joystickOutRange;
            center.GetComponent<Image>().sprite = existingSO.joystickCenterSprite;
            knob.GetComponent<Image>().sprite = existingSO.joystickKnobSprite;
            fixedJoystick = existingSO.fixedJoystick;
            alwaysDisplay = existingSO.alwaysDisplayJoystick;
        }
    }

    public void SaveValuesToScriptableObject()
    {
        InputControllerSO existingSO = (InputControllerSO)Resources.Load<InputControllerSO>(scriptableObjectName);
        if (existingSO)
        {
            existingSO.useJoystick = useJoystick;
            existingSO.joystickOutRange = outRange;
            existingSO.joystickCenterSprite = center.GetComponent<Image>().sprite;
            existingSO.joystickKnobSprite = knob.GetComponent<Image>().sprite;
            existingSO.fixedJoystick = fixedJoystick;
            existingSO.alwaysDisplayJoystick = alwaysDisplay;
            #if UNITY_EDITOR
            EditorUtility.SetDirty(existingSO); //Saves changes made to this file
            #endif
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SheenJoystick))]
public class SheenJoystickEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUILayout.Space(5);
        SheenJoystick sheenJoystickScript = (SheenJoystick)target;
        if (GUILayout.Button("Save"))
        {
            sheenJoystickScript.SaveValuesToScriptableObject();
        }
        GUILayout.Space(10);
    } 
}
#endif