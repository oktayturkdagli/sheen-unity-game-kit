#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class SheenLevelController : EditorWindow
{
    Texture[] placeHolderTextures;

    string scriptableObjectName = "LevelControllerSO"; //Name of script to save InputController data
    string prefabName = "Sheen Level Controller"; //Name of prefab to be copied via InputController
    
    string[] dropDownItems = new string[] { "Order", "Random" };
    int dropDown2Selection = 0;
    int dropDown3Selection = 0;
    int dropDown4Selection = 0;
    int startSegmentUnit = 0, midSegmentUnit = 0, finalSegmentUnit = 0;

    List<GameObject> startSegment = new List<GameObject>();
    List<GameObject> midSegment = new List<GameObject>();
    List<GameObject> finalSegment = new List<GameObject>();
    List<bool> startSegmentButtons = new List<bool>(new bool[100]);
    List<bool> midSegmentButtons = new List<bool>(new bool[100]);
    List<bool> finalSegmentButtons = new List<bool>(new bool[100]);

    Editor gameObjectEditor;
    Vector2 scroll1Position, scroll2Position, scroll3Position;

    [MenuItem("Window/Sheen/Sheen Level Controller")]
    static void Init()
    {
        EditorWindow editorWindow = GetWindow(typeof(SheenLevelController), false, "Level Controller", true);
        editorWindow.minSize = new Vector2(520, 650);
        editorWindow.maxSize = new Vector2(520, 650);
    }

    void OnEnable()
    {
        TakeTextures();
        LoadValuesFromScriptableObject();
    }

    void OnGUI()
    {
        RunLevelControllerLogic();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        //Build Button
        EditorGUILayout.Space(); EditorGUILayout.Space();
        bool buttonBuild = GUILayout.Button(" Build ");
        GUILayout.EndHorizontal();
        if (buttonBuild)
        {
            CreateScriptableObject();
            CreatePrefab();
            //this.Close(); //Closes the currently open custom editor window
        }
    }

    void RunLevelControllerLogic()
    {
        float originalLabelValue = EditorGUIUtility.labelWidth;
        EditorGUILayout.Space(10);
        GUILayout.Label("Level Controller", EditorStyles.boldLabel);
        EditorGUILayout.Space(15);

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(); 

        //Start Segment
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal(GUILayout.Width(500), GUILayout.ExpandWidth(false)); //Dropdown menu
        EditorGUIUtility.labelWidth = 100;
        dropDown2Selection = EditorGUILayout.Popup("Start Segment", dropDown2Selection, dropDownItems, GUILayout.ExpandWidth(false), GUILayout.Width(175));
        EditorGUIUtility.labelWidth = 30; GUILayout.FlexibleSpace();
        startSegmentUnit = EditorGUILayout.IntField("Unit", startSegmentUnit, GUILayout.Width(65));
        EditorGUIUtility.labelWidth = originalLabelValue;
        GUILayout.EndHorizontal();
        EditorGUILayout.Space(3);
        GUILayout.BeginHorizontal(EditorStyles.helpBox, GUILayout.Width(500), GUILayout.Height(100), GUILayout.ExpandWidth(false));//Image
        DragAndDropProcess(50, 100, startSegment);
        GUILayout.Space(5);
        scroll1Position = GUILayout.BeginScrollView(scroll1Position, false, false);
        GUILayout.BeginHorizontal();
        PreviewObject(50, 65, startSegment, startSegmentButtons);
        GUILayout.EndHorizontal();
        GUILayout.EndScrollView();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        EditorGUILayout.Space(15);

        //Mid Segment
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal(GUILayout.Width(500), GUILayout.ExpandWidth(false)); //Dropdown menu
        EditorGUIUtility.labelWidth = 100;
        dropDown3Selection = EditorGUILayout.Popup("Mid Segment", dropDown3Selection, dropDownItems, GUILayout.ExpandWidth(false), GUILayout.Width(175));
        EditorGUIUtility.labelWidth = 30; GUILayout.FlexibleSpace();
        midSegmentUnit = EditorGUILayout.IntField("Unit", midSegmentUnit, GUILayout.Width(65));
        EditorGUIUtility.labelWidth = originalLabelValue;
        GUILayout.EndHorizontal();
        EditorGUILayout.Space(3);
        GUILayout.BeginHorizontal(EditorStyles.helpBox, GUILayout.Width(500), GUILayout.Height(100));//Image
        DragAndDropProcess(50, 100, midSegment);
        GUILayout.Space(5);
        scroll2Position = GUILayout.BeginScrollView(scroll2Position, false, false);
        GUILayout.BeginHorizontal();
        PreviewObject(50, 65, midSegment, midSegmentButtons);
        GUILayout.EndHorizontal();
        GUILayout.EndScrollView();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        EditorGUILayout.Space(15);

        //Final Segment
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal(GUILayout.Width(500), GUILayout.ExpandWidth(false)); //Dropdown menu
        EditorGUIUtility.labelWidth = 100;
        dropDown4Selection = EditorGUILayout.Popup("Final Segment", dropDown4Selection, dropDownItems, GUILayout.ExpandWidth(false), GUILayout.Width(175));
        EditorGUIUtility.labelWidth = 30; GUILayout.FlexibleSpace();
        finalSegmentUnit = EditorGUILayout.IntField("Unit", finalSegmentUnit, GUILayout.Width(65));
        EditorGUIUtility.labelWidth = originalLabelValue;
        GUILayout.EndHorizontal();
        EditorGUILayout.Space(3);
        GUILayout.BeginHorizontal(EditorStyles.helpBox, GUILayout.Width(500), GUILayout.Height(100));//Image
        DragAndDropProcess(50, 100, finalSegment);
        GUILayout.Space(5);
        scroll3Position = GUILayout.BeginScrollView(scroll3Position, false, false);
        GUILayout.BeginHorizontal();
        PreviewObject(60, 65, finalSegment, finalSegmentButtons);
        GUILayout.EndHorizontal();
        GUILayout.EndScrollView();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        EditorGUILayout.Space(15);

        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        EditorGUILayout.Space(15);

    }

    void TakeTextures()
    {
        placeHolderTextures = BringTexture("Assets/Sheen/Images/Level/PlaceHolders/");
    }

    Texture[] BringTexture(string path)
    {
        DirectoryInfo directory = new DirectoryInfo(@path); //Assuming Test is your Folder
        FileInfo[] files = directory.GetFiles("*.png"); //Getting png files
        Texture[] textures = new Texture[files.Length];

        for (int i = 0; i < files.Length; i++)
        {
            textures[i] = (Texture)AssetDatabase.LoadAssetAtPath(path + files[i].Name, typeof(Texture));
        }

        return textures;
    }

    public void LoadValuesFromScriptableObject()
    {
        LevelControllerSO existingSO = (LevelControllerSO)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Resources/" + scriptableObjectName + ".asset", typeof(ScriptableObject));
        if (existingSO)
        {
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

    public void SaveValuesToScriptableObject()
    {
        LevelControllerSO existingSO = (LevelControllerSO)Resources.Load<LevelControllerSO>(scriptableObjectName);
        if (existingSO)
        {
            existingSO.dropDown2Selection = dropDown2Selection;
            existingSO.dropDown3Selection = dropDown3Selection;
            existingSO.dropDown4Selection = dropDown4Selection;
            existingSO.startSegment = startSegment;
            existingSO.midSegment = midSegment;
            existingSO.finalSegment = finalSegment;
            existingSO.startSegmentUnit = startSegmentUnit;
            existingSO.midSegmentUnit = midSegmentUnit;
            existingSO.finalSegmentUnit = finalSegmentUnit;
        }
    }

    void CreateScriptableObject()
    {
        if (!Directory.Exists("Assets/Sheen/Resources/"))
        {
            Directory.CreateDirectory("Assets/Sheen/Resources/");
        }

        LevelControllerSO existingSO = (LevelControllerSO)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Resources/" + scriptableObjectName + ".asset", typeof(ScriptableObject));
        if (!existingSO)
        {
            LevelControllerSO levelControllerSOC = ScriptableObject.CreateInstance<LevelControllerSO>();
            AssetDatabase.CreateAsset(levelControllerSOC, "Assets/Sheen/Resources/" + scriptableObjectName + ".asset");
            AssetDatabase.SaveAssets();
            existingSO = levelControllerSOC;
        }

        SaveValuesToScriptableObject();
        EditorUtility.SetDirty(existingSO); //Saves changes made to this file
    }

    void CreatePrefab()
    {
        Level level = FindObjectOfType<Level>();
        if (!level)
        {
            Object myPrefab = AssetDatabase.LoadAssetAtPath("Assets/Sheen/LevelController/Prefabs/" + prefabName + ".prefab", typeof(GameObject));
            PrefabUtility.InstantiatePrefab(myPrefab);
            level = FindObjectOfType<Level>();
        }

        level.LoadValuesFromScriptableObject();
        level.Build();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = level;
    }

    void DragAndDropProcess(float width, float height, List<GameObject> segmentType)
    {
        Event evt = Event.current;
        Rect dropArea = GUILayoutUtility.GetRect(width, height, GUILayout.ExpandWidth(false));
        GUI.Box(dropArea, placeHolderTextures[1]);

        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!dropArea.Contains(evt.mousePosition))
                    return;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();

                    for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
                    {
                        Object obj = DragAndDrop.objectReferences[i];
                        GameObject gameobj = (GameObject)DragAndDrop.objectReferences[i];

                        string path = DragAndDrop.paths[i];
                        segmentType.Add(gameobj);
                        
                        //Debug.Log(gameobj.name);
                        //Debug.Log(path);

                    }
                }
                break;
        }
    }

    void PreviewObject(float width, float height, List<GameObject> segmentType, List<bool> segmentButtons)
    {
        for (int i = 0; i < segmentType.Count; i++)
        {
            GUIStyle labelStyle = new GUIStyle();
            labelStyle.normal.background = Texture2D.grayTexture;

            GUIStyle buttonStyle = new GUIStyle();
            buttonStyle.normal.background = (Texture2D)placeHolderTextures[3];

            if (segmentType[i] != null)
            {
                if (gameObjectEditor == null)
                    gameObjectEditor = Editor.CreateEditor(segmentType[i]);

                GUILayout.Space(5);
                GUILayout.BeginVertical(GUILayout.Width(width), GUILayout.Height(70));
                GUILayout.BeginHorizontal(buttonStyle);
                gameObjectEditor.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(width - 20, height), buttonStyle);
                segmentButtons[i] = GUILayout.Button("x", GUILayout.Width(20));
                if (segmentButtons[i]) { RemoveObject(segmentType, i); i = 0; }
                GUILayout.EndHorizontal();

                if (segmentType.Count < 1)
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.EndVertical();
                    GUILayout.Space(5);
                    break;
                } 

                GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace(); GUILayout.Label(segmentType[i].name, labelStyle); GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.EndVertical();
                GUILayout.Space(5);
            }
        }
    }

    void RemoveObject(List<GameObject> segmentType, int index)
    {
        if (segmentType.Count <= index)
            return;

        segmentType.RemoveAt(index);
        Debug.Log("Segment " + index + " removed.");

        int segmentButtonsCount = startSegmentButtons.Count;
        for (int i = 0; i < segmentButtonsCount; i++)
        {
            startSegmentButtons[i] = false;
        }
    }
}
#endif
