using System.Collections.Generic;
using Services.AudioService;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(menuName = "StaticData/Audio", fileName = "AudioConfig", order = 1)]
    public class AudioStaticData : ScriptableObject
    {
        public List<Sound> Sounds = new();
    }
}