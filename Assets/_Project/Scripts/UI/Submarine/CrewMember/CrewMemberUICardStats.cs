using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Submarine.CrewMember
{
    public class CrewMemberUICardStats : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI statValueText;
        [SerializeField] private List<Image> statValueBars;
        
        [SerializeField] private Color baseSkillColor;
        [SerializeField] private Color newSkillColor;

        public void SetSkill(float statValue, float initialStatValue)
        {
            var integerStatValue = Mathf.FloorToInt(statValue);
            statValueText.text = integerStatValue.ToString();
            for (var i = 0; i < statValueBars.Count; i++)
            {
                statValueBars[i].color = i < initialStatValue ? baseSkillColor : newSkillColor;
                statValueBars[i].fillAmount = i < statValue ? 1f : 0f;
            }
            
            var decimalStatValue = statValue - integerStatValue;
            if (decimalStatValue > 0.05f)
            {
                statValueBars[integerStatValue].fillAmount = decimalStatValue;
                statValueBars[integerStatValue].color = newSkillColor;
            }
        }
        
    }
}
