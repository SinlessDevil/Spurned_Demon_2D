using UnityEngine;

namespace Plugins.SDK
{
    [CreateAssetMenu(menuName = "SDK Config", fileName = "Config", order = 0)]
    public class Config : ScriptableObject
    {
        public bool IsInitialized;
        public string CompanyName = "Utin Computer";
        public string DefaultFolderName = "FOLDER_WITH_PROJECT";
        public string InitialScenePath = "Assets/FOLDER_WITH_PROJECT/Scenes/Initial.unity";
        public string FirstScenePath = "Assets/FOLDER_WITH_PROJECT/Scenes/First.unity";
    }
}