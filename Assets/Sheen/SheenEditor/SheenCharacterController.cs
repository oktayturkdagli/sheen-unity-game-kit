#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public class SheenCharacterController : EditorWindow
{
    int selectedToolbarIndex = 0;
    Texture[] standartTextures, toolbarTextures;
    Texture[] hairTextures, eyebrowsTextures, chestTextures, armsTextures, legsTextures, feetTextures;

    Texture hair, eyebrows;
    Texture chest, arms;
    Texture legs, feet;

    int hairCounter = 0, eyebrowsCounter = 0;
    int chestCounter = 0, armsCounter = 0;
    int legsCounter = 0, feetCounter = 0;

    string scriptableObjectName = "CharacterControllerSO"; //Name of script to save InputController data
    string prefabName = "Sheen Character Controller"; //Name of prefab to be copied via InputController

    bool gender = true; // T -> Male, False -> Female

    [MenuItem("Window/Sheen/Sheen Character Controller")]
    static void Init()
    {
        GetWindow(typeof(SheenCharacterController));
    }

    void OnEnable()
    {
        TakeTextures();
        LoadValuesFromScriptableObject();
    }

    void OnGUI()
    {
        //Consists of a double toolbar menu
        selectedToolbarIndex = GUILayout.Toolbar(selectedToolbarIndex, toolbarTextures);
        switch (selectedToolbarIndex)
        {
            case 0:
                gender = true;
                TakeTextures();
                RunCharacterControllerLogic();
                break;
            case 1:
                gender = false;
                TakeTextures();
                RunCharacterControllerLogic();
                break;
            default:
                gender = true;
                TakeTextures();
                RunCharacterControllerLogic();
                break;
        }

        //Build Button
        EditorGUILayout.Space(); EditorGUILayout.Space();
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
        bool buttonBuild = GUILayout.Button(" Build ");
        GUILayout.EndHorizontal();
        if (buttonBuild)
        {
            CreateScriptableObject();
            CreatePrefab();
            this.Close(); //Closes the currently open custom editor window
        }
    }

    void RunCharacterControllerLogic()
    {
        EditorGUILayout.Space(10);
        if (gender)
        {
            GUILayout.Label("Character (Male)", EditorStyles.boldLabel);
        }
        else
        {
            GUILayout.Label("Character (Female)", EditorStyles.boldLabel);
        }
        EditorGUILayout.Space(15);

        //-----Head-----
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        //Hair
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUILayout.Height(105));
        GUILayout.FlexibleSpace(); bool button1L = GUILayout.Button(standartTextures[0], GUILayout.Width(25), GUILayout.Height(25)); GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.BeginVertical(GUILayout.Width(75));
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace(); GUILayout.Box("Hair", GUILayout.Width(75)); GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace(); GUILayout.Box(hair, GUILayout.Width(50), GUILayout.Height(50)); GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace(); GUILayout.Box(hairCounter.ToString(), GUILayout.Width(30)); GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.BeginVertical(GUILayout.Height(105));
        GUILayout.FlexibleSpace(); bool button1R = GUILayout.Button(standartTextures[1], GUILayout.Width(25), GUILayout.Height(25)); GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        //Mid Space
        GUILayout.FlexibleSpace(); 
        //Eyebrows
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUILayout.Height(105));
        GUILayout.FlexibleSpace(); bool button2L = GUILayout.Button(standartTextures[0], GUILayout.Width(25), GUILayout.Height(25)); GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.BeginVertical(GUILayout.Width(75));
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace(); GUILayout.Box("Eyebrows", GUILayout.Width(75)); GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace(); GUILayout.Box(eyebrows, GUILayout.Width(50), GUILayout.Height(50)); GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace(); GUILayout.Box(eyebrowsCounter.ToString(), GUILayout.Width(30)); GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.BeginVertical(GUILayout.Height(105));
        GUILayout.FlexibleSpace(); bool button2R = GUILayout.Button(standartTextures[1], GUILayout.Width(25), GUILayout.Height(25)); GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        //End Space
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        EditorGUILayout.Space(15);
        //-----Head-----


        //-----Body-----
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        //Chest
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUILayout.Height(105));
        GUILayout.FlexibleSpace(); bool button3L = GUILayout.Button(standartTextures[0], GUILayout.Width(25), GUILayout.Height(25)); GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.BeginVertical(GUILayout.Width(75));
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace(); GUILayout.Box("Chest", GUILayout.Width(75)); GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace(); GUILayout.Box(chest, GUILayout.Width(50), GUILayout.Height(50)); GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace(); GUILayout.Box(chestCounter.ToString(), GUILayout.Width(30)); GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.BeginVertical(GUILayout.Height(105));
        GUILayout.FlexibleSpace(); bool button3R = GUILayout.Button(standartTextures[1], GUILayout.Width(25), GUILayout.Height(25)); GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        //Mid Space
        GUILayout.FlexibleSpace();
        //Arms
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUILayout.Height(105));
        GUILayout.FlexibleSpace(); bool button4L = GUILayout.Button(standartTextures[0], GUILayout.Width(25), GUILayout.Height(25)); GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.BeginVertical(GUILayout.Width(75));
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace(); GUILayout.Box("Arms", GUILayout.Width(75)); GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace(); GUILayout.Box(arms, GUILayout.Width(50), GUILayout.Height(50)); GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace(); GUILayout.Box(armsCounter.ToString(), GUILayout.Width(30)); GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.BeginVertical(GUILayout.Height(105));
        GUILayout.FlexibleSpace(); bool button4R = GUILayout.Button(standartTextures[1], GUILayout.Width(25), GUILayout.Height(25)); GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        //End Space
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        EditorGUILayout.Space(15);
        //-----Body-----


        //-----Legs-----
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        //Legs
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUILayout.Height(105));
        GUILayout.FlexibleSpace(); bool button5L = GUILayout.Button(standartTextures[0], GUILayout.Width(25), GUILayout.Height(25)); GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.BeginVertical(GUILayout.Width(75));
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace(); GUILayout.Box("Legs", GUILayout.Width(75)); GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace(); GUILayout.Box(legs, GUILayout.Width(50), GUILayout.Height(50)); GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace(); GUILayout.Box(legsCounter.ToString(), GUILayout.Width(30)); GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.BeginVertical(GUILayout.Height(105));
        GUILayout.FlexibleSpace(); bool button5R = GUILayout.Button(standartTextures[1], GUILayout.Width(25), GUILayout.Height(25)); GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        //Mid Space
        GUILayout.FlexibleSpace();
        //Feet
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUILayout.Height(105));
        GUILayout.FlexibleSpace(); bool button6L = GUILayout.Button(standartTextures[0], GUILayout.Width(25), GUILayout.Height(25)); GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.BeginVertical(GUILayout.Width(75));
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace(); GUILayout.Box("Feet", GUILayout.Width(75)); GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace(); GUILayout.Box(feet, GUILayout.Width(50), GUILayout.Height(50)); GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace(); GUILayout.Box(feetCounter.ToString(), GUILayout.Width(30)); GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.BeginVertical(GUILayout.Height(105));
        GUILayout.FlexibleSpace(); bool button6R = GUILayout.Button(standartTextures[1], GUILayout.Width(25), GUILayout.Height(25)); GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        //End Space
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        EditorGUILayout.Space(5);
        //-----Legs-----

        //Head Buttons
        if (button1L)
        {
            hairCounter--;
            if (hairCounter < 0)
                hairCounter = hairTextures.Length - 1;
            hair = hairTextures[hairCounter];
        }
        else if (button1R)
        {
            hairCounter++;
            if (hairCounter > hairTextures.Length - 1)
                hairCounter = 0;
            hair = hairTextures[hairCounter];
        }
        else if (button2L)
        {
            eyebrowsCounter--;
            if (eyebrowsCounter < 0)
                eyebrowsCounter = eyebrowsTextures.Length - 1;
            eyebrows = eyebrowsTextures[eyebrowsCounter];
        }
        else if (button2R)
        {
            eyebrowsCounter++;
            if (eyebrowsCounter > eyebrowsTextures.Length - 1)
                eyebrowsCounter = 0;
            eyebrows = eyebrowsTextures[eyebrowsCounter];
        }

        //Body Buttons
        if (button3L)
        {
            chestCounter--;
            if (chestCounter < 0)
                chestCounter = chestTextures.Length - 1;
            chest = chestTextures[chestCounter];
        }
        else if (button3R)
        {
            chestCounter++;
            if (chestCounter > chestTextures.Length - 1)
                chestCounter = 0;
            chest = chestTextures[chestCounter];
        }
        else if (button4L)
        {
            armsCounter--;
            if (armsCounter < 0)
                armsCounter = armsTextures.Length - 1;
            arms = armsTextures[armsCounter];
        }
        else if (button4R)
        {
            armsCounter++;
            if (armsCounter > armsTextures.Length - 1)
                armsCounter = 0;
            arms = armsTextures[armsCounter];
        }

        //Legs Buttons
        if (button5L)
        {
            legsCounter--;
            if (legsCounter < 0)
                legsCounter = legsTextures.Length - 1;
            legs = legsTextures[legsCounter];
        }
        else if (button5R)
        {
            legsCounter++;
            if (legsCounter > legsTextures.Length - 1)
                legsCounter = 0;
            legs = legsTextures[legsCounter];
        }
        else if (button6L)
        {
            feetCounter--;
            if (feetCounter < 0)
                feetCounter = feetTextures.Length - 1;
            feet = feetTextures[feetCounter];
        }
        else if (button6R)
        {
            feetCounter++;
            if (feetCounter > feetTextures.Length - 1)
                feetCounter = 0;
            feet = feetTextures[feetCounter];
        }

    }

    void TakeTextures()
    {
        toolbarTextures = BringTexture("Assets/Sheen/Images/Character/Toolbar/");
        standartTextures = BringTexture("Assets/Sheen/Images/Arrows/");

        string genderType = "Male";
        if (!gender)
            genderType = "Female";

        hairTextures = BringTexture("Assets/Sheen/Images/Character/" + genderType + "/Hair/");
        eyebrowsTextures = BringTexture("Assets/Sheen/Images/Character/" + genderType + "/Eyebrows/");
        chestTextures = BringTexture("Assets/Sheen/Images/Character/" + genderType + "/Chest/");
        armsTextures = BringTexture("Assets/Sheen/Images/Character/" + genderType + "/Arms/");
        legsTextures = BringTexture("Assets/Sheen/Images/Character/" + genderType + "/Legs/");
        feetTextures = BringTexture("Assets/Sheen/Images/Character/" + genderType + "/Feet/");

        hair = hairTextures[hairCounter];
        eyebrows = eyebrowsTextures[eyebrowsCounter];
        chest = chestTextures[chestCounter];
        arms = armsTextures[armsCounter];
        legs = legsTextures[legsCounter];
        feet = feetTextures[feetCounter];
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
        CharacterControllerSO existingSO = (CharacterControllerSO)AssetDatabase.LoadAssetAtPath("Assets/Sheen/Resources/" + scriptableObjectName + ".asset", typeof(ScriptableObject));
        if (existingSO)
        {
            gender = existingSO.gender;
            hairCounter = existingSO.hair;
            eyebrowsCounter = existingSO.eyebrows;
            chestCounter = existingSO.chest;
            armsCounter = existingSO.arms;
            legsCounter = existingSO.legs;
            feetCounter = existingSO.feet;
        }
    }

    public void SaveValuesToScriptableObject()
    {
        CharacterControllerSO existingSO = (CharacterControllerSO)Resources.Load<CharacterControllerSO>(scriptableObjectName);
        if (existingSO)
        {
            existingSO.gender = gender;
            existingSO.hair = hairCounter;
            existingSO.eyebrows = eyebrowsCounter;
            existingSO.chest = chestCounter;
            existingSO.arms = armsCounter;
            existingSO.legs = legsCounter;
            existingSO.feet = feetCounter;
        }
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
            CharacterControllerSO characterControllerSOC = ScriptableObject.CreateInstance<CharacterControllerSO>();
            AssetDatabase.CreateAsset(characterControllerSOC, "Assets/Sheen/Resources/" + scriptableObjectName + ".asset");
            AssetDatabase.SaveAssets();
            existingSO = characterControllerSOC;
        }

        SaveValuesToScriptableObject();
        EditorUtility.SetDirty(existingSO); //Saves changes made to this file
    }

    void CreatePrefab()
    {
        Character character = FindObjectOfType<Character>();
        if (!character)
        {
            Object myPrefab = AssetDatabase.LoadAssetAtPath("Assets/Sheen/CharacterController/Prefabs/" + prefabName + ".prefab", typeof(GameObject));
            PrefabUtility.InstantiatePrefab(myPrefab);
            character = FindObjectOfType<Character>();
        }

        character.LoadValuesFromScriptableObject();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = character;
    }

}
#endif
