#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public class SheenLevelController : EditorWindow
{

    [MenuItem("Window/Sheen/Sheen Level Controller")]
    static void Init()
    {
        GetWindow(typeof(SheenLevelController));
    }

    void OnEnable()
    {

    }

    void OnGUI()
    {
       
    }

    public void LoadValuesFromScriptableObject()
    {

    }

    public void SaveValuesToScriptableObject()
    {

    }

}
#endif
