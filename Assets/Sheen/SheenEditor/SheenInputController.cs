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
