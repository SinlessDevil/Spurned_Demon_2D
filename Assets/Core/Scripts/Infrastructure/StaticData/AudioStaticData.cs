using System.Collections.Generic;
using Infrastructure.Services.AudioService;
using UnityEngine;

namespace Infrastructure.StaticData
{
    [CreateAssetMenu(menuName = "StaticData/Audio", fileName = "AudioConfig", order = 1)]
    public class AudioStaticData : ScriptableObject
    {
        public List<Sound> Sounds = new();
    }
}