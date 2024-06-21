using System;
using System.Linq;
using UnityEngine;

namespace _Project.Scripts.Manager.AudioSystem.Triggers
{
    public class OnAwakeAudioTrigger : MonoBehaviour
    {
        private enum PlayMode
        {
            Play, Pause, Nothing
        }

        [Serializable]
        private class SoundName
        {
            public string name;
        }
        
        [SerializeField] private SoundName[] soundNames;
        [SerializeField] private PlayMode onAwake;
        [SerializeField] private PlayMode onDisable;

        private void OnEnable() => PlayPause(onAwake);

        private void OnDisable() => PlayPause(onDisable);

        private void PlayPause(PlayMode playMode)
        {
            switch (playMode)
            {
                case PlayMode.Play:
                    soundNames.Select(s=>s.name).ToList().ForEach(AudioManager.Play);
                    break;
                case PlayMode.Pause:
                    soundNames.Select(s=>s.name).ToList().ForEach(AudioManager.Pause);
                    break;
            }
        }
    }
    
}