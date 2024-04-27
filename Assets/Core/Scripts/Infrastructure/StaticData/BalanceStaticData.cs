using System;
using UnityEngine;

namespace Infrastructure.StaticData
{
    [CreateAssetMenu(menuName = "StaticData/Balance", fileName = "Balance", order = 0)]
    public class BalanceStaticData : ScriptableObject
    {
        public CameraConfig Camera = new CameraConfig();
        
        [Serializable]
        public class CameraConfig
        {
            public float Smoothing = 2f;
            public Vector3 Offset = new Vector3(0,2,-10);
        }
    }
}