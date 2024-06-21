using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Submarine.ProgressBar
{
    public class EngineIntegrityBar : MonoBehaviour
    {
        [SerializeField] private Image fillImage;
        [SerializeField] private Image warningIcon;

        [SerializeField] private Color standardFillBarColor;
        [SerializeField] private Color lowValueFillBarColor;
        
        [SerializeField] public float engineIntegrityLowValueThreshold = 25f;
        
        [SerializeField] private TextMeshProUGUI engineIntegrityValueText;

        private const string _ENGINE_INTEGRITY_TEXT_FORMAT = "{0}%";
        
        public void UpdateVisual(float engineIntegrity)
        {
            fillImage.fillAmount = engineIntegrity / Model.Ship.RESOURCE_MAX_VALUE;
            engineIntegrityValueText.text = string.Format(_ENGINE_INTEGRITY_TEXT_FORMAT, Mathf.RoundToInt(engineIntegrity));
            
            fillImage.color = engineIntegrity > engineIntegrityLowValueThreshold ? standardFillBarColor : lowValueFillBarColor;
        }
        
        public void ShowWarningIcon(bool state)
        {
            warningIcon.gameObject.SetActive(state);
        }
        
    }
}