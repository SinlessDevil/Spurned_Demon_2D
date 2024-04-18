using UnityEngine;

namespace  Infrastructure.StaticData
{
    [CreateAssetMenu(menuName = "StaticData/PathResources", fileName = "PathConfig", order = 4)]
    public class PathResourcesStaticData : ScriptableObject
    {
        [Header("Entities")]
        public string PlayerPath = "Entities/Player";
        [Header("UI Path")]
        public string UiRootPath = "UI/UiRoot";
        public string GameHudPath = "UI/Hud/GameHud";
        public string MenuHudPath = "UI/Hud/MenuHud";
    }
}