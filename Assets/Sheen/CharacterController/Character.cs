using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class Character : MonoBehaviour
{
    [SerializeField] Transform customize;
    [SerializeField] Transform hair;
    [SerializeField] Transform eyebrows;
    [SerializeField] Transform chest;
    [SerializeField] Transform arms;
    [SerializeField] Transform legs;
    [SerializeField] Transform feet;

    [SerializeField] bool genderValue;
    [SerializeField] int hairValue;
    [SerializeField] int eyebrowsValue;
    [SerializeField] int chestValue;
    [SerializeField] int armsValue;
    [SerializeField] int legsValue;
    [SerializeField] int feetValue;

    string scriptableObjectName = "CharacterControllerSO";

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

    void RunCharacterLogic()
    {

    }

    public void AccessCharacter()
    {
        Transform characterReference = transform.GetChild(0);
        if (characterReference)
        {
            Transform male = transform.GetChild(0).GetChild(1);
            Transform female = transform.GetChild(0).GetChild(2);
            male.gameObject.SetActive(false);
            female.gameObject.SetActive(false);

            if (genderValue)
            {
                male.gameObject.SetActive(true);
                customize = male.GetChild(0);
            }   
            else
            {
                female.gameObject.SetActive(true);
                customize = female.GetChild(0);
            }
            
            hair = customize.GetChild(0);
            eyebrows = customize.GetChild(1);
            chest = customize.GetChild(2);
            arms = customize.GetChild(3);
            legs = customize.GetChild(4);
            feet = customize.GetChild(5);
        }
    }

    public void LoadValuesFromScriptableObject()
    {
        
        CharacterControllerSO existingSO = (CharacterControllerSO)Resources.Load<CharacterControllerSO>(scriptableObjectName);
        if (existingSO)
        {
            genderValue = existingSO.gender;
            hairValue = existingSO.hair;
            eyebrowsValue = existingSO.eyebrows;
            chestValue = existingSO.chest;
            armsValue = existingSO.arms;
            legsValue = existingSO.legs;
            feetValue = existingSO.feet;
        }

        AccessCharacter();

        Transform characterReference = transform.GetChild(0);
        if (characterReference)
        {
            OpenACustomizeProperty(hair, hairValue);
            OpenACustomizeProperty(eyebrows, eyebrowsValue);
            OpenACustomizeProperty(chest, chestValue);
            OpenACustomizeProperty(arms, armsValue);
            OpenACustomizeProperty(legs, legsValue);
            OpenACustomizeProperty(feet, feetValue);
        }
        
    }

    public void SaveValuesToScriptableObject()
    {
        CharacterControllerSO existingSO = (CharacterControllerSO)Resources.Load<CharacterControllerSO>(scriptableObjectName);
        if (existingSO)
        {
            existingSO.gender = genderValue;
            existingSO.hair = hairValue;
            existingSO.eyebrows = eyebrowsValue;
            existingSO.chest = chestValue;
            existingSO.arms = armsValue;
            existingSO.chest = legsValue;
            existingSO.arms = feetValue;
#if UNITY_EDITOR
            EditorUtility.SetDirty(existingSO); //Saves changes made to this file
#endif
        }
    }

    public void OpenACustomizeProperty(Transform objectReferance, int value)
    {
        int childcount = objectReferance.childCount;
        if (value >= childcount || value < 0)
            value = 0;

        //Close all active object
        for (int i = 0; i < childcount; i++)
        {
            objectReferance.GetChild(i).gameObject.SetActive(false);
        }

        //Open aim object
        objectReferance.GetChild(value).gameObject.SetActive(true);
    }

}

//#if UNITY_EDITOR
//[CustomEditor(typeof(Character))]
//public class CharacterEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();
//        GUILayout.Space(5);
//        Character character = (Character)target;
//        if (GUILayout.Button("Save"))
//        {
//            character.SaveValuesToScriptableObject();
//        }
//        GUILayout.Space(10);
//    }
//}
//#endif