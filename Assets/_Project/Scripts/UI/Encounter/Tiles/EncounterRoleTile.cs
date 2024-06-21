using System.Linq;
using _Project.ScriptableObjects;
using _Project.Scripts.Model;
using _Project.Scripts.UI.DragAndDrop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Encounter.Tiles
{
    public class EncounterRoleTile : MonoBehaviour, IAssignable<CrewMember>
    {
        public EncounterRole role;
        [SerializeField] private Image crewMemberImage;
        [SerializeField] private GameObject assignedBlock;
        [SerializeField] private GameObject unassignedBlock;
        [SerializeField] private Image assignedIcon;
        [SerializeField] private Image unassignedIcon;
        [SerializeField] private TextMeshProUGUI assignedText;
        [SerializeField] private TextMeshProUGUI unassignedText;

        public CrewMember CrewMember { get; private set; }

        public void Start()
        {
            assignedIcon.sprite = role.icon;
            unassignedIcon.sprite = role.icon;
            assignedText.text = role.roleName;
            unassignedText.text = role.roleName;
            EncounterUI.GetRoleTiles().Add(this);
        }

        public void OnDestroy()
        {
            EncounterUI.GetRoleTiles().Remove(this);
        }

        public bool CanDragFrom => CrewMember != null;

        public bool Assign(CrewMember crewMember)
        {
            var oldCrewMember = CrewMember;
            CrewMember = crewMember;
            crewMemberImage.sprite = crewMember.GreenSprite;
            assignedBlock.SetActive(true);
            unassignedBlock.SetActive(false);

            var encounterRoleTile = EncounterUI.GetRoleTiles()
                .FirstOrDefault(tile => tile.CrewMember == crewMember && tile != this);
            if (encounterRoleTile != null) encounterRoleTile.Unassign();
            if(oldCrewMember != null) 
                EncounterUI
                    .GetCrewMemberTiles()
                    .First(tile => tile.CrewMember == oldCrewMember)
                    .Assign(oldCrewMember);
            
            return true;
        }

        public (CrewMember, Sprite) Unassign()
        {
            assignedBlock.SetActive(false);
            unassignedBlock.SetActive(true);
            var crewMember = CrewMember;
            
            CrewMember = null;
            return (crewMember, crewMember?.GreenSprite);
        }
    }
}