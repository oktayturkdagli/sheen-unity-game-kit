using UnityEngine;
using UnityEditor;

public class SheenLevelController : MonoBehaviour
{
    public int level;

}


[CustomEditor(typeof(SheenLevelController))]
public class SheenLevelControllerEditor : SheenEditor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.HelpBox("This is Level Controller", MessageType.Info);
    }

}
