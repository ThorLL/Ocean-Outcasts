using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Logic.Submarine.Room;
using _Project.Scripts.Model;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Manager.AudioSystem
{
    public class AudioManager : MonoBehaviour
    {
        public Sound[] sounds;
        private static AudioManager _instance;
        private bool _isAmbienceFrozen;
        private readonly Dictionary<SoundType, float> _volumeMap = new();
        
        private void Awake()
        {
            if (_instance != null) return;
            _instance = this;
            DontDestroyOnLoad(transform.gameObject);
            ShipRoomManagerBehaviour.OnCrewMembersChangedRoom += OnCrewMembersChangedRoom;
            foreach (var sound in sounds)
            {
                var source = gameObject.AddComponent<AudioSource>();
                sound.Init(source);
            }
            GetAffectedTypes(new List<SoundType> { SoundType.Theme }).ToList().ForEach(sound => sound.Play());
        }

        private void LateUpdate()
        {
            FreezeAmbience();
        }

        private static void FreezeAmbience()
        {
            if (Time.timeScale == 0 && !_instance._isAmbienceFrozen)
            {
                GetAffectedTypes(new List<SoundType> { SoundType.Ambience }).ToList().ForEach(sound => sound.Mute());
                _instance._isAmbienceFrozen = true;
                return;
            }

            if (Time.timeScale != 0 && _instance._isAmbienceFrozen)
            {
                _instance._isAmbienceFrozen = false;
                GetAffectedTypes(new List<SoundType> { SoundType.Ambience }).ToList().ForEach(sound => sound.Unmute());
            }
        }

        private static void OnCrewMembersChangedRoom(object sender, (string crewMemberId, ShipRoomName roomName) data)
        {
            var s = (ShipRoomManagerBehaviour)sender;
            s.RoomsToCrewMembers.ToList()
                .Where(pair => pair.Value.Count == 0)
                .Select(e=> e.Key).ToList()
                .ForEach(Pause);
            s.RoomsToCrewMembers.ToList()
                .Where(pair => pair.Value.Count != 0)
                .Select(e => e.Key).ToList()
                .ForEach(Play);
        }
        
        private static Sound GetSound(string soundName)
        {
            return _instance.sounds.FirstOrDefault(sound => sound.name == soundName);
        }

        public static void Play(string soundName) => GetSound(soundName).Play();
        public static void Pause(string soundName) => GetSound(soundName).Pause();
        public static void Play(ShipRoomName roomName) => Play(MapRoomToSound(roomName));
        public static void Pause(ShipRoomName roomName) => Pause(MapRoomToSound(roomName));

        public static void StopAll()
        {
            GetAffectedTypes(new List<SoundType> { SoundType.Ambience, SoundType.OneShot}).ToList().ForEach(sound => sound.Stop());
        }
        
        public static void SetVolume(float volume, List<SoundType> affectedTypes)
        {
            affectedTypes.ForEach(soundType => _instance._volumeMap[soundType] = volume);
            GetAffectedTypes(affectedTypes).ToList().ForEach(sound => sound.Volume = volume);
        }

        public static float GetVolume(SoundType soundType)
        {
            return _instance._volumeMap.GetValueOrDefault(soundType, 1);
        }
        
        private static IEnumerable<Sound> GetAffectedTypes(List<SoundType> affectedTypes = null)
        {
            affectedTypes ??= Enum.GetValues(typeof(SoundType)).OfType<SoundType>().ToList();
            return _instance.sounds.ToList().Where(sound => affectedTypes.Contains(sound.soundType));
        }  
        
        private static string MapRoomToSound(ShipRoomName roomName) => roomName switch
        {
            ShipRoomName.Engine => "EngineRoom",
            ShipRoomName.Navigation => "NavigationRoom",
            ShipRoomName.Weapons => "WeaponsRoom",
            ShipRoomName.Oxygen => "OxygenRoom",
            _ => ""
        };
    }
}
