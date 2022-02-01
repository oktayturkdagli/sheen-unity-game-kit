using UnityEditor;

public class SheenCustomInspectorForLC : EditorWindow
{
    [MenuItem("Window/Sheen/Sheen Level Controller")]
    public static void ShowWindow()
    {
        SheenCustomInspectorForLC window = (SheenCustomInspectorForLC)EditorWindow.GetWindow(typeof(SheenCustomInspectorForLC));
    }

    void OnGUI()
    {
        var editor = Editor.CreateEditor(Selection.activeGameObject.GetComponent<SheenLevelController>());
        editor.OnInspectorGUI();
    }
}
