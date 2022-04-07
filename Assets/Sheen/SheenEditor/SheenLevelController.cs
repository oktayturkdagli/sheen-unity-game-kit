#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class SheenLevelController : EditorWindow
{
    Texture[] placeHolderTextures;
    Texture placeHolder1;
    int placeHolderCounter = 0;

    string scriptableObjectName = "LevelControllerSO"; //Name of script to save InputController data
    string prefabName = "Sheen Level Controller"; //Name of prefab to be copied via InputController
    
    string[] dropDown1Items = new string[] { "Path1", "Path2", "Path3", "Path4" };
    string[] dropDown2Items = new string[] { "Random", "Order" };
    int dropDown1Selection = 0;
    int dropDown2Selection = 0;
    int dropDown3Selection = 0;
    int dropDown4Selection = 0;
    List<float> sliders = new List<float>(new float[100]);

    int startSegmentUnit = 0, midSegmentUnit = 0, finalSegmentUnit = 0;

    [MenuItem("Window/Sheen/Sheen Level Controller")]
    static void Init()
    {
        EditorWindow editorWindow = GetWindow(typeof(SheenLevelController), false, "Level Controller", true);
        editorWindow.minSize = new Vector2(500, 650);
        editorWindow.maxSize = new Vector2(500, 650);
    }

    void OnEnable()
    {
        TakeTextures();
        LoadValuesFromScriptableObject();
    }

    void OnGUI()
    {
        RunLevelControllerLogic();
        //Build Button
        EditorGUILayout.Space(); EditorGUILayout.Space();
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
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
        int sliderCounter = 0;
        float originalLabelValue = EditorGUIUtility.labelWidth;
        EditorGUILayout.Space(10);
        GUILayout.Label("Level Controller", EditorStyles.boldLabel);
        EditorGUILayout.Space(15);

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical();

        //Path
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal(); //Dropdown menu
        EditorGUIUtility.labelWidth = 50;
        dropDown1Selection = EditorGUILayout.Popup("Path", dropDown1Selection, dropDown1Items, GUILayout.ExpandWidth(false), GUILayout.Width(125));
        EditorGUIUtility.labelWidth = originalLabelValue;  GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(EditorStyles.helpBox, GUILayout.Width(500), GUILayout.Height(100));//Image
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(); GUILayout.FlexibleSpace();
        GUILayout.Box(placeHolder1, GUILayout.Width(490), GUILayout.Height(90), GUILayout.ExpandWidth(true)); 
        GUILayout.FlexibleSpace(); GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        EditorGUILayout.Space(15);


        

        //Start Segment
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal(); //Dropdown menu
        EditorGUIUtility.labelWidth = 100;
        dropDown2Selection = EditorGUILayout.Popup("Start Segment", dropDown2Selection, dropDown2Items, GUILayout.ExpandWidth(false), GUILayout.Width(175));
        EditorGUIUtility.labelWidth = 30; GUILayout.FlexibleSpace();
        startSegmentUnit = EditorGUILayout.IntField("Unit", startSegmentUnit, GUILayout.Width(65));
        EditorGUIUtility.labelWidth = originalLabelValue;
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(EditorStyles.helpBox, GUILayout.Width(500), GUILayout.Height(100));//Image
        GUILayout.BeginVertical(); GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        GUILayout.Box(placeHolder1);
        sliders[sliderCounter] = GUILayout.HorizontalSlider(sliders[sliderCounter], 0, 100); sliderCounter++;
        GUILayout.EndVertical();
        GUILayout.Space(5);
        GUILayout.BeginVertical();
        GUILayout.Box(placeHolder1);
        sliders[sliderCounter] = GUILayout.HorizontalSlider(sliders[sliderCounter], 0, 100); sliderCounter++;
        GUILayout.EndVertical();
        GUILayout.Space(5);
        GUILayout.BeginVertical();
        GUILayout.Box(placeHolder1);
        sliders[sliderCounter] = GUILayout.HorizontalSlider(sliders[sliderCounter], 0, 100); sliderCounter++;
        GUILayout.EndVertical();
        GUILayout.Space(5);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace(); GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        EditorGUILayout.Space(15);

        //Mid Segment
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal(); //Dropdown menu
        EditorGUIUtility.labelWidth = 100;
        dropDown3Selection = EditorGUILayout.Popup("Mid Segment", dropDown3Selection, dropDown2Items, GUILayout.ExpandWidth(false), GUILayout.Width(175));
        EditorGUIUtility.labelWidth = 30; GUILayout.FlexibleSpace();
        midSegmentUnit = EditorGUILayout.IntField("Unit", midSegmentUnit, GUILayout.Width(65));
        EditorGUIUtility.labelWidth = originalLabelValue;
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(EditorStyles.helpBox, GUILayout.Width(500), GUILayout.Height(100));//Image
        GUILayout.BeginVertical(); GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        GUILayout.Box(placeHolder1);
        sliders[sliderCounter] = GUILayout.HorizontalSlider(sliders[sliderCounter], 0, 100); sliderCounter++;
        GUILayout.EndVertical();
        GUILayout.Space(5);
        GUILayout.BeginVertical();
        GUILayout.Box(placeHolder1);
        sliders[sliderCounter] = GUILayout.HorizontalSlider(sliders[sliderCounter], 0, 100); sliderCounter++;
        GUILayout.EndVertical();
        GUILayout.Space(5);
        GUILayout.BeginVertical();
        GUILayout.Box(placeHolder1);
        sliders[sliderCounter] = GUILayout.HorizontalSlider(sliders[sliderCounter], 0, 100); sliderCounter++;
        GUILayout.EndVertical();
        GUILayout.Space(5);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace(); GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        EditorGUILayout.Space(15);

        //Final Segment
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal(); //Dropdown menu
        EditorGUIUtility.labelWidth = 100;
        dropDown4Selection = EditorGUILayout.Popup("Final Segment", dropDown4Selection, dropDown2Items, GUILayout.ExpandWidth(false), GUILayout.Width(175));
        EditorGUIUtility.labelWidth = 30; GUILayout.FlexibleSpace();
        finalSegmentUnit = EditorGUILayout.IntField("Unit", finalSegmentUnit, GUILayout.Width(65));
        EditorGUIUtility.labelWidth = originalLabelValue;
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(EditorStyles.helpBox, GUILayout.Width(500), GUILayout.Height(100));//Image
        GUILayout.BeginVertical(); GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        GUILayout.Box(placeHolder1);
        sliders[sliderCounter] = GUILayout.HorizontalSlider(sliders[sliderCounter], 0, 100); sliderCounter++;
        GUILayout.EndVertical();
        GUILayout.Space(5);
        GUILayout.BeginVertical();
        GUILayout.Box(placeHolder1);
        sliders[sliderCounter] = GUILayout.HorizontalSlider(sliders[sliderCounter], 0, 100); sliderCounter++;
        GUILayout.EndVertical();
        GUILayout.Space(5);
        GUILayout.BeginVertical();
        GUILayout.Box(placeHolder1);
        sliders[sliderCounter] = GUILayout.HorizontalSlider(sliders[sliderCounter], 0, 100); sliderCounter++;
        GUILayout.EndVertical();
        GUILayout.Space(5);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace(); GUILayout.EndVertical();
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
        placeHolderTextures = BringTexture("Assets/Sheen/Images/Level/PlaceHolder/");
        placeHolder1 = placeHolderTextures[placeHolderCounter];
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
        
    }

    public void SaveValuesToScriptableObject()
    {
        
    }

    void CreateScriptableObject()
    {
        if (!Directory.Exists("Assets/Sheen/Resources/"))
        {
            Directory.CreateDirectory("Assets/Sheen/Resources/");
        }

        CharacterControllerSO existingSO = (CharacterControllerSO)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Resources/" + scriptableObjectName + ".asset", typeof(ScriptableObject));
        if (!existingSO)
        {
            CharacterControllerSO levelControllerSOC = ScriptableObject.CreateInstance<CharacterControllerSO>();
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

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = level;
    }

}
#endif
