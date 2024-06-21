using System.Linq;
using _Project.Scripts.Model;
using _Project.Scripts.UI.DragAndDrop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Encounter.Tiles
{
    public class CrewMemberTile : MonoBehaviour, IAssignable<CrewMember>
    {
        [SerializeField] public Image image;
        [SerializeField] public TextMeshProUGUI nameField;
        [SerializeField] public Image healthBar;
        [SerializeField] private GameObject navigationBar;
        [SerializeField] private GameObject engineerBar;
        [SerializeField] private GameObject medicBar;
        [SerializeField] private GameObject weaponsBar;
        [SerializeField] private Image assignedShader;

        public CrewMember CrewMember;

        private void Awake()
        {
            EncounterUI.GetCrewMemberTiles().Add(this);
        }

        public void OnDestroy()
        {
            EncounterUI.GetCrewMemberTiles().Remove(this);
        }

        public bool CanDragFrom => !assignedShader.isActiveAndEnabled;

        public bool Assign(CrewMember crewMember)
        {
            if(EncounterUI.GetRoleTiles().All(tile => tile.CrewMember != crewMember))
                EncounterUI
                    .GetCrewMemberTiles()
                    .First(tile => tile.CrewMember == crewMember).assignedShader.gameObject
                    .SetActive(false);

            return true;
        }

        public (CrewMember, Sprite) Unassign()
        {
            assignedShader.gameObject.SetActive(true);
            return (CrewMember, CrewMember.GreenSprite);
        }


        public void Init(CrewMember crewMember)
        {
            CrewMember = crewMember;
            image.sprite = CrewMember.GreenSprite;
            nameField.text = CrewMember.Name;
            healthBar.fillAmount = CrewMember.Health / 100.0f;
            var skills = CrewMember.Skills;
            FillBar(skills.Navigation, navigationBar);
            FillBar(skills.Engineer, engineerBar);
            FillBar(skills.Medic, medicBar);
            FillBar(skills.Weapons, weaponsBar);
        }

        private static void FillBar(float skill, GameObject bar)
        {
            for (var i = 1; i <= Mathf.CeilToInt(skill); i++)
            {
                var child = bar.transform.GetChild(i);
                child.gameObject.SetActive(true);
                child.GetComponent<Image>().fillAmount = skill - i + 1;
            }
        }
    }
}