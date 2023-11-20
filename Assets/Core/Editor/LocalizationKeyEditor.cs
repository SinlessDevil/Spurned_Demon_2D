using System.Collections.Generic;
using Infrastructure.Services.LocalizationService;
using UnityEngine;
using UnityEditor;

namespace Editor
{
    public class LocalizationKeyEditor : EditorWindow
    {
        private List<LocalizBase> localizScripts = new List<LocalizBase>();
        private Vector2 scrollPosition;

        [MenuItem("Tools/Localization Key Editor")]
        public static void Init()
        {
            LocalizationKeyEditor window = GetWindow<LocalizationKeyEditor>();
            window.titleContent = new GUIContent("Localization Key Editor");
            window.Show();
        }

        private void OnGUI()
        {
            DrawWindowWithBorder();

            TextLocalizationKey();

            ButtonFindLocalize();
            ButtonSaveChanged();
            ButtonClearWindow();

            UpdateWindow();

            CloseDrawWidow();
        }
        private void DrawWindowWithBorder()
        {
            GUILayout.Space(10);

            GUIStyle boxStyle = GUI.skin.box;
            GUILayout.BeginHorizontal(boxStyle);
            GUILayout.BeginVertical();
        }

        private void TextLocalizationKey()
        {
            GUILayout.Space(10);
            GUIStyle labelStyle = new GUIStyle(EditorStyles.boldLabel);
            labelStyle.fontSize = 18;
            labelStyle.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("Localization Key Editor", labelStyle, GUILayout.ExpandWidth(true));
            GUILayout.Space(10);
        }

        private void ButtonFindLocalize()
        {
            GUI.backgroundColor = Color.yellow;
            if (GUILayout.Button("Find all Localize"))
            {
                OnFindAllLocalize();
            }
        }
        private void ButtonSaveChanged()
        {
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Save Changed"))
            {
                OnSaveChanged();
            }
        }
        private void ButtonClearWindow()
        {
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Clear Window"))
            {
                OnClearAllLocalizationKeys();
            }
        }

        private void UpdateWindow()
        {
            GUI.backgroundColor = Color.gray;

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            List<LocalizBase> toRemove = new List<LocalizBase>();

            foreach (var script in localizScripts)
            {
                if (script == null || !EditorUtility.IsPersistent(script))
                {
                    toRemove.Add(script);
                    continue;
                }

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(script.name, GUILayout.Width(150));
                script.LocalizationKey = EditorGUILayout.TextField("Localization Key", script.LocalizationKey);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();

            foreach (var script in toRemove)
            {
                localizScripts.Remove(script);
            }
        }

        private static void CloseDrawWidow()
        {
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
        }

        private void OnFindAllLocalize()
        {
            localizScripts.Clear();

            string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Core/Resources" });

            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                LocalizBase[] scriptsInPrefab = prefab.GetComponentsInChildren<LocalizBase>(true);
                localizScripts.AddRange(scriptsInPrefab);
            }

            Repaint();
        }
        private void OnSaveChanged()
        {
            foreach (var localize in localizScripts)
            {
                if (GUI.changed)
                {
                    EditorUtility.SetDirty(localize);
                    AssetDatabase.SaveAssets();
                }
            }
        }
        private void OnClearAllLocalizationKeys()
        {
            localizScripts.Clear();

            Repaint();
        }
    }
}