using UnityEngine;
using UnityEditor;

public class SheenCharacterController : MonoBehaviour
{
    public int character;
}


[CustomEditor(typeof(SheenCharacterController))]
public class SheenCharacterControllerEditor : SheenEditor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.HelpBox("This is Character Controller", MessageType.Info);
    }

}
