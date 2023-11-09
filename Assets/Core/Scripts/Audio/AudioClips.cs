using System;
using UnityEngine;

namespace Audio
{
    public class AudioClips : MonoBehaviour
    {
        public static AudioClips Instance;

        public Sound[] sounds;

        private void Awake()
        {
            Debug.Log("AudioClips is Load");

            Instance = this;

            DontDestroyOnLoad(this.gameObject);

            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.playOnAwake = s.playOnAwake;
            }
        }

        #region Clip Methods

        public void PlayClip(string name)
        {
            FindSound(name).source.Play();
        }
        public void StopClip(string name)
        {
            FindSound(name).source.Stop();
        }

        private Sound FindSound(string nameSound)
        {
            Sound s = Array.Find(sounds, sound => sound.name == nameSound);
            if (s is null){
                Debug.LogWarning("Sound:" + nameSound + "not found!");
                return null;
            }
            return s;
        }
        #endregion
    }
}