using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class SheenInputController : EditorWindow
{
    int selectedToolbarIndex = 0;
    Texture[] standartTextures, toolbarTextures, touchpadTextures;
    float referenceDPI = 200f, tapTreshold = 0.2f, swipeTreshold = 100f;
    
    bool joystickEnable = false;
    float JoystickOutRange = 2;

    bool optionalSettingsEnable = false;
    bool optionalToggle1 = true;
    string optionalTextField1 = "Optional text";

    [MenuItem("Window/Sheen/Sheen Input Controller")]
    public static void Init()
    {
        GetWindow(typeof(SheenInputController));
    }

    void OnGUI()
    {
        TakeTextures();
        selectedToolbarIndex = GUILayout.Toolbar(selectedToolbarIndex, toolbarTextures);
        switch (selectedToolbarIndex)
        {
            default:
            case 0:
                ControlTouchPad();
                break;
            case 1:
                ControlKeyboard();
                break;
            case 2:
                ControlGamePad();
                break;
        }
    }

    void ControlTouchPad()
    {
        //Standart
        EditorGUILayout.Space();
        GUILayout.Label("Touch Controller", EditorStyles.boldLabel);
        referenceDPI = EditorGUILayout.FloatField("Reference DPI", referenceDPI);
        tapTreshold = EditorGUILayout.FloatField("Tap Treshold", tapTreshold);
        swipeTreshold = EditorGUILayout.FloatField("Swipe Treshold", swipeTreshold);
        EditorGUILayout.Space(); EditorGUILayout.Space();

        //Joystick
        joystickEnable = EditorGUILayout.BeginToggleGroup("Joystick", joystickEnable);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUILayout.Height(65));GUILayout.FlexibleSpace();
        bool button1 = GUILayout.Button(touchpadTextures[0], GUILayout.Width(25), GUILayout.Height(25));
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        GUILayout.Box(" Center ");
        GUILayout.Box(touchpadTextures[2], GUILayout.Width(50), GUILayout.Height(50));
        GUILayout.EndVertical();
        GUILayout.BeginVertical(GUILayout.Height(65)); GUILayout.FlexibleSpace();
        bool button2 = GUILayout.Button(touchpadTextures[1], GUILayout.Width(25), GUILayout.Height(25));
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUILayout.Height(65)); GUILayout.FlexibleSpace();
        bool button3 = GUILayout.Button(touchpadTextures[0], GUILayout.Width(25), GUILayout.Height(25));
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        GUILayout.Box("   Knob  ");
        GUILayout.Box(touchpadTextures[3], GUILayout.Width(50), GUILayout.Height(50));
        GUILayout.EndVertical();
        GUILayout.BeginVertical(GUILayout.Height(65)); GUILayout.FlexibleSpace();
        bool button4 = GUILayout.Button(touchpadTextures[1], GUILayout.Width(25), GUILayout.Height(25));
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        JoystickOutRange = EditorGUILayout.Slider("Outrange", JoystickOutRange, 0, 10);
        GUILayout.EndHorizontal();
        EditorGUILayout.EndToggleGroup();
        EditorGUILayout.Space(); EditorGUILayout.Space();

        //Optional Settings
        optionalSettingsEnable = EditorGUILayout.BeginToggleGroup("Optional Settings", optionalSettingsEnable);
        optionalToggle1 = EditorGUILayout.Toggle("Toggle 1", optionalToggle1);
        optionalTextField1 = EditorGUILayout.TextField("Text", optionalTextField1);
        EditorGUILayout.EndToggleGroup();
        EditorGUILayout.Space(); EditorGUILayout.Space();

        if (button1)
        {
            Debug.Log("Button1 clicked");
        }
        else if (button2)
        {
            Debug.Log("Button2 clicked");
        }
        else if (button3)
        {
            Debug.Log("Button3 clicked");
        }
        else if (button4)
        {
            Debug.Log("Button4 clicked");
        }

    }

    void ControlKeyboard()
    {
        

    }

    void ControlGamePad()
    {
        EditorGUILayout.BeginHorizontal();
        bool button1 = GUILayout.Button("button1", GUILayout.ExpandWidth(true));
        bool button2 = GUILayout.Button("button2", GUILayout.ExpandWidth(true));
        bool button3 = GUILayout.Button("button3", GUILayout.ExpandWidth(true));
        EditorGUILayout.EndHorizontal();

        if (button1)
        {
            Debug.Log("Button1 clicked");
        }
        else if (button2)
        {
            Debug.Log("Button2 clicked");
        }
        else if (button3)
        {
            Debug.Log("Button3 clicked");
        }

    }

    void TakeTextures()
    {
        //if (toolbarTextures == null)
        //{
            standartTextures = new Texture[1];
            standartTextures[0] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/control_touch_black.png", typeof(Texture));

            toolbarTextures = new Texture[3];
            toolbarTextures[0] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/control_touch_black.png", typeof(Texture));
            toolbarTextures[1] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/control_keyboard_black.png", typeof(Texture));
            toolbarTextures[2] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/control_gamepad_black.png", typeof(Texture));

            touchpadTextures = new Texture[4];
            touchpadTextures[0] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/arrow_left_black.png", typeof(Texture));
            touchpadTextures[1] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/arrow_right_black.png", typeof(Texture));
            touchpadTextures[2] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/Joysticks/joystick_center_1.png", typeof(Texture));
            touchpadTextures[3] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/Joysticks/joystick_knob_1.png", typeof(Texture));
        //}
    }

}