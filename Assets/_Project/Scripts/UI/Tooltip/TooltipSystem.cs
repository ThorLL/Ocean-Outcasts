using UnityEngine;

namespace _Project.Scripts.UI.Tooltip
{
    public class TooltipSystem : MonoBehaviour
    {
        private static TooltipSystem _current;
        [SerializeField] private Tooltip tooltip;
        private bool _disabled;
        private void Awake()
        {
            _current = this;
        }

        public static void Disable(bool isDisabled)
        {
            Hide();
            _current._disabled = isDisabled;
        }
        
        public static void Show(string content, TooltipPositions tooltipPosition, RectTransform transform)
        {
            if(_current._disabled) return;
            _current.tooltip.SetPosition(tooltipPosition, transform);
            _current.tooltip.SetText(content);
            _current.tooltip.scale = _current.GetComponent<Canvas>().scaleFactor;
            _current.tooltip.gameObject.SetActive(true);
        }
        
        public static void Hide()
        {
            if(_current._disabled) return;
            _current.tooltip.gameObject.SetActive(false);
        }
    }
}
