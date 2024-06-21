using System;
using System.Collections.Generic;
using System.Linq;
using _Project.ScriptableObjects;
using _Project.Scripts.Configuration;
using _Project.Scripts.Logic.Util;
using _Project.Scripts.Manager;
using _Project.Scripts.Model;
using _Project.Scripts.UI.Encounter;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Logic.Submarine
{
    public class EncounterTrigger : MonoBehaviour
    {
        [SerializeField] private List<MonsterEncounter> easyEncounters;
        [SerializeField] private List<MonsterEncounter> mediumEncounters;
        [SerializeField] private List<MonsterEncounter> hardEncounters;
        private GameData _gameData;

        private GameParameters _parameters;
        private bool _threatAlertTriggered;

        private float _timerTriggerEncounterCurrent;
        private float _timerTriggerEncounterThreshold;
        
        private float EncounterWave()
        {
            Func<float, float> invLerpDepth = depth => Mathf.InverseLerp(0, _parameters.encounterDepthFrequency,
                depth % _parameters.encounterDepthFrequency);
            
            var altimeterToEncounterFreq = Easing.EasingLerp(_parameters.minEncounterTime, _parameters.maxEncounterTime,
                invLerpDepth, Easing.EaseSineWave);
            
            return altimeterToEncounterFreq(_gameData.Ship.Altimeter);
        }
        
        private void Start()
        {
            EncounterUI.SetActive(false);
            _gameData = GameDataManager.Instance.GameData;
            _parameters = GameParameters.Instance;

            _timerTriggerEncounterCurrent = 0f;
            _timerTriggerEncounterThreshold = EncounterWave();
        }

        private void Update()
        {
            _timerTriggerEncounterCurrent += Time.deltaTime;

            if (!_threatAlertTriggered && _timerTriggerEncounterCurrent >=
                _timerTriggerEncounterThreshold - _parameters.encounterThreatAlert)
            {
                OnSetThreatAlertStatus?.Invoke(this, true);
                _threatAlertTriggered = true;
            }

            if (_timerTriggerEncounterCurrent <= _timerTriggerEncounterThreshold) return;


            // Disable threat alert as encounter is triggered
            OnSetThreatAlertStatus?.Invoke(this, false);

            EncounterUI.SetActive(true, _gameData, GetRandomEncounter());

            GameSceneManager.Pause(TimeScaleRequester.Encounter);

            // Reset timers so that we can trigger another encounter as soon as this one is over
            _timerTriggerEncounterCurrent = 0f;
            _timerTriggerEncounterThreshold = EncounterWave();
            _threatAlertTriggered = false;
        }

        private void OnDestroy()
        {
            OnSetThreatAlertStatus = null;
        }

        public static event EventHandler<bool> OnSetThreatAlertStatus;

        private (MonsterEncounter encounter, int level) GetRandomEncounter()
        {
            var zone = ZoneUtil.GetZoneFromDepth(_parameters.descendZones, _gameData.Ship.Altimeter);
            var totalChance = zone.easyEncounterChance + zone.mediumEncounterChance + zone.hardEncounterChance;
            var n = Random.Range(1, totalChance + 1); // Should be between 1 and 100 for all the zones I've made

            n -= zone.easyEncounterChance;
            if (n <= 0)
                return (RandomCollectionUtil.GetRandomElementsFromCollection(easyEncounters, 1).First(),1);

            n -= zone.mediumEncounterChance;
            if (n <= 0)
                return (RandomCollectionUtil.GetRandomElementsFromCollection(mediumEncounters, 1).First(),2);

            return (RandomCollectionUtil.GetRandomElementsFromCollection(hardEncounters, 1).First(),3);
        }
    }
}