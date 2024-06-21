using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Submarine.ProgressBar
{
    public class OxygenBar: MonoBehaviour
    {
        [SerializeField] private Image fillImage;
        [SerializeField] private Image warningIcon;
        
        [SerializeField] private TextMeshProUGUI oxygenValueText;
        [SerializeField] private float orientation = 180f;
        
        private const string _OXYGEN_TEXT_FORMAT = "{0}<size=35%>%";
        
        public void UpdateVisual(float oxygen)
        {
            fillImage.fillAmount = oxygen / Model.Ship.RESOURCE_MAX_VALUE;
            fillImage.transform.rotation = Quaternion.Euler(0, 0, orientation * fillImage.fillAmount); 
                
            oxygenValueText.text = string.Format(_OXYGEN_TEXT_FORMAT, Mathf.RoundToInt(oxygen));

            warningIcon.gameObject.SetActive(oxygen < 50);
        }
        
        public void ShowWarningIcon(bool state)
        {
            warningIcon.gameObject.SetActive(state);
        }
        
    }
}