using System.Collections.Generic;
using _Project.Scripts.Manager.AudioSystem;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Menu.Options
{
    public class VolumeSlider : MonoBehaviour
    {
        [SerializeField] private SoundType soundType;
        private Slider _slider;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
            _slider.onValueChanged.AddListener(value => AudioManager.SetVolume(value, new List<SoundType>{soundType}));
        }

        private void OnEnable()
        {
            _slider.value = AudioManager.GetVolume(soundType);
        }
    }
}
