using UnityEditor;
using UnityEngine;

public class SheenDemo2 : EditorWindow
{
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;

    [MenuItem("Window/Sheen/Sheen Demo 2")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(SheenDemo2));
    }

    void OnGUI()
    {
        Texture texture1 = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/control_touch_black.png", typeof(Texture));
        GUILayout.Box(texture1);
        Texture texture2 = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/control_keyboard_black.png", typeof(Texture));
        GUILayout.Box(texture2);


        GUI.DrawTexture(new Rect(10, 250, 60, 60), texture1, ScaleMode.StretchToFill, true, 0f);

        GUILayout.Label("Costumize", EditorStyles.boldLabel);


        myString = EditorGUILayout.TextField("Text Field", myString);

        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();

    }
}