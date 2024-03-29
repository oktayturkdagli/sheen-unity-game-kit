﻿#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public class SheenInputController : EditorWindow
{
    int selectedToolbarIndex = 0;
    Texture[] standartTextures, toolbarTextures, touchpadTextures;
    
    //Touch
    bool useTouch = true, workOnHalfOfScreenTouch = true;
    Sprite[] touchpadSprites;
    float tapThreshold = 0.2f;

    //Joystick
    bool useJoystick = true, fixedJoystick = true, alwaysDisplayJoystick = true, workOnHalfOfScreenJoystick = true;
    int joystickCenterCounter = 1, joystickKnobCounter = 5;
    Texture joystickCenterTexture, joystickKnobTexture;
    float joystickOutRange = 1f;

    //TODO: Optional menu will added
    //bool optionalSettingsEnable = false;
    //bool optionalToggle1 = true;
    //string optionalTextField1 = "Optional text";

    string scriptableObjectName = "InputControllerSO"; //Name of script to save InputController data
    string prefabName = "Sheen Input Controller"; //Name of prefab to be copied via InputController

    [MenuItem("Window/Sheen/Sheen Input Controller")]
    public static void Init()
    {
        EditorWindow editorWindow = GetWindow(typeof(SheenInputController), false, "Input Controller", true);
        editorWindow.minSize = new Vector2(350, 500);
        editorWindow.maxSize = new Vector2(350, 500);
    }

    void OnEnable()
    {
        TakeTextures();
        LoadValuesFromScriptableObject();
    }

    void OnGUI()
    {
        //InputController consists of a triple toolbar menu
        //But now the user can only see the first menu containing the Touchbar and Joystick features.
        //Other options will be added in later versions
        selectedToolbarIndex = GUILayout.Toolbar(selectedToolbarIndex, toolbarTextures);
        switch (selectedToolbarIndex)
        {
            default:
            case 0:
                RunInputControllerLogic();
                break;
            case 1:
                Debug.Log("This is reserved for future developments.");
                break;
            case 2:
                Debug.Log("This is reserved for future developments.");
                break;
        }

        //Build Button
        EditorGUILayout.Space(); EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        bool buttonBuild = GUILayout.Button(" Build ");
        GUILayout.EndHorizontal();
        if (buttonBuild)
        {
            CreateScriptableObject();
            CreatePrefab();
            this.Close(); //Closes the currently open custom editor window
        }
    }

    void RunInputControllerLogic()
    {
        //Touch
        EditorGUILayout.Space();
        useTouch = EditorGUILayout.BeginToggleGroup("Touch", useTouch);
        tapThreshold = EditorGUILayout.Slider("Tap Treshold", tapThreshold, 0.01f, 1f);
        workOnHalfOfScreenTouch = EditorGUILayout.Toggle("Work on Only Half Of Screen", workOnHalfOfScreenTouch);
        EditorGUILayout.EndToggleGroup();
        EditorGUILayout.Space(); EditorGUILayout.Space();

        //Joystick
        useJoystick = EditorGUILayout.BeginToggleGroup("Joystick", useJoystick);
        //Joystick Visuals
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        //Center
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUILayout.Height(105));
        GUILayout.FlexibleSpace(); bool buttonCL = GUILayout.Button(standartTextures[0], GUILayout.Width(25), GUILayout.Height(25)); GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.BeginVertical(GUILayout.Width(75));
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace(); GUILayout.Box("Center", GUILayout.Width(75)); GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace(); GUILayout.Box(joystickCenterTexture, GUILayout.Width(50), GUILayout.Height(50)); GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace(); GUILayout.Box(joystickCenterCounter.ToString(), GUILayout.Width(30)); GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.BeginVertical(GUILayout.Height(105));
        GUILayout.FlexibleSpace(); bool buttonCR = GUILayout.Button(standartTextures[1], GUILayout.Width(25), GUILayout.Height(25)); GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        //Mid Space
        GUILayout.FlexibleSpace();
        //Knob
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUILayout.Height(105));
        GUILayout.FlexibleSpace(); bool buttonKL = GUILayout.Button(standartTextures[0], GUILayout.Width(25), GUILayout.Height(25)); GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.BeginVertical(GUILayout.Width(75));
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace(); GUILayout.Box("Knob", GUILayout.Width(75)); GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace(); GUILayout.Box(joystickKnobTexture, GUILayout.Width(50), GUILayout.Height(50)); GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace(); GUILayout.Box(joystickKnobCounter.ToString(), GUILayout.Width(30)); GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.BeginVertical(GUILayout.Height(105));
        GUILayout.FlexibleSpace(); bool buttonKR = GUILayout.Button(standartTextures[1], GUILayout.Width(25), GUILayout.Height(25)); GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        //End Space
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        EditorGUILayout.Space(5);
        //Joystick Visuals
        joystickOutRange = EditorGUILayout.Slider("Outrange", joystickOutRange, 0.01f, 1f);
        fixedJoystick = EditorGUILayout.Toggle("Fixed Joystick", fixedJoystick);
        alwaysDisplayJoystick = EditorGUILayout.Toggle("Always Display", alwaysDisplayJoystick);
        workOnHalfOfScreenJoystick = EditorGUILayout.Toggle("Work on Only Half Of Screen", workOnHalfOfScreenJoystick);
        EditorGUILayout.EndToggleGroup();
        EditorGUILayout.Space(); EditorGUILayout.Space();

        ////Optional Settings
        //optionalSettingsEnable = EditorGUILayout.BeginFoldoutHeaderGroup(optionalSettingsEnable, "Optional Settings");
        //if (optionalSettingsEnable)
        //{
        //    optionalToggle1 = EditorGUILayout.Toggle("Optional Toggle", optionalToggle1);
        //    optionalTextField1 = EditorGUILayout.TextField("Optional Text", optionalTextField1);
        //    EditorGUILayout.HelpBox("You must save your changes for them to take effect.", MessageType.Info);
        //}
        //EditorGUILayout.EndFoldoutHeaderGroup();
        //EditorGUILayout.Space();

        //Buttons created to select joystick UI textures
        if (buttonCL)
        {
            joystickCenterCounter--;
            if (joystickCenterCounter < 0)
                joystickCenterCounter = touchpadTextures.Length - 1;
            joystickCenterTexture = touchpadTextures[joystickCenterCounter];
        }
        else if (buttonCR)
        {
            joystickCenterCounter++;
            if (joystickCenterCounter > touchpadTextures.Length - 1)
                joystickCenterCounter = 0;
            joystickCenterTexture = touchpadTextures[joystickCenterCounter];
        }
        else if (buttonKL)
        {
            joystickKnobCounter--;
            if (joystickKnobCounter < 0)
                joystickKnobCounter = touchpadTextures.Length - 1;
            joystickKnobTexture = touchpadTextures[joystickKnobCounter];
        }
        else if (buttonKR)
        {
            joystickKnobCounter++;
            if (joystickKnobCounter > touchpadTextures.Length - 1)
                joystickKnobCounter = 0;
            joystickKnobTexture = touchpadTextures[joystickKnobCounter];
        }

    }

    void TakeTextures()
    {
        toolbarTextures = BringTexture("Assets/Sheen/Images/Input/Toolbar/");
        standartTextures = BringTexture("Assets/Sheen/Images/Arrows/");
        touchpadTextures = BringTexture("Assets/Sheen/Images/Input/Joysticks/");
        touchpadSprites = BringSprite("Assets/Sheen/Images/Input/Joysticks/");

        joystickCenterTexture = touchpadTextures[joystickCenterCounter];
        joystickKnobTexture = touchpadTextures[joystickKnobCounter];
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

    Sprite[] BringSprite(string path)
    {
        DirectoryInfo directory = new DirectoryInfo(@path); //Assuming Test is your Folder
        FileInfo[] files = directory.GetFiles("*.png"); //Getting png files
        Sprite[] sprites = new Sprite[files.Length];

        for (int i = 0; i < files.Length; i++)
        {
            sprites[i] = (Sprite)AssetDatabase.LoadAssetAtPath(path + files[i].Name, typeof(Sprite));
        }

        return sprites;
    }

    public void LoadValuesFromScriptableObject()
    {
        InputControllerSO existingSO = (InputControllerSO)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Resources/" + scriptableObjectName + ".asset", typeof(ScriptableObject));
        if (existingSO)
        {
            useTouch = existingSO.useTouch;
            tapThreshold = existingSO.tapThreshold;
            workOnHalfOfScreenTouch = existingSO.workOnHalfOfScreenTouch;
            useJoystick = existingSO.useJoystick;
            //joystickCenterTexture = existingSO.joystickCenterTexture;
            //joystickKnobTexture = existingSO.joystickKnobTexture;
            joystickOutRange = existingSO.joystickOutRange;
            fixedJoystick = existingSO.fixedJoystick;
            alwaysDisplayJoystick = existingSO.alwaysDisplayJoystick;
            workOnHalfOfScreenJoystick = existingSO.workOnHalfOfScreenJoystick;
        }
    }

    public void SaveValuesToScriptableObject()
    {
        InputControllerSO existingSO = (InputControllerSO)Resources.Load<InputControllerSO>(scriptableObjectName);
        if (existingSO)
        {
            existingSO.useTouch = useTouch;
            existingSO.tapThreshold = tapThreshold;
            existingSO.workOnHalfOfScreenTouch = workOnHalfOfScreenTouch;
            existingSO.useJoystick = useJoystick;
            existingSO.joystickCenterSprite = touchpadSprites[joystickCenterCounter];
            existingSO.joystickKnobSprite = touchpadSprites[joystickKnobCounter];
            existingSO.joystickOutRange = joystickOutRange;
            existingSO.fixedJoystick = fixedJoystick;
            existingSO.alwaysDisplayJoystick = alwaysDisplayJoystick;
            existingSO.workOnHalfOfScreenJoystick = workOnHalfOfScreenJoystick;
        }
    }

    void CreateScriptableObject()
    {
        if (!Directory.Exists("Assets/Sheen/Resources/"))
        {
            Directory.CreateDirectory("Assets/Sheen/Resources/");
        }

        InputControllerSO existingSO = (InputControllerSO)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Resources/" + scriptableObjectName + ".asset", typeof(ScriptableObject));
        if (!existingSO)
        {
            InputControllerSO inputControllerSOC = ScriptableObject.CreateInstance<InputControllerSO>();
            AssetDatabase.CreateAsset(inputControllerSOC, "Assets/Sheen/Resources/" + scriptableObjectName + ".asset");
            AssetDatabase.SaveAssets();
            existingSO = inputControllerSOC;
        }

        SaveValuesToScriptableObject();
        EditorUtility.SetDirty(existingSO); //Saves changes made to this file
    }

    void CreatePrefab()
    {
        SheenTouch sheenTouch = FindObjectOfType<SheenTouch>();
        SheenJoystick sheenJoystick = FindObjectOfType<SheenJoystick>();
        if (!sheenTouch)
        {
            Object myPrefab = AssetDatabase.LoadAssetAtPath("Assets/Sheen/InputController/Prefabs/" + prefabName + ".prefab", typeof(GameObject));
            PrefabUtility.InstantiatePrefab(myPrefab);
            sheenTouch = FindObjectOfType<SheenTouch>();
            sheenJoystick = FindObjectOfType<SheenJoystick>();
        }

        sheenTouch.LoadValuesFromScriptableObject();
        sheenJoystick.LoadValuesFromScriptableObject();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = sheenTouch;
    }
}
#endif
