using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace UnityToolbarExtender.Examples
{
    static class ToolbarStyles
    {
        public static readonly GUIStyle commandButtonStyle;

        static ToolbarStyles()
        {
            commandButtonStyle = new GUIStyle("Command")
            {
                fontSize = 16,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove,
                fontStyle = FontStyle.Bold
            };
        }
    }

    [InitializeOnLoad]
    public class SceneSwitchLeftButton
    {
        static SceneSwitchLeftButton()
        {
            ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
        }

        static void OnToolbarGUI()
        {
            GUILayout.FlexibleSpace();

            string JAButtonText = "JA";
            string WoosungBntText = "WoosungTest";
            GUIContent JAButtonContent = new GUIContent(JAButtonText);
            Vector2 ButtonSize = GUI.skin.button.CalcSize(JAButtonContent);
            if (GUILayout.Button(JAButtonContent, GUILayout.Width(ButtonSize.x), GUILayout.Height(ButtonSize.y)))
            {
                SceneHelper.OpenScene(JAButtonText);
            }

            GUIContent WoosungBntContent = new GUIContent(WoosungBntText);
            ButtonSize = GUI.skin.button.CalcSize(WoosungBntContent);
            if (GUILayout.Button(WoosungBntContent, GUILayout.Width(ButtonSize.x), GUILayout.Height(ButtonSize.y)))
            {
                SceneHelper.OpenScene(WoosungBntText);
            }
        }
    }

    static class SceneHelper
    {
        public static void OpenScene(string name)
        {
            var saved = EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            if (saved)
            {
                _ = EditorSceneManager.OpenScene($"Assets/3. Scenes/{name}.unity");
            }
        }
    }
}