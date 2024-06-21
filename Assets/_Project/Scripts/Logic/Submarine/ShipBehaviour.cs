using System;
using System.Linq;
using _Project.Scripts.Configuration;
using _Project.Scripts.Logic.Submarine.Room;
using _Project.Scripts.Logic.Util;
using _Project.Scripts.Manager;
using _Project.Scripts.Model;
using UnityEngine;

namespace _Project.Scripts.Logic.Submarine
{
    public class ShipBehaviour : MonoBehaviour
    {
        public static event EventHandler<GameData> OnGenericUIToBeUpdated;
        public static event EventHandler<GameData> OnCrewMembersToBeUpdated;
        public static event EventHandler<string> OnCrewMemberDied;
        public static event EventHandler<GameData> OnGameOver;
        
        [SerializeField] private ShipRoomManagerBehaviour shipRoomManagerBehaviour;

        private GameData _gameData;
        private GameParameters _parameters;
        private bool _isGameOver;
        
        private void Awake()
        {
            _gameData = GameDataManager.Instance.GameData;
            _parameters = GameParameters.Instance;
        }

        private void Start()
        {
            OnGenericUIToBeUpdated?.Invoke(this, _gameData);
            OnCrewMembersToBeUpdated?.Invoke(this, _gameData);
        }

        private void Update()
        {
            if (_isGameOver) return;

            UpdateShipResources();
            UpdateCrewMembersStats();
            UpdateAltimeter();
            
            IsGameOver();
        }

        private void FixedUpdate()
        {
            if (_isGameOver) return;

            OnGenericUIToBeUpdated?.Invoke(this, _gameData);
            OnCrewMembersToBeUpdated?.Invoke(this, _gameData);
        }

        private void OnDestroy()
        {
            OnGenericUIToBeUpdated = null;
            OnCrewMembersToBeUpdated = null;
            OnCrewMemberDied = null;
            OnGameOver = null;
        }
        
        private float UpdateSingleResource(ShipRoomName roomName, Func<CrewMember, float> skillMapper,
            float skillMultiplier, float decayRate)
        {
            /*TODO this is hard to read can't we just lerp between a highest and lowest progression speed
            Mathf.Lerp(
                 _parameters.LowestProgressSpeed,
                _parameters.HighestProgressSpeed,
                CrewMemberUtil.InvLerpCrewMemberSkill(skill)
            );
            */

            var assignedCrewMembers = shipRoomManagerBehaviour.GetCrewMembers(roomName);
            return assignedCrewMembers.Count == 0
                ? -decayRate * Time.deltaTime
                : assignedCrewMembers
                    .Select(skillMapper.Invoke)
                    .Select(skill => skillMultiplier * (_parameters.genericWorkSpeed + skill / CrewMember.SkillData.MAX_VALUE))
                    .Select(workSpeed => workSpeed * Time.deltaTime)
                    .Sum();
        }

        private void UpdateShipResources()
        {
            _gameData.Ship.EngineIntegrity += UpdateSingleResource(ShipRoomName.Engine,
                crewMember => crewMember.Skills.Engineer, _parameters.engineSkillMultiplier,
                _parameters.engineDecayRate);
            _gameData.Ship.Navigation += UpdateSingleResource(ShipRoomName.Navigation,
                crewMember => crewMember.Skills.Navigation, _parameters.navigationSkillMultiplier,
                _parameters.navigationDecayRate);
            _gameData.Ship.Oxygen += UpdateSingleResource(ShipRoomName.Oxygen, crewMember => crewMember.Skills.Medic,
                _parameters.oxygenSkillMultiplier, _parameters.oxygenDecayRate);
            _gameData.Ship.Ammunition += UpdateSingleResource(ShipRoomName.Weapons,
                crewMember => crewMember.Skills.Weapons, _parameters.weaponsSkillMultiplier, 0);

            _gameData.Ship.EngineIntegrity = Mathf.Clamp(_gameData.Ship.EngineIntegrity, Ship.RESOURCE_MIN_VALUE,
                Ship.RESOURCE_MAX_VALUE);
            _gameData.Ship.Navigation = Mathf.Clamp(_gameData.Ship.Navigation, Ship.RESOURCE_MIN_VALUE,
                Ship.RESOURCE_MAX_VALUE);
            _gameData.Ship.Oxygen =
                Mathf.Clamp(_gameData.Ship.Oxygen, Ship.RESOURCE_MIN_VALUE, Ship.RESOURCE_MAX_VALUE);
            
            _gameData.Ship.Speed = Mathf.Lerp(
                _parameters.minDescendSpeed,
                _parameters.maxDescendSpeed,
                Easing.EaseOutQuad(_gameData.Ship.Navigation / Ship.RESOURCE_MAX_VALUE)
            );
        }

        private void UpdateCrewMembersStats()
        {
            if (_gameData.Ship.Oxygen > _parameters.oxygenPenaltyStart)
            {
                return;
            }

            foreach (var crewMember in _gameData.CrewMembers)
            {
                crewMember.Health -= _parameters.oxygenCrewMemberHealthPenaltyRate * Time.deltaTime;
            }
        }

        private void UpdateAltimeter()
        {
            _gameData.Ship.Altimeter += _gameData.Ship.Speed * Time.deltaTime;
        }
        
        
        private void GameOver()
        {
            OnGameOver?.Invoke(this, _gameData);
            _isGameOver = true;
        }

        private void IsGameOver()
        {
            foreach (var crewMember in _gameData.CrewMembers.ToList().Where(crewMember => crewMember.Health < 0f + float.Epsilon))
            {
                KillCrewMember(crewMember);
            }

            if (_gameData.Ship.EngineIntegrity <= Ship.RESOURCE_MIN_VALUE || _gameData.CrewMembers.Count == 0)
            {
                GameOver();
            }
        }

        private void KillCrewMember(CrewMember crewMember)
        {
            shipRoomManagerBehaviour.RemoveCrewMember(crewMember.Id);
            _gameData.CrewMembers.Remove(crewMember);

            OnCrewMemberDied?.Invoke(this, crewMember.Id);
        }
    }
    
}