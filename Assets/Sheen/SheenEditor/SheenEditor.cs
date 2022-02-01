using UnityEngine;
using UnityEditor;

public abstract class SheenEditor : Editor
{
	public static void Info(string message)
	{
		EditorGUILayout.HelpBox(message, MessageType.Info); // Help boxes can't display rich text for some reason, so strip it
	}

	public static void Warning(string message)
	{
		EditorGUILayout.HelpBox(message, MessageType.Warning); // Help boxes can't display rich text for some reason, so strip it
	}

	public static void Error(string message)
	{
		EditorGUILayout.HelpBox(message, MessageType.Error); // Help boxes can't display rich text for some reason, so strip it
	}

	public static void Separator()
	{
		EditorGUILayout.Separator();
	}

	public static void BeginIndent()
	{
		EditorGUI.indentLevel += 1;
	}

	public static void EndIndent()
	{
		EditorGUI.indentLevel -= 1;
	}

	public static bool Button(string text)
	{
		return GUILayout.Button(text);
	}

	public static bool HelpButton(string helpText, UnityEditor.MessageType type, string buttonText, float buttonWidth)
	{
		var clicked = false;

		EditorGUILayout.BeginHorizontal();
		{
			EditorGUILayout.HelpBox(helpText, type);

			var style = new GUIStyle(GUI.skin.button); style.wordWrap = true;

			clicked = GUILayout.Button(buttonText, style, GUILayout.ExpandHeight(true), GUILayout.Width(buttonWidth));
		}
		EditorGUILayout.EndHorizontal();

		return clicked;
	}

	public static void BeginDisabled(bool disabled = true)
	{
		EditorGUI.BeginDisabledGroup(disabled);
	}

	public static void EndDisabled()
	{
		EditorGUI.EndDisabledGroup();
	}
}

