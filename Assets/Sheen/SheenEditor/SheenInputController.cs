using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class SheenInputController : EditorWindow
{
    int toolbarInt = 0;
    Texture[] toolbarTextures;
    Texture[] touchpadTextures;

    [MenuItem("Window/Sheen/Sheen Input Controller")]
    public static void Init()
    {
        GetWindow(typeof(SheenInputController));
        
    }

    void OnGUI()
    {
        TakeTextures();
        toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarTextures);
        switch (toolbarInt)
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
        GUILayout.Label("Costumize", EditorStyles.boldLabel);

        //GUILayout.Button(FormatTexture("control_touch_black.png"), GUILayout.ExpandWidth(false));
    }

    void ControlKeyboard()
    {
        string myString = "Hello World";
        bool groupEnabled = false;
        bool myBool = true;
        float myFloat = 1.23f;

        EditorGUILayout.BeginHorizontal();
        myString = EditorGUILayout.TextField("Text Field", myString);
        EditorGUILayout.EndHorizontal();


        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();

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
        toolbarTextures = new Texture[3];
        toolbarTextures[0] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/control_touch_black.png", typeof(Texture));
        toolbarTextures[1] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/control_keyboard_black.png", typeof(Texture));
        toolbarTextures[2] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/control_gamepad_black.png", typeof(Texture));

        touchpadTextures = new Texture[3];
        touchpadTextures[0] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/arrow_left_black.png", typeof(Texture));
        touchpadTextures[0] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/arrow_right_black.png", typeof(Texture));
        touchpadTextures[0] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/control_gamepad_black.png", typeof(Texture));

    }
}