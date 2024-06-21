using System;
using _Project.Scripts.Model;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Project.Scripts.UI.SelectCrew
{
    public class SelectCrewMemberDescription : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private GameObject border;

        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI bioText;
        [SerializeField] private GameObject navigationBar;
        [SerializeField] private GameObject engineerBar;
        [SerializeField] private GameObject medicBar;
        [SerializeField] private GameObject weaponsBar;

        [SerializeField] private TextMeshProUGUI buttonText;

        [SerializeField] private string removedText = "SELECT";
        [SerializeField] private string selectedText = "DESELECT";
        
        public CrewMember CrewMember { set; get; }
        public bool Selected { set; get; }

        public Action<SelectCrewMemberDescription> OnPrisonerSelected { get; set; }

        private void FillBar(float skill, GameObject bar)
        {
            for (var i = 1; i <= skill; i++)
            {
                var child = bar.transform.GetChild(i);
                child.gameObject.SetActive(true);
            }
        }
        
        private void Start()
        {
            Selected = false;

            image.sprite = CrewMember.GreenSprite;
            nameText.text = "[ " + CrewMember.Name + " ]";
            bioText.text = CrewMember.Bio;

            var skills = CrewMember.Skills;
            FillBar(skills.Navigation, navigationBar);
            FillBar(skills.Engineer, engineerBar);
            FillBar(skills.Medic, medicBar);
            FillBar(skills.Weapons, weaponsBar);
        }

        public void ButtonPressed()
        {
            OnPrisonerSelected(this);
            ChangeStyle();
        }

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            ButtonPressed();
        }

        private void ChangeStyle()
        {
            border.SetActive(Selected);
            buttonText.text = Selected ? selectedText : removedText;
        }
    }
}