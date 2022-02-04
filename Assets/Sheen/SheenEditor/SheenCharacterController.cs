using UnityEngine;
using UnityEditor;

public class SheenCharacterController : MonoBehaviour
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

[CustomEditor(typeof(SheenCharacterController))]
public class SheenCharacterControllerEditor : SheenEditor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        SheenCharacterController myScript = (SheenCharacterController)target;
        if (GUILayout.Button("Build Object"))
        {
            myScript.BuildObject();
        }
        EditorGUILayout.HelpBox("This is a help box", MessageType.Info);

    }

}

public class SheenCharacterControllerInspector : EditorWindow
{
    //----
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;
    //----

    [MenuItem("Window/Sheen/Sheen Character Controller")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(SheenCharacterControllerInspector));
    }

    void OnGUI()
    {
        var editor = Editor.CreateEditor(Selection.activeGameObject.GetComponent<SheenCharacterController>());
        editor.OnInspectorGUI();

        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        myString = EditorGUILayout.TextField("Text Field", myString);

        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();
    }


}