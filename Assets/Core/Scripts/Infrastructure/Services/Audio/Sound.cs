using System;
using UnityEngine;

namespace Infrastructure.Services.AudioService
{
    [Serializable]
    public class Sound
    {
        public TypeSound typeSound;

        public AudioClip clip;

        [Range(0f, 1f)] public float volume;

        [Range(0.1f, 3f)] public float pitch;

        public bool loop;

        public bool playOnAwake;

        [HideInInspector] public AudioSource source;
    }
}