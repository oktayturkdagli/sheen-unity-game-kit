using UnityEngine;
using UnityEditor;


public class SheenCustomInspectorForIC : EditorWindow
{
    //----
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;
    //----

    [MenuItem("Window/Sheen/Sheen Input Controller")]
    public static void ShowWindow()
    {
        SheenCustomInspectorForIC window = (SheenCustomInspectorForIC)EditorWindow.GetWindow(typeof(SheenCustomInspectorForIC));
    }

    void OnGUI()
    {
        var editor = Editor.CreateEditor(Selection.activeGameObject.GetComponent<SheenInputController>());
        editor.OnInspectorGUI();

        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        myString = EditorGUILayout.TextField("Text Field", myString);

        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();
    }
}
