using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Submarine.ProgressBar
{
    public class AmmunitionCounter: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI ammunitionValueText;
        
        [SerializeField] private Image increasingAmmunitionIcon;

        private void Awake()
        {
            ShowIconIncreasingAmmunition(false);
        }

        public void UpdateVisual(float ammunition)
        {
            ammunitionValueText.text = Mathf.FloorToInt(ammunition).ToString();
        }
        
        public void ShowIconIncreasingAmmunition(bool state)
        {
            increasingAmmunitionIcon.gameObject.SetActive(state);
        }
    }
}