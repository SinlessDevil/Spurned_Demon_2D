using UnityEngine;

namespace Infrastructure.StaticData
{
    [CreateAssetMenu(menuName = "StaticData/KeyWord", fileName = "KeyWordConfig", order = 0)]
    public class KeyWordsStaticData : ScriptableObject
    {
        public KeyCode AttackKey = KeyCode.Mouse0;
        public KeyCode AttackBlockKey = KeyCode.Mouse1;
        public KeyCode Ulta = KeyCode.R;
        public KeyCode InteractKey = KeyCode.E;
        public KeyCode OpenInventoryKey = KeyCode.I;
        public KeyCode OpenSettingsKey = KeyCode.Escape;
        public KeyCode JumpKey = KeyCode.Space;
    }
}