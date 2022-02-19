#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class SheenLevelController : EditorWindow
{
    [MenuItem("Window/Sheen/Sheen Level Controller")]
    public static void Init()
    {
        GetWindow(typeof(SheenLevelController));
    }

    void OnGUI()
    {

    }
}
#endif
