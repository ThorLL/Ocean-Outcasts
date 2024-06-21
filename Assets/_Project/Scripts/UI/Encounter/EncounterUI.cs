using System.Collections;
using System.Collections.Generic;
using _Project.ScriptableObjects;
using _Project.Scripts.Logic.Encounter;
using _Project.Scripts.Manager;
using _Project.Scripts.Manager.AudioSystem;
using _Project.Scripts.Model;
using _Project.Scripts.Model.Encounter;
using _Project.Scripts.UI.Encounter.TileCreators;
using _Project.Scripts.UI.Encounter.Tiles;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Encounter
{
    public class EncounterUI : MonoBehaviour
    {
        private static EncounterUI _current;
        [SerializeField] private TextMeshProUGUI encounterName;
        [SerializeField] private TextMeshProUGUI encounterDesc;
        [SerializeField] private Image encounterImage;
        [SerializeField] private Button fightButton;
        [SerializeField] private CrewMemberTilesCreator crewMemberTilesCreator;

        [SerializeField] private Slider torpedoSlider;
        [SerializeField] private TextMeshProUGUI torpedoCounter;
        [SerializeField] private TextMeshProUGUI torpedoText;

        private readonly EncounterBehaviour _encounterBehaviour = new();
        private List<CrewMemberTile> _crewMemberTiles = new();
        private MonsterEncounter _encounter;

        private List<EncounterRoleTile> _encounterRoleTiles = new();
        private GameData _gameData;

        [SerializeField] private GameObject encounterUIElements;
        [SerializeField] private GameObject outcomeUIElements;

        [SerializeField] private Button outcomeContinueButton;

        [SerializeField] private GameObject outcomeMessagePanel;
        [SerializeField] private GameObject outcomeCrewMemberCardPanel;
        
        [SerializeField] private OutcomeMessage messagePrefabTemplate;
        [SerializeField] private OutcomeCrewMemberCard outcomeCrewMemberCardPrefabTemplate;

        private List<GameObject> _destroyOnNextLoad = new();

        private void Awake()
        {
            _current = this;
            _current.fightButton.onClick.AddListener(OnFightClick);
            _current.outcomeContinueButton.onClick.AddListener(OnContinueClick);
        }

        public static void SetActive(bool isActive, GameData gameData = null, (MonsterEncounter monster, int level) encounter = default)
        {
            _current.gameObject.SetActive(isActive);
            if (!isActive) return;
            _current._destroyOnNextLoad.ForEach(Destroy);
            _current._destroyOnNextLoad.Clear();
            if (gameData != null && encounter.monster != null)
            {
                _current._gameData = gameData;
                _current._encounter = encounter.monster;
                _current.encounterName.text = _current._encounter.monsterName;
                _current.encounterDesc.text = _current._encounter.desc;
                _current.encounterImage.sprite = _current._encounter.sprite;
                _current.crewMemberTilesCreator.CreateCrewMemberTiles(_current._gameData.CrewMembers);
                _current.torpedoSlider.maxValue = Mathf.FloorToInt(_current._gameData.Ship.Ammunition);
                _current.torpedoSlider.value = _current.torpedoSlider.maxValue == 0 ? 0 : 1;
                _current.torpedoSlider.minValue = _current.torpedoSlider.maxValue == 0 ? 0 : 1;
                _current.torpedoSlider.onValueChanged.AddListener(UpdateTorpedoCount);
                _current.torpedoText.text = $"Ammunition {Mathf.FloorToInt(_current._gameData.Ship.Ammunition)}";
                _current.torpedoCounter.text =
                    $"{Mathf.FloorToInt(_current.torpedoSlider.value)}/{Mathf.FloorToInt(_current._gameData.Ship.Ammunition)}";
                
                var monsterSound = encounter.level switch
                {
                    1 => "SmallMonster",
                    2 => "MediumMonster",
                    3 => "BigMonster",
                    _ => ""
                };

                AudioManager.Play(monsterSound);
            }
            else
            {
                Debug.LogError("Must have a GameData and Encounter!");
            }
        }

        private static void OnFightClick()
        {
            var torpedoes = Mathf.FloorToInt(_current.torpedoSlider.value);
            var results = _current._encounterBehaviour.Fight(_current._gameData, _current._encounter, torpedoes);
            _current.encounterUIElements.SetActive(false);
            _current.outcomeUIElements.SetActive(true);
            
            AudioManager.Play("Torpedo");

            var wait = 1;

            _current.StartCoroutine(results.TorpedoesHit > 0
                ? AddMessageToPanel($"You hit {results.TorpedoesHit} torpedoes and {(results.TorpedoesHit < _current._encounter.health ? "did not " : "")} managed to kill the {_current._encounter.monsterName}", wait)
                : AddMessageToPanel("All of your torpedoes missed", wait));
            wait++;

            _current.StartCoroutine(!results.DodgedAttack
                ? AddMessageToPanel($"Your submarine got hit and lost {results.HullDamage:0}% Engine Integrity", wait)
                : AddMessageToPanel($"You dodged the {_current._encounter.monsterName}'s attack", wait));
            wait++;
            
            foreach (var (crewMember, result) in results.CrewMemberResults)
            {
                _current.StartCoroutine(crewMember.Health <= CrewMember.MIN_HEALTH + float.Epsilon
                    ? AddMessageToPanel($"{crewMember.Name} died", wait)
                    : AddCrewMemberToPanel(crewMember, result, wait));
                wait++;
            }
        }

        private static IEnumerator AddMessageToPanel(string text, float seconds)
        {
            yield return new WaitForSecondsRealtime(seconds * 0.5f);
            var instance = Instantiate(
                _current.messagePrefabTemplate,
                _current.outcomeMessagePanel.transform,
                worldPositionStays: false
            );
            instance.SetText($"> {text}");
            _current._destroyOnNextLoad.Add(instance.gameObject);
        }
        
        private static IEnumerator AddCrewMemberToPanel(CrewMember crewMember, CrewMemberEncounterResult result, float seconds)
        {
            yield return new WaitForSecondsRealtime(seconds * 0.5f);
            var instance = Instantiate(
                _current.outcomeCrewMemberCardPrefabTemplate,
                _current.outcomeCrewMemberCardPanel.transform,
                worldPositionStays: false
            );
            instance.Init(crewMember, result);
            _current._destroyOnNextLoad.Add(instance.gameObject);
        }
        
        private static void OnContinueClick()
        {
            _current.encounterUIElements.SetActive(true);
            _current.outcomeUIElements.SetActive(false);
            _current.gameObject.SetActive(false);
            GameSceneManager.Resume(TimeScaleRequester.Encounter);
        }

        private static void UpdateTorpedoCount(float torpedoes)
        {
            _current.torpedoCounter.text = $"{torpedoes}/{Mathf.FloorToInt(_current._gameData.Ship.Ammunition)}";
        }

        public static ref List<EncounterRoleTile> GetRoleTiles()
        {
            return ref _current._encounterRoleTiles;
        }

        public static ref List<CrewMemberTile> GetCrewMemberTiles()
        {
            return ref _current._crewMemberTiles;
        }

        public static ref List<GameObject> GetDestroyOnNextLoad() => ref _current._destroyOnNextLoad;
    }
}