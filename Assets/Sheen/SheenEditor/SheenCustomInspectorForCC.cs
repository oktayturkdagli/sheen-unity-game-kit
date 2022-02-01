using UnityEditor;

public class SheenCustomInspectorForCC : EditorWindow
{
    [MenuItem("Window/Sheen/Sheen Character Controller")]
    public static void ShowWindow()
    {
        SheenCustomInspectorForCC window = (SheenCustomInspectorForCC)EditorWindow.GetWindow(typeof(SheenCustomInspectorForCC));
    }

    void OnGUI()
    {
        var editor = Editor.CreateEditor(Selection.activeGameObject.GetComponent<SheenCharacterController>());
        editor.OnInspectorGUI();
    }
}
