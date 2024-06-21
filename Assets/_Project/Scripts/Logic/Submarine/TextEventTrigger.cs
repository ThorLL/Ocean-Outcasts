using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Configuration;
using _Project.Scripts.Logic.Util;
using _Project.Scripts.Manager;
using _Project.Scripts.Model;
using _Project.Scripts.UI.TextEvent;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Logic.Submarine
{
    public class TextEventTrigger: MonoBehaviour
    {
        [SerializeField] private List<ScriptableObjects.TextEvent> events;
        
        [SerializeField] private ScriptableObjects.TextEvent lowNavigationEvent;
        
        [SerializeField] private TextEventUI textEventUI;
        
        private GameData _gameData;

        private GameParameters _parameters;

        private float _timerTriggerTextEventCurrent;
        private float _timerTriggerTextEventThreshold;
        
        private float _timerBrakeCurrent;

        private void Awake()
        {
            textEventUI.gameObject.SetActive(false);
            
            _gameData = GameDataManager.Instance.GameData;
            _parameters = GameParameters.Instance;

            _timerTriggerTextEventCurrent = 0f;
            _timerTriggerTextEventThreshold = GetRandomEncounterInterval();
        }

        private void Start()
        {
            InvokeRepeating(nameof(TriggerLowNavigationEvent), 2.0f, 3.0f);  //1s delay, repeat every 1s
        }

        private void Update()
        {
            if (Time.timeScale == 0.0f) return;
            TriggerRandomEvent();
        }
        
        private void TriggerLowNavigationEvent()
        {
            _timerBrakeCurrent -= Time.deltaTime;
            if (Time.timeScale == 0.0f || _timerBrakeCurrent > 0f || _gameData.Ship.Navigation > _parameters.lowNavigationThreshold) return;
            
            var random = Random.Range(0.0f, 1.0f);
            var threshold = Mathf.Lerp(0.0f, 0.40f, 1 - Easing.EaseInQuad(_gameData.Ship.Navigation / _parameters.lowNavigationThreshold));

            if (random < threshold)
            {
                TriggerEvent(lowNavigationEvent);
                _timerBrakeCurrent = _parameters.lowNavigationTextEventBrakeSeconds;
            }
        }
        
        private void TriggerRandomEvent()
        {
            _timerTriggerTextEventCurrent += Time.deltaTime;
            if (_timerTriggerTextEventCurrent <= _timerTriggerTextEventThreshold) return;
            
            var textEvent = GetRandomEncounter();

            TriggerEvent(textEvent);
        }
        
        private void TriggerEvent(ScriptableObjects.TextEvent textEvent)
        {
            GameSceneManager.Pause(TimeScaleRequester.TextEvent);

            textEventUI.Init(_gameData, textEvent);
            textEventUI.gameObject.SetActive(true);

            _timerTriggerTextEventCurrent = 0f;
            _timerTriggerTextEventThreshold = GetRandomEncounterInterval();
        }

        private float GetRandomEncounterInterval()
        {
            var variance = Random.Range(-_parameters.textEventVarianceSeconds, _parameters.textEventVarianceSeconds);
            return _parameters.textEventBaseIntervalSeconds + variance;
        }

        private ScriptableObjects.TextEvent GetRandomEncounter()
        {
            return RandomCollectionUtil.GetRandomElementsFromCollection(events, 1).First();
        }
    }
}