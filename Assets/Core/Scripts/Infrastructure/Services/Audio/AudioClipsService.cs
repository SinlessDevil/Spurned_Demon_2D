using System;
using UnityEngine;

namespace Infrastructure.Services.AudioService
{
    public class AudioClipsService : IAudioClipsService
    {
        public Sound[] Sounds { get; set; }

        public void PlayClip(TypeSound typeSound)
        {
            FindSound(typeSound).source.Play();
        }
        public void StopClip(TypeSound typeSound)
        {
            FindSound(typeSound).source.Stop();
        }

        private Sound FindSound(TypeSound typeSound)
        {
            Sound s = Array.Find(Sounds, sound => sound.typeSound == typeSound);
            if (s is null)
            {
                Debug.LogWarning($"Sound:" + typeSound + " not found!");
                return null;
            }
            return s;
        }
    }
}