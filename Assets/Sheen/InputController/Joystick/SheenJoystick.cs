using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;


public class SheenJoystick : MonoBehaviour
{
    [SerializeField] bool useJoystick = true; //Can the joystick component be used?
    [SerializeField] private Transform character; //The character to use the joystick
    [SerializeField] private RectTransform center; //Outer circle
    [SerializeField] private RectTransform knob; //Inner circle
    [SerializeField] private float outRange; //Determines how far the knob can be from the center
    [SerializeField] private bool fixedJoystick; //Joystick pins to a default point
    [SerializeField] private bool alwaysDisplay; //If, false makes the joystick disappear from the screen when not in use

    
    private Vector2 direction;
    Vector2 fixedJoystickPosition;
    string scriptableObjectName = "InputControllerSO";

    void Start()
    {
        LoadValuesFromScriptableObject();
        ShowJoystick(alwaysDisplay);
        fixedJoystickPosition = center.position;
    }
       
    void Update()
    {
        Logic();
    }

    protected void OnEnable()
    {
        LoadValuesFromScriptableObject();
    }

    void Logic()
    {
        Vector2 touchPosition = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            ShowJoystick(true);
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
        }
        else
        {
            if (!alwaysDisplay)
                ShowJoystick(false);
            direction = Vector2.zero;
            knob.position = center.position;
        }

    }

    void CharacterLogic()
    {
        
    }

    void ShowJoystick(bool state)
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
