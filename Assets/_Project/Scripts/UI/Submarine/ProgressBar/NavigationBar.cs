using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Submarine.ProgressBar
{
    public class NavigationBar: MonoBehaviour
    {
        [SerializeField] private Image fillImage;
        [SerializeField] private TextMeshProUGUI speedText;

        [SerializeField] private float maxFillValue = 0.75f;
        
        private const string _NAVIGATION_SPEED_FORMAT = "{0} <size=35%>m/s";
        
        public void UpdateVisual(float navigationValue, float speed)
        {
            var fillValue = navigationValue / Model.Ship.RESOURCE_MAX_VALUE;
            fillImage.fillAmount = fillValue * maxFillValue;
            speedText.text = string.Format(_NAVIGATION_SPEED_FORMAT, Mathf.RoundToInt(speed));
        }
    }
}