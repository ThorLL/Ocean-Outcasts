using System;
using _Project.ScriptableObjects;
using _Project.Scripts.Model;
using _Project.Scripts.Model.Encounter;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Encounter
{
    public class OutcomeCrewMemberCard : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI nameField;
        [SerializeField] private Image healthBar;
        [SerializeField] private Image healthLostBar;
        [SerializeField] private TextMeshProUGUI skillChangedAmountField;
        [SerializeField] private TextMeshProUGUI skillNameField;
        [SerializeField] private GameObject skillBar;
        [SerializeField] private GameObject skillChangePanel;

        public void Init(CrewMember crewMember, CrewMemberEncounterResult crewMemberResult)
        {
            image.sprite = crewMember.GreenSprite;
            nameField.text = crewMember.Name;
            healthBar.fillAmount = crewMember.Health / 100.0f;
            healthLostBar.fillAmount = (crewMember.Health + crewMemberResult.Damage) / 100.0f;

            if (crewMemberResult.SkillXp.role == null)
            {
                skillChangePanel.SetActive(false);
                return;
            }
            var (skillLevel, skillName) = crewMemberResult.SkillXp.role switch
            {
                EncounterRoles.Torpedo => (crewMember.Skills.Weapons,CrewMember.SkillData.WEAPONS_SKILL_NAME),
                EncounterRoles.Repairs => (crewMember.Skills.Engineer,CrewMember.SkillData.ENGINEER_SKILL_NAME),
                EncounterRoles.Navigator => (crewMember.Skills.Navigation,CrewMember.SkillData.NAVIGATION_SKILL_NAME),
                EncounterRoles.Medics => (crewMember.Skills.Medic,CrewMember.SkillData.MEDIC_SKILL_NAME),
                _ => throw new ArgumentOutOfRangeException()
            };
            FillBar(skillLevel, skillBar);
            skillNameField.text = skillName;
            skillChangedAmountField.text = $"+{crewMemberResult.SkillXp.xp:0.0} SKILL POINTS";
        }

        private static void FillBar(float skill, GameObject bar)
        {
            for (var i = 1; i <= Mathf.CeilToInt(skill); i++)
            {
                var child = bar.transform.GetChild(i);
                child.gameObject.SetActive(true);
                child.GetComponent<Image>().fillAmount = skill - i + 1;
                if (i == Mathf.CeilToInt(skill)) child.GetComponent<Image>().color = new Color(169, 254, 193, 1);
            }
        }
    }
}
