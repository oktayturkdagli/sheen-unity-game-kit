using UnityEngine;
using UnityEditor;


public class SheenCustomInspectorForSD : EditorWindow
{
    //----
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;
    //----

    [MenuItem("Window/Sheen/Sheen Demo")]
    public static void ShowWindow()
    {
        SheenCustomInspectorForSD window = (SheenCustomInspectorForSD)EditorWindow.GetWindow(typeof(SheenCustomInspectorForSD));
    }

    void OnGUI()
    {
        var editor = Editor.CreateEditor(Selection.activeGameObject.GetComponent<SheenDemo>());
        editor.OnInspectorGUI();

        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        myString = EditorGUILayout.TextField("Text Field", myString);

        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();
    }
}
