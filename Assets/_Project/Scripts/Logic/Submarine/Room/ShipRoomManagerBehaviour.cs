using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Logic.Submarine.Crew;
using _Project.Scripts.Logic.Util;
using _Project.Scripts.Manager;
using _Project.Scripts.Manager.AudioSystem;
using _Project.Scripts.Model;
using UnityEngine;

namespace _Project.Scripts.Logic.Submarine.Room
{
    public class ShipRoomManagerBehaviour: MonoBehaviour
    {
        [SerializeField] private CrewMemberManagerBehaviour crewMemberManagerBehaviour;

        public Dictionary<ShipRoomName, List<CrewMember>> RoomsToCrewMembers { get; private set; } = new();
        private readonly Dictionary<string, ShipRoomName> _crewMembersToRooms = new();
        
        public static event EventHandler<(string crewMemberId, ShipRoomName roomName)> OnCrewMembersChangedRoom;

        private GameData _gameData;

        public void Awake()
        {
            _gameData = GameDataManager.Instance.GameData;
            var roomNames = Enum.GetValues(typeof(ShipRoomName)).Cast<ShipRoomName>().ToList();
            foreach (var roomName in roomNames)
            {
                RoomsToCrewMembers[roomName] = new List<CrewMember>();
            }

            var randomRooms = RandomCollectionUtil.GetRandomElementsFromCollection(roomNames, 3);
            for (var i = 0; i < _gameData.CrewMembers.Count; i++)
            {
                AddCrewMemberToRoom(_gameData.CrewMembers[i].Id, randomRooms[i]);
            }
            
            crewMemberManagerBehaviour.ShipRoomManagerBehaviour = this;
            crewMemberManagerBehaviour.Init(_crewMembersToRooms.Select(pair => (pair.Key, pair.Value)).ToList());
        }

        private void OnDestroy()
        {
            OnCrewMembersChangedRoom = null;
        }

        public void RemoveCrewMember(string crewMemberId)
        {
            if (_crewMembersToRooms.TryGetValue(crewMemberId, out var shipRoomName))
            {
                RemoveCrewMember(crewMemberId, shipRoomName);
                _crewMembersToRooms.Remove(crewMemberId);
            }
        }

        private void RemoveCrewMember(string crewMemberId, ShipRoomName shipRoomName)
        {
            var findIndex = RoomsToCrewMembers[shipRoomName].FindIndex(crewMember => crewMember.Id == crewMemberId);
            if (findIndex != -1)
            {
                RoomsToCrewMembers[shipRoomName].RemoveAt(findIndex);
                AudioManager.Pause(shipRoomName);
            }
        }

        public void AddCrewMemberToRoom(string crewMemberId, ShipRoomName shipRoomName)
        {
            if (_crewMembersToRooms.TryGetValue(crewMemberId, out var room))
            {
                RemoveCrewMember(crewMemberId, room);
            }
            AudioManager.Play(shipRoomName);
            RoomsToCrewMembers[shipRoomName].Add(GetCrewMemberById(crewMemberId));
            _crewMembersToRooms[crewMemberId] = shipRoomName;
            
            OnCrewMembersChangedRoom?.Invoke(this, (crewMemberId, shipRoomName));
        }

        public ShipRoomName GetShipRoomOfCrewMember(string crewMemberId)
        {
            return _crewMembersToRooms[crewMemberId];
        }
        
        public CrewMember GetCrewMemberById(string crewMemberId)
        {
            return CrewMemberUtil.GetCrewMemberById(_gameData, crewMemberId);
        }
        
        public List<CrewMember> GetCrewMembers(ShipRoomName shipRoomName)
        {
            return RoomsToCrewMembers[shipRoomName];
        }
    }
}