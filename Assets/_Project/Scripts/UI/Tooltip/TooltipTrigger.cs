using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Scripts.UI.Tooltip
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TooltipPositions tooltipPosition;
        [SerializeField] private string contentText;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            TooltipSystem.Show(contentText, tooltipPosition, (RectTransform)transform);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipSystem.Hide();
        }
    }
}
