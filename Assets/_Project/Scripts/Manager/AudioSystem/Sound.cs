using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Manager.AudioSystem
{
    [Serializable]
    public class Sound
    {
        public string name;
        [SerializeField] private AudioClip clip;

        [SerializeField] [Range(0f, 1f)] private float baseVolume = 1;
        [SerializeField] [Range(.1f, 3f)] private float pitch = 1;
        public SoundType soundType;

        private float _volume;
        
        public float Volume
        {
            get => _volume;
            set
            {
                _volume = baseVolume * value;
                _source.volume = _volume;
            }
        }

        private AudioSource _source;

        public void Init(AudioSource source)
        {
            _source = source;
            _source.clip = clip;
            _source.volume = baseVolume;
            _source.pitch = pitch;
            _source.loop = soundType is SoundType.Ambience or SoundType.Theme;
        }
        public void Play() => _source.Play();
        public void Pause() => _source.Pause();
        public void Stop() => _source.Stop();
        public void Mute() => _source.mute = true;
        public void Unmute() => _source.mute = false;
    }

    public enum SoundType
    {
        OneShot, Ambience, Theme 
    }
}