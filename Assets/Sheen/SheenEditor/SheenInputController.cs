using UnityEditor;
using UnityEngine;
using Sheen.Touch;

public class SheenInputController : EditorWindow
{
    int selectedToolbarIndex = 0;
    Texture[] standartTextures, toolbarTextures, touchpadTextures;
    bool isFirstOpen = true;
    float referenceDpi = 200f, tapThreshold = 0.2f, swipeThreshold = 100f;

    bool joystickEnable = false;
    float JoystickOutRange = 2;

    bool optionalSettingsEnable = false;
    bool optionalToggle1 = true;
    string optionalTextField1 = "Optional text";

    string scriptableObjectName = "InputControllerSO";
    string prefabName = "SheenTouch";

    [MenuItem("Window/Sheen/Sheen Input Controller")]
    public static void Init()
    {
        GetWindow(typeof(SheenInputController));
    }

    void OnGUI()
    {
        if (isFirstOpen)
            FirstOpen();

        TakeTextures();
        selectedToolbarIndex = GUILayout.Toolbar(selectedToolbarIndex, toolbarTextures);
        switch (selectedToolbarIndex)
        {
            default:
            case 0:
                ControlTouchPad();
                break;
            case 1:
                Debug.Log("This part is not ready yet :)");
                break;
            case 2:
                Debug.Log("This part is not ready yet :)");
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
            CreateScriptableObject(scriptableObjectName);
            CreatePrefab(prefabName);
        }
    }

    void ControlTouchPad()
    {
        //Standart
        EditorGUILayout.Space();
        GUILayout.Label("Touch Controller", EditorStyles.boldLabel);
        referenceDpi = EditorGUILayout.FloatField("Reference DPI", referenceDpi);
        tapThreshold = EditorGUILayout.FloatField("Tap Treshold", tapThreshold);
        swipeThreshold = EditorGUILayout.FloatField("Swipe Treshold", swipeThreshold);
        EditorGUILayout.Space(); EditorGUILayout.Space();

        //Joystick
        joystickEnable = EditorGUILayout.BeginToggleGroup("Joystick", joystickEnable);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUILayout.Height(65));GUILayout.FlexibleSpace();
        bool buttonCL = GUILayout.Button(touchpadTextures[0], GUILayout.Width(25), GUILayout.Height(25));
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        GUILayout.Box(" Center ");
        GUILayout.Box(touchpadTextures[2], GUILayout.Width(50), GUILayout.Height(50));
        GUILayout.EndVertical();
        GUILayout.BeginVertical(GUILayout.Height(65)); GUILayout.FlexibleSpace();
        bool buttonCR = GUILayout.Button(touchpadTextures[1], GUILayout.Width(25), GUILayout.Height(25));
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUILayout.Height(65)); GUILayout.FlexibleSpace();
        bool buttonKL = GUILayout.Button(touchpadTextures[0], GUILayout.Width(25), GUILayout.Height(25));
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        GUILayout.Box("   Knob  ");
        GUILayout.Box(touchpadTextures[3], GUILayout.Width(50), GUILayout.Height(50));
        GUILayout.EndVertical();
        GUILayout.BeginVertical(GUILayout.Height(65)); GUILayout.FlexibleSpace();
        bool buttonKR = GUILayout.Button(touchpadTextures[1], GUILayout.Width(25), GUILayout.Height(25));
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        JoystickOutRange = EditorGUILayout.Slider("Outrange", JoystickOutRange, 0, 10);
        GUILayout.EndHorizontal();
        EditorGUILayout.EndToggleGroup();
        EditorGUILayout.Space(); EditorGUILayout.Space();

        //Optional Settings
        optionalSettingsEnable = EditorGUILayout.BeginToggleGroup("Optional Settings", optionalSettingsEnable);
        optionalToggle1 = EditorGUILayout.Toggle("Toggle 1", optionalToggle1);
        optionalTextField1 = EditorGUILayout.TextField("Text", optionalTextField1);
        EditorGUILayout.EndToggleGroup();
        EditorGUILayout.Space(); EditorGUILayout.Space();

        if (buttonCL)
        {
            Debug.Log("ButtonCL clicked");
        }
        else if (buttonCR)
        {
            Debug.Log("ButtonCR clicked");
        }
        else if (buttonKL)
        {
            Debug.Log("ButtonKL clicked");
        }
        else if (buttonKR)
        {
            Debug.Log("ButtonKR clicked");
        }

    }

    void FirstOpen()
    {
        TakeTextures();
        LoadValuesFromScriptableObject(scriptableObjectName);
        isFirstOpen = false;
    }

    void TakeTextures()
    {
            standartTextures = new Texture[1];
            standartTextures[0] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/control_touch_black.png", typeof(Texture));

            toolbarTextures = new Texture[1];
            toolbarTextures[0] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/control_touch_black.png", typeof(Texture));
            //toolbarTextures[1] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/control_keyboard_black.png", typeof(Texture));
            //toolbarTextures[2] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/control_gamepad_black.png", typeof(Texture));

            touchpadTextures = new Texture[4];
            touchpadTextures[0] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/arrow_left_black.png", typeof(Texture));
            touchpadTextures[1] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/arrow_right_black.png", typeof(Texture));
            touchpadTextures[2] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/Joysticks/joystick_center_1.png", typeof(Texture));
            touchpadTextures[3] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Images/Joysticks/joystick_knob_1.png", typeof(Texture));
    }

    void LoadValuesFromScriptableObject(string soName)
    {
        InputControllerSOC existingSO = (InputControllerSOC)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Resources/" + soName + ".asset", typeof(ScriptableObject));
        if (existingSO)
        {
            referenceDpi = existingSO.referenceDpi;
            tapThreshold = existingSO.tapThreshold;
            swipeThreshold = existingSO.swipeThreshold;
        }
    }
    
    void CreateScriptableObject(string soName)
    {
        InputControllerSOC existingSO = (InputControllerSOC)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Resources/" + soName + ".asset", typeof(ScriptableObject));
        if (!existingSO)
        {
            InputControllerSOC inputControllerSOC = ScriptableObject.CreateInstance<InputControllerSOC>();
            AssetDatabase.CreateAsset(inputControllerSOC, "Assets/Sheen/Resources/" + soName + ".asset");
            AssetDatabase.SaveAssets();
            existingSO = inputControllerSOC;
        }

        existingSO.referenceDpi = referenceDpi;
        existingSO.tapThreshold = tapThreshold;
        existingSO.swipeThreshold = swipeThreshold;

        EditorUtility.SetDirty(existingSO); //Saves changes made to this file
        LoadValuesFromScriptableObject(scriptableObjectName);
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = existingSO;
    }

    void CreatePrefab(string preName)
    {
        SheenTouch objectToFind = FindObjectOfType<SheenTouch>();
        if (!objectToFind)
        {
            Object myPrefab = AssetDatabase.LoadAssetAtPath("Assets/Sheen/InputController/" + preName + ".prefab", typeof(GameObject));
            PrefabUtility.InstantiatePrefab(myPrefab);
        }
    }

}

