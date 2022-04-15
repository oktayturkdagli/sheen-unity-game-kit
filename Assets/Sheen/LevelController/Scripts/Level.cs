using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class Level : MonoBehaviour
{
    [SerializeField] int dropDown1Selection = 0;
    [SerializeField] int dropDown2Selection = 0;
    [SerializeField] int dropDown3Selection = 0;
    [SerializeField] int dropDown4Selection = 0;
    [SerializeField] int startSegmentUnit = 0, midSegmentUnit = 0, finalSegmentUnit = 0;
    [SerializeField] List<GameObject> startSegment;
    [SerializeField] List<GameObject> midSegment;
    [SerializeField] List<GameObject> finalSegment;

    string scriptableObjectName = "LevelControllerSO";

    private void Awake()
    {
        LoadValuesFromScriptableObject();
    }

    private void OnEnable()
    {
        LoadValuesFromScriptableObject();
    }

    public void Build()
    {
        Transform parent = transform;
        int parentChildCount = parent.childCount;
        for (int i = 0; i < parentChildCount; i++)
            DestroyImmediate(parent.GetChild(0).gameObject);

        Vector3 direction = Vector3.forward;
        Vector3 position = Vector3.zero;
        switch (dropDown1Selection)
        {
            case 1:
                direction = Vector3.up;
                break;
            case 2:
                direction = Vector3.forward;
                break;
            case 3:
                direction = Vector3.right;
                break;
            default:
                direction = Vector3.right;
                break;
        }

        int startCounter = -1;
        for (int i = 0; i < startSegmentUnit; i++)
        {
            startCounter++;
            if (startCounter > startSegment.Count - 1)
            {
                startCounter = 0;
                if (startCounter > startSegment.Count - 1)
                    break;
            }

            if (dropDown2Selection == 1)
                startCounter = Random.Range(0, startSegment.Count);

            Instantiate(startSegment[startCounter], position, Quaternion.identity, parent);
            position += direction * 10;
        }

        int midCounter = -1;
        for (int i = 0; i < midSegmentUnit; i++)
        {
            midCounter++;
            if (midCounter > midSegment.Count - 1)
            {
                midCounter = 0;
                if (midCounter > midSegment.Count - 1)
                    break;
            }

            if (dropDown3Selection == 1)
                midCounter = Random.Range(0, midSegment.Count);

            Instantiate(midSegment[midCounter], position, Quaternion.identity, parent);
            position += direction * 10;
        }

        int finalCounter = -1;
        for (int i = 0; i < finalSegmentUnit; i++)
        {
            finalCounter++;
            if (finalCounter > finalSegment.Count - 1)
            {
                finalCounter = 0;
                if (finalCounter > finalSegment.Count - 1)
                    break;
            }

            if (dropDown4Selection == 1)
                finalCounter = Random.Range(0, finalSegment.Count);

            Instantiate(finalSegment[finalCounter], position, Quaternion.identity, parent);
            position += direction * 10;
        }
    }

    public void LoadValuesFromScriptableObject()
    {
        LevelControllerSO existingSO = (LevelControllerSO)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Resources/" + scriptableObjectName + ".asset", typeof(ScriptableObject));
        if (existingSO)
        {
            dropDown1Selection = existingSO.dropDown1Selection;
            dropDown2Selection = existingSO.dropDown2Selection;
            dropDown3Selection = existingSO.dropDown3Selection;
            dropDown4Selection = existingSO.dropDown4Selection;
            startSegment = existingSO.startSegment;
            midSegment = existingSO.midSegment;
            finalSegment = existingSO.finalSegment;
            startSegmentUnit = existingSO.startSegmentUnit;
            midSegmentUnit = existingSO.midSegmentUnit;
            finalSegmentUnit = existingSO.finalSegmentUnit;
        }
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUILayout.Space(5);
        Level level = (Level)target;
        if (GUILayout.Button("Build"))
        {
            level.Build();
        }
        GUILayout.Space(10);
    }
}
#endif