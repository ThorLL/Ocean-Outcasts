using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Logic.Submarine;
using _Project.Scripts.Logic.Submarine.Room;
using _Project.Scripts.Logic.Util;
using _Project.Scripts.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Submarine.CrewMember
{
    [Serializable]
    public class RoomImage
    {
        public Sprite sprite;
        public ShipRoomName roomName;
    }
    
    public class CrewMemberUICard : MonoBehaviour
    {
        [SerializeField] private Image roomImage;
        [SerializeField] private AspectRatioFitter roomAspectRatioFitter;
        [SerializeField] private Image pictureImage;
        [SerializeField] private Image healthBarImage;
        [SerializeField] private TextMeshProUGUI statusText;
        
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI bioText;
        
        [SerializeField] private CrewMemberUICardStats weaponsUICardStats;
        [SerializeField] private CrewMemberUICardStats medicUICardStats;
        [SerializeField] private CrewMemberUICardStats navigationUICardStats;
        [SerializeField] private CrewMemberUICardStats engineerUICardStats;
        
        [SerializeField] private List<RoomImage> roomImages;

        private const string _CREW_MEMBER_JOB_STATUS = "[Working in the {0} Room]";
        
        private Dictionary<ShipRoomName, Sprite> _roomImages = new ();
        private string _shownCrewMemberId;
        
        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        private void Awake()
        {
            _roomImages = roomImages.ToDictionary(x => x.roomName, x => x.sprite);
            
            ShipBehaviour.OnCrewMembersToBeUpdated += OnCrewMembersToBeUpdated;
            ShipBehaviour.OnCrewMemberDied += OnCrewMemberDied;
            
            ShipRoomManagerBehaviour.OnCrewMembersChangedRoom += OnCrewMembersChangedRoom;
            
            gameObject.SetActive(false);
        }
        
        private void OnCrewMembersChangedRoom(object sender, (string crewMemberId, ShipRoomName roomName) data)
        { 
            if (_shownCrewMemberId != data.crewMemberId)
            {
                return;
            }
            SetRoom(data.roomName);
        }

        private void OnCrewMemberDied(object sender, string id)
        {
            if (_shownCrewMemberId != id)
            {
                return;
            }
            gameObject.SetActive(false);
            _shownCrewMemberId = null;
        }

        private void OnCrewMembersToBeUpdated(object sender, GameData gameData)
        {
            var crewMember = CrewMemberUtil.GetCrewMemberById(gameData, _shownCrewMemberId);
            if (crewMember != null)
            {
                SetHealth(crewMember.Health);
            }
        }

        
        private void SetHealth(float health)
        {
            healthBarImage.fillAmount = health / Model.CrewMember.MAX_HEALTH;
        }
        
        private void SetRoom(ShipRoomName roomName)
        {
            roomImage.sprite = _roomImages[roomName];
            roomAspectRatioFitter.aspectRatio = roomImage.sprite.rect.width / roomImage.sprite.rect.height;
            statusText.text = string.Format(_CREW_MEMBER_JOB_STATUS, roomName);
        }
        
        
        public void SetCrewMember(Model.CrewMember crewMember, ShipRoomName roomName)
        {
            _shownCrewMemberId = crewMember.Id;
            
            pictureImage.sprite = crewMember.NeutralSprite;
            
            SetRoom(roomName);
            SetHealth(crewMember.Health);
            
            nameText.text = crewMember.Name;
            bioText.text = crewMember.Bio;
            
            weaponsUICardStats.SetSkill(crewMember.Skills.Weapons, crewMember.InitialSkills.Weapons);
            medicUICardStats.SetSkill(crewMember.Skills.Medic, crewMember.InitialSkills.Medic);
            navigationUICardStats.SetSkill(crewMember.Skills.Navigation, crewMember.InitialSkills.Navigation);
            engineerUICardStats.SetSkill(crewMember.Skills.Engineer, crewMember.InitialSkills.Engineer);
        }
        
    }
}
