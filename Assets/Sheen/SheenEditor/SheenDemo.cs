using UnityEngine;
using UnityEditor;

public class SheenDemo : MonoBehaviour
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


[CustomEditor(typeof(SheenDemo))]
public class SheenDemoEditor : SheenEditor
{


    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        SheenDemo myScript = (SheenDemo)target;
        if (GUILayout.Button("Build Object"))
        {
            myScript.BuildObject();
        }
        EditorGUILayout.HelpBox("This is a help box", MessageType.Info);



    }

}
