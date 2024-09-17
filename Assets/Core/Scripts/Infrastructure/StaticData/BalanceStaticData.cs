using System;
using UnityEngine;
using Utilities.Attributes;

namespace Infrastructure.StaticData
{
    [CreateAssetMenu(menuName = "StaticData/Balance", fileName = "Balance", order = 0)]
    public class BalanceStaticData : ScriptableObject
    {
        [Space(5)] [Header("Camera Config")]
        public CameraConfig Camera = new CameraConfig();
        [Space(5)] [Header("Item Config")]
        public ItemId DefaultItem;
        
        [Serializable]
        public class CameraConfig
        {
            public float Smoothing = 2f;
            public Vector3 Offset = new Vector3(0,2,-10);
        }
    }
}