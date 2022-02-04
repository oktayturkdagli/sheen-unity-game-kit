using UnityEngine;
using UnityEditor;

public class SheenInputController : MonoBehaviour
{
    public int level;
    public float health;
    public Vector3 target;

    public GameObject obj;
    public Vector3 spawnPoint;

    public void BuildObject()
    {
        Instantiate(obj, spawnPoint, Quaternion.identity);
    }

}

[CustomEditor(typeof(SheenInputController))]
public class SheenInputControllerEditor : SheenEditor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        SheenInputController myScript = (SheenInputController)target;
        if (GUILayout.Button("Build Object"))
        {
            myScript.BuildObject();
        }
        EditorGUILayout.HelpBox("This is a help box", MessageType.Info);

    }

}

public class SheenInputControllerInspector : EditorWindow
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
        EditorWindow.GetWindow(typeof(SheenInputControllerInspector));
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