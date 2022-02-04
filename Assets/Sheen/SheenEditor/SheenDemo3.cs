using UnityEditor;
using UnityEngine;
using UnityEditor.AnimatedValues;

public class SheenDemo3 : EditorWindow
{
    int toolbarInt = 0;
    string[] toolbarStrings = { "Toolbar1", "Toolbar2", "Toolbar3" };
    
    [MenuItem("Window/Sheen/Sheen Demo 3")]
    public static void Init()
    {
        GetWindow(typeof(SheenDemo3));
    }

    void OnGUI()
    {
        toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarStrings);
        switch (toolbarInt)
        {
            default: case 0:
                Texture texture1 = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/control_touch_black.png", typeof(Texture));
                Texture texture2 = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/control_keyboard_black.png", typeof(Texture));
                Texture texture3 = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/control_gamepad_black.png", typeof(Texture));

                EditorGUILayout.BeginHorizontal();
                bool button1 = GUILayout.Button(texture1, GUILayout.ExpandWidth(true));
                bool button2 = GUILayout.Button(texture2, GUILayout.ExpandWidth(true));
                bool button3 = GUILayout.Button(texture3, GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();

                if (button1)
                {
                    ControlTouchPad();
                }
                else if(button2)
                {
                    ControlKeyboard();
                }
                else if (button3)
                {
                    ControlGamePad();
                }

                break;
            case 1:
                
                break;
            case 2:
                break;
        }
    }

    void ControlTouchPad()
    {
        Debug.Log("Clicked the Touchpad");
    }

    void ControlKeyboard()
    {
        Debug.Log("Clicked the Keyboard");

    }

    void ControlGamePad()
    {
        Debug.Log("Clicked the Gamepad");

    }

}