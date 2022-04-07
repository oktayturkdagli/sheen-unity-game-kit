using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class Level : MonoBehaviour
{
    //string scriptableObjectName = "LevelControllerSO";

    private void Awake()
    {
        LoadValuesFromScriptableObject();
    }

    private void OnEnable()
    {
        LoadValuesFromScriptableObject();
    }

    void Start()
    {

    }

    void Update()
    {

    }

    void RunLevelLogic()
    {

    }

    public void AccessLevel()
    {
        
    }

    public void LoadValuesFromScriptableObject()
    {

    }

    public void SaveValuesToScriptableObject()
    {

    }

}

//#if UNITY_EDITOR
//[CustomEditor(typeof(Level))]
//public class LevelEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();
//        GUILayout.Space(5);
//        Level level = (Level)target;
//        if (GUILayout.Button("Save"))
//        {
//            level.SaveValuesToScriptableObject();
//        }
//        GUILayout.Space(10);
//    }
//}
//#endif