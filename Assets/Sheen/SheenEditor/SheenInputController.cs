using UnityEngine;
using UnityEditor;
using System.IO;


public class SheenInputController : EditorWindow
{
    int selectedToolbarIndex = 0;
    Texture[] standartTextures, toolbarTextures, touchpadTextures;
    Sprite[] touchpadSprites;
    bool useTouch = true;
    float tapThreshold = 0.2f;

    bool useJoystick = true, fixedJoystick = true, alwaysDisplayJoystick = true;
    int joystickCenterCounter = 1, joystickKnobCounter = 5;
    Texture joystickCenterTexture, joystickKnobTexture;
    float joystickOutRange = 1f;

    //bool optionalSettingsEnable = false;
    //bool optionalToggle1 = true;
    //string optionalTextField1 = "Optional text";

    string scriptableObjectName = "InputControllerSO";
    string prefabName = "Sheen Input Controller";

    [MenuItem("Window/Sheen/Sheen Input Controller")]
    public static void Init()
    {
         GetWindow(typeof(SheenInputController));
    }

    private void OnEnable()
    {
        TakeTextures();
        LoadValuesFromScriptableObject();
    }

    void OnGUI()
    {
        selectedToolbarIndex = GUILayout.Toolbar(selectedToolbarIndex, toolbarTextures);
        switch (selectedToolbarIndex)
        {
            default:
            case 0:
                LogicTouchpad();
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
            //this.Close();
        }
    }

    void LogicTouchpad()
    {
        //Standart
        EditorGUILayout.Space();
        useTouch = EditorGUILayout.BeginToggleGroup("Touch", useTouch);
        tapThreshold = EditorGUILayout.Slider("Tap Treshold", tapThreshold, 0.01f, 1f);
        EditorGUILayout.EndToggleGroup();
        EditorGUILayout.Space(); EditorGUILayout.Space();

        //Joystick
        useJoystick = EditorGUILayout.BeginToggleGroup("Joystick", useJoystick);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUILayout.Height(65));GUILayout.FlexibleSpace();
        bool buttonCL = GUILayout.Button(standartTextures[0], GUILayout.Width(25), GUILayout.Height(25));
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        GUILayout.Box(" Center ");
        GUILayout.Box(joystickCenterTexture, GUILayout.Width(50), GUILayout.Height(50));
        GUILayout.EndVertical();
        GUILayout.BeginVertical(GUILayout.Height(65)); GUILayout.FlexibleSpace();
        bool buttonCR = GUILayout.Button(standartTextures[1], GUILayout.Width(25), GUILayout.Height(25));
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUILayout.Height(65)); GUILayout.FlexibleSpace();
        bool buttonKL = GUILayout.Button(standartTextures[0], GUILayout.Width(25), GUILayout.Height(25));
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        GUILayout.Box("   Knob  ");
        GUILayout.Box(joystickKnobTexture, GUILayout.Width(50), GUILayout.Height(50));
        GUILayout.EndVertical();
        GUILayout.BeginVertical(GUILayout.Height(65)); GUILayout.FlexibleSpace();
        bool buttonKR = GUILayout.Button(standartTextures[1], GUILayout.Width(25), GUILayout.Height(25));
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();
        joystickOutRange = EditorGUILayout.Slider("Outrange", joystickOutRange, 0.01f, 1f);
        fixedJoystick = EditorGUILayout.Toggle("Fixed Joystick", fixedJoystick);
        alwaysDisplayJoystick = EditorGUILayout.Toggle("Always Display", alwaysDisplayJoystick);
        EditorGUILayout.EndToggleGroup();
        EditorGUILayout.Space(); EditorGUILayout.Space();

        ////Optional Settings
        //optionalSettingsEnable = EditorGUILayout.BeginFoldoutHeaderGroup(optionalSettingsEnable, "Optional Settings");
        //if (optionalSettingsEnable)
        //{
        //    optionalToggle1 = EditorGUILayout.Toggle("Optional Toggle", optionalToggle1);
        //    optionalTextField1 = EditorGUILayout.TextField("Optional Text", optionalTextField1);
        //}
        //EditorGUILayout.EndFoldoutHeaderGroup();
        //EditorGUILayout.Space();

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
        toolbarTextures = new Texture[1];
        toolbarTextures[0] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/control_touch_black.png", typeof(Texture));
        //toolbarTextures[1] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/control_keyboard_black.png", typeof(Texture));
        //toolbarTextures[2] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/control_gamepad_black.png", typeof(Texture));

        standartTextures = new Texture[3];
        standartTextures[0] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/arrow_left_black.png", typeof(Texture));
        standartTextures[1] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/arrow_right_black.png", typeof(Texture));
        standartTextures[2] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/control_touch_black.png", typeof(Texture));
        
        touchpadTextures = new Texture[7];
        touchpadTextures[0] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/Joysticks/joystick_texture_1.png", typeof(Texture));
        touchpadTextures[1] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/Joysticks/joystick_texture_2.png", typeof(Texture));
        touchpadTextures[2] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/Joysticks/joystick_texture_3.png", typeof(Texture));
        touchpadTextures[3] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/Joysticks/joystick_texture_4.png", typeof(Texture));
        touchpadTextures[4] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/Joysticks/joystick_texture_5.png", typeof(Texture));
        touchpadTextures[5] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/Joysticks/joystick_texture_6.png", typeof(Texture));
        touchpadTextures[6] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/Joysticks/joystick_texture_7.png", typeof(Texture));

        touchpadSprites = new Sprite[7];
        touchpadSprites[0] = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/Joysticks/joystick_texture_1.png", typeof(Sprite));
        touchpadSprites[1] = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/Joysticks/joystick_texture_2.png", typeof(Sprite));
        touchpadSprites[2] = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/Joysticks/joystick_texture_3.png", typeof(Sprite));
        touchpadSprites[3] = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/Joysticks/joystick_texture_4.png", typeof(Sprite));
        touchpadSprites[4] = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/Joysticks/joystick_texture_5.png", typeof(Sprite));
        touchpadSprites[5] = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/Joysticks/joystick_texture_6.png", typeof(Sprite));
        touchpadSprites[6] = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/Joysticks/joystick_texture_7.png", typeof(Sprite));

        joystickCenterTexture = touchpadTextures[joystickCenterCounter];
        joystickKnobTexture = touchpadTextures[joystickKnobCounter];
    }

    public void LoadValuesFromScriptableObject()
    {
        InputControllerSOC existingSO = (InputControllerSOC)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Resources/" + scriptableObjectName + ".asset", typeof(ScriptableObject));
        if (existingSO)
        {
            useTouch = existingSO.useTouch;
            tapThreshold = existingSO.tapThreshold;
            useJoystick = existingSO.useJoystick;
            //joystickCenterTexture = existingSO.joystickCenterTexture;
            //joystickKnobTexture = existingSO.joystickKnobTexture;
            joystickOutRange = existingSO.joystickOutRange;
            fixedJoystick = existingSO.fixedJoystick;
            alwaysDisplayJoystick = existingSO.alwaysDisplayJoystick;
        }
    }

    public void SaveValuesToScriptableObject()
    {
        InputControllerSOC existingSO = (InputControllerSOC)Resources.Load<InputControllerSOC>(scriptableObjectName);
        if (existingSO)
        {
            existingSO.useTouch = useTouch;
            existingSO.tapThreshold = tapThreshold;
            existingSO.useJoystick = useJoystick;
            existingSO.joystickCenterSprite = touchpadSprites[joystickCenterCounter];
            existingSO.joystickKnobSprite = touchpadSprites[joystickKnobCounter];
            existingSO.joystickOutRange = joystickOutRange;
            existingSO.fixedJoystick = fixedJoystick;
            existingSO.alwaysDisplayJoystick = alwaysDisplayJoystick;   
        }
    }

    void CreateScriptableObject()
    {
        if (!Directory.Exists("Assets/Sheen/Resources/"))
        {
            Directory.CreateDirectory("Assets/Sheen/Resources/");
        }

        InputControllerSOC existingSO = (InputControllerSOC)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Resources/" + scriptableObjectName + ".asset", typeof(ScriptableObject));
        if (!existingSO)
        {
            InputControllerSOC inputControllerSOC = ScriptableObject.CreateInstance<InputControllerSOC>();
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

