using Plugins.SDK;
using UnityEditor;
using UnityEngine;

namespace Infrastructure.Editor
{
    [CustomEditor(typeof(Config))]
    public class SDKConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Install"))
            {
                ProjectSettingsInstaller.Install();
            }
        }
    }
}