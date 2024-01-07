using UnityEngine;
using UnityEngine.Serialization;

namespace Infrastructure.StaticData
{
    [CreateAssetMenu(menuName = "StaticData/Player", fileName = "PlayerConfig", order = 4)]
    public class PlayerStaticData : ScriptableObject
    {
        public GameObject Prefab;
        public float MoveSpeed;
        public float JumpHeight;
    }
}