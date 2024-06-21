using System.Collections.Generic;
using _Project.Scripts.Logic.Submarine.Room;
using _Project.Scripts.Manager;
using _Project.Scripts.Model;
using _Project.Scripts.UI.Submarine.CrewMember;
using UnityEngine;

namespace _Project.Scripts.Logic.Submarine.Crew
{
    public class CrewMemberManagerBehaviour : MonoBehaviour
    {
        [SerializeField] private List<ShipRoomBehaviour> rooms;
        [SerializeField] private CrewMemberBehaviour crewMemberGameObjectTemplate;
        [SerializeField] private Canvas canvas;
        [SerializeField] private CrewMemberUICard crewMemberUICard;

        private readonly Dictionary<string, CrewMemberBehaviour> _crewMemberGameObjects = new();
        private bool _isDraggingCrewMember;

        public ShipRoomManagerBehaviour ShipRoomManagerBehaviour { get; set; }

        public void Init(List<(string id, ShipRoomName roomName)> crewMembersAssignment)
        {
            rooms.ForEach(room => room.ShipRoomManagerBehaviour = ShipRoomManagerBehaviour);
            foreach (var (crewMember, shipRoomName) in crewMembersAssignment)
            {
                var room = rooms.Find(room => room.roomName == shipRoomName);
                var behaviour = Instantiate(crewMemberGameObjectTemplate, room.transform);

                behaviour.Canvas = canvas;
                behaviour.Id = crewMember;
                behaviour.ShipRoomManagerBehaviour = ShipRoomManagerBehaviour;
                behaviour.OnHovered = () => OnCrewMemberHovered(crewMember);
                behaviour.OnCrewMemberStartDrag = OnCrewMemberStartDrag;
                behaviour.OnCrewMemberEndDrag = OnCrewMemberEndDrag;

                _crewMemberGameObjects.Add(crewMember, behaviour);
            }

            ShipBehaviour.OnCrewMemberDied += OnCrewMemberDied;
            ShipBehaviour.OnCrewMembersToBeUpdated += OnCrewMembersToBeUpdated;
        }

        private void OnCrewMemberDied(object sender, string id)
        {
            if (_crewMemberGameObjects.TryGetValue(id, out var go))
            {
                Destroy(go.gameObject);
            }
        }

        private void OnCrewMembersToBeUpdated(object sender, GameData gameData)
        {
            foreach (var crew in gameData.CrewMembers)
            {
                var card = _crewMemberGameObjects[crew.Id];
                card.SetHealth(crew.Health);
            }
        }

        private void OnCrewMemberStartDrag()
        {
            _isDraggingCrewMember = true;
            GameSceneManager.Slow(TimeScaleRequester.CrewMemberDrag,.25f);
        }

        private void OnCrewMemberEndDrag()
        {
            _isDraggingCrewMember = false;
            GameSceneManager.Resume(TimeScaleRequester.CrewMemberDrag);
        }

        private void OnCrewMemberHovered(string id)
        {
            if (_isDraggingCrewMember)
            {
                return;
            }

            var crewMember = ShipRoomManagerBehaviour.GetCrewMemberById(id);
            var roomName = ShipRoomManagerBehaviour.GetShipRoomOfCrewMember(id);
            
            crewMemberUICard.SetCrewMember(crewMember, roomName);
            crewMemberUICard.Show();
        }
    }
}