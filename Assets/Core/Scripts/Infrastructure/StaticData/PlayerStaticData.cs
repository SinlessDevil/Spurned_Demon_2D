using UnityEngine;

namespace Infrastructure.StaticData
{
    [CreateAssetMenu(menuName = "StaticData/Entity/Player", fileName = "PlayerConfig", order = 4)]
    public class PlayerStaticData : ScriptableObject
    {
        public float MoveSpeed;
        public float JumpHeight;
    }
}