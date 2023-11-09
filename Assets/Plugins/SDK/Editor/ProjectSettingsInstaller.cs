using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Plugins.SDK
{
    [InitializeOnLoad]
    public class ProjectSettingsInstaller
    {
        public const string EditorConfigPath = "Editor/Config";
        
        private static Config _config;

        static ProjectSettingsInstaller()
        {
            Install();
        }

        public static void Install()
        {
            _config = LoadConfig();

            if (_config is null || _config.IsInitialized)
                return;

            SetProjectSettings();
            AddBaseSceneToBuildSettings();
            //ChangeProjectFolderName();

            _config.IsInitialized = true;

            AssetDatabase.SaveAssets();
        }

        private static void SetProjectSettings()
        {
            PlayerSettings.companyName = _config.CompanyName;
            EditorSettings.serializationMode = SerializationMode.ForceText;
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel22;
        }

        private static void AddBaseSceneToBuildSettings()
        {
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            
            scenes = scenes
                .Append(new EditorBuildSettingsScene(_config.InitialScenePath, true))
                .Append(new EditorBuildSettingsScene(_config.FirstScenePath, true))
                .ToArray();
            
            EditorBuildSettings.scenes = scenes;
        }

        private static void ChangeProjectFolderName()
        {
            string sourcePath = "Assets/" + _config.DefaultFolderName;
            string destinationPath = "Assets/" +GetFolderName();
            
            AssetDatabase.MoveAsset(sourcePath, destinationPath);
        }

        private static string GetFolderName()
        {
            string[] productNameWords = Application.productName.Split(' ');
            productNameWords = productNameWords
                .Select(x =>
                {
                    string word = String.Empty;

                    if (x.Any())
                        word += char.ToUpper(x[0]);

                    if (x.Length > 1)
                        word += x.Substring(1);

                    return word;
                })
                .ToArray();

            return string.Join("", productNameWords);
        }

        private static Config LoadConfig() =>
            Resources.Load<Config>(EditorConfigPath);
    }
}