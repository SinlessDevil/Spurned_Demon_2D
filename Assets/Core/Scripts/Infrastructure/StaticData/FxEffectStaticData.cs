using System;
using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.StaticData
{
    [CreateAssetMenu(menuName = "StaticData/FxEffect", fileName = "FxEffectStaticData", order = 0)]
    public class FxEffectStaticData : ScriptableObject
    {
        public List<FxEffectConfig> FxEffectContainers = new();
    }

    [Serializable]
    public class FxEffectConfig
    {
        public FxTypeId FxType;
        public ParticleSystem FxPrefab;
    }

    public enum FxTypeId
    {
        Unknown = 0,
    }
}