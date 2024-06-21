using _Project.Scripts.Logic.Submarine.Crew;
using _Project.Scripts.Model;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Scripts.Logic.Submarine.Room
{
    public class ShipRoomBehaviour : MonoBehaviour, IDropHandler 
    {
        
        public ShipRoomManagerBehaviour ShipRoomManagerBehaviour { get; set; }
        public ShipRoomName roomName;
        
        public void OnDrop(PointerEventData eventData)
        {
            var memberCrew = eventData.pointerDrag.GetComponent<CrewMemberBehaviour>();
            if (memberCrew == null)
            {
                return;
            }
            
            ShipRoomManagerBehaviour.AddCrewMemberToRoom(memberCrew.Id, roomName);
            eventData.pointerDrag.transform.SetParent(transform);
        }
        
    }
}
