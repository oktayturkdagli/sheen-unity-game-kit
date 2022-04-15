#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class Test : EditorWindow
{
    [MenuItem("Window/Sheen/Test")]
    static void Init()
    {
        EditorWindow editorWindow = GetWindow(typeof(Test), false, "Test", true);
        editorWindow.minSize = new Vector2(500, 650);
        editorWindow.maxSize = new Vector2(500, 650);
    }

    void OnGUI()
    {
		Test1();
    }

    void Test1()
    {
        Rect clickArea = EditorGUILayout.GetControlRect();

        Event current = Event.current;

        if (clickArea.Contains(current.mousePosition) && current.type == EventType.ContextClick)
        {
            //Do a thing, in this case a drop down menu

            GenericMenu menu = new GenericMenu();

            menu.AddDisabledItem(new GUIContent("I clicked on a thing"));
            menu.AddItem(new GUIContent("Do a thing"), false, YourCallback);
            menu.ShowAsContext();

            current.Use();
        }

        void YourCallback()
        {
            Debug.Log("Hi there");
        }
    }





}
#endif
