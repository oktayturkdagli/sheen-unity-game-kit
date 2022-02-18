using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Events;

public class SheenJoystick : MonoBehaviour
{
    [SerializeField] bool useJoystick; //Can the joystick component be used?
    [SerializeField] RectTransform center; //Outer circle
    [SerializeField] RectTransform knob; //Inner circle
    [SerializeField] float outRange; //Determines how far the knob can be from the center
    [SerializeField] bool fixedJoystick; //Joystick pins to a default point
    [SerializeField] bool alwaysDisplay; //If, false makes the joystick disappear from the screen when not in use
    Vector2 direction;
    Vector2 fixedJoystickPosition;
    string scriptableObjectName = "InputControllerSO";

    [SerializeField] public UnityEvent<Vector2> OnJoystick;

    void Start()
    {
        LoadValuesFromScriptableObject();
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
            Logic();
        } 
    }

    void OnEnable()
    {
        LoadValuesFromScriptableObject();
    }

    void Logic()
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

    void AccessJoystick()
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
        AccessJoystick();
        InputControllerSOC existingSO = (InputControllerSOC)Resources.Load<InputControllerSOC>(scriptableObjectName);
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
        InputControllerSOC existingSO = (InputControllerSOC)Resources.Load<InputControllerSOC>(scriptableObjectName);
        if (existingSO)
        {
            existingSO.useJoystick = useJoystick;
            existingSO.joystickOutRange = outRange;
            existingSO.joystickCenterSprite = center.GetComponent<Image>().sprite;
            existingSO.joystickKnobSprite = knob.GetComponent<Image>().sprite;
            existingSO.fixedJoystick = fixedJoystick;
            existingSO.alwaysDisplayJoystick = alwaysDisplay;
            EditorUtility.SetDirty(existingSO); //Saves changes made to this file
        }
    }

}

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
        //EditorGUILayout.HelpBox("You must save your changes for them to take effect.", MessageType.Info);
    }
}
