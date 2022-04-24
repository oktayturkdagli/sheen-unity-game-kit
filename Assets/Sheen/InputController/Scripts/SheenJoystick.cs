using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class SheenJoystick : MonoBehaviour
{
    [SerializeField] bool useJoystick; //Can the joystick component be used?
    [SerializeField] RectTransform center; //Outer circle
    [SerializeField] RectTransform knob; //Inner circle
    [SerializeField] float outRange; //Determines how far the knob can be from the center
    [SerializeField] bool fixedJoystick; //Joystick pins to a default point
    [SerializeField] bool alwaysDisplay; //If, false makes the joystick disappear from the screen when not in use
    [SerializeField] bool workOnHalfOfScreen; //Makes the joystick work on only half of the screen
    Vector2 direction;
    Vector2 fixedJoystickPosition;
    
    string scriptableObjectName = "InputControllerSO";

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

    public void AccessJoystick()
    {
        Transform joystick = transform.GetChild(1);
        if (joystick)
        {
            center = (RectTransform)joystick.GetChild(0).gameObject.transform;
            knob = (RectTransform)joystick.GetChild(0).GetChild(0).gameObject.transform;
        }
    }

    void DisplayJoystick(bool state)
    {
        center.gameObject.SetActive(state);
        knob.gameObject.SetActive(state);
    }

    void RunJoystickLogic()
    {
        if (workOnHalfOfScreen && !IsItLeftHalfOfScreen())
            return;

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

    bool IsItLeftHalfOfScreen()
    {
        if (Input.mousePosition.x < Screen.width / 2)
        {
            return true;
        }
        else if (Input.mousePosition.x > Screen.width / 2)
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
            useJoystick = existingSO.useJoystick;
            outRange = existingSO.joystickOutRange;
            center.GetComponent<Image>().sprite = existingSO.joystickCenterSprite;
            knob.GetComponent<Image>().sprite = existingSO.joystickKnobSprite;
            fixedJoystick = existingSO.fixedJoystick;
            alwaysDisplay = existingSO.alwaysDisplayJoystick;
            workOnHalfOfScreen = existingSO.workOnHalfOfScreenJoystick;
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
            existingSO.workOnHalfOfScreenJoystick = workOnHalfOfScreen;
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