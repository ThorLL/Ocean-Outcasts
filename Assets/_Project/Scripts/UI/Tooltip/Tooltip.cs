using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Tooltip
{
    public class Tooltip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI contentField;
        [SerializeField] private LayoutElement layoutElement;
        [SerializeField] private int characterWarpLimit;
        private TooltipPositions _tooltipPositions;
        private RectTransform _parentTransform;
        public float scale;

        public void SetText(string content)
        {
            contentField.text = content;
            layoutElement.enabled = contentField.text.Length > characterWarpLimit;
        }
        private void Update()
        {
            var (parentWidth, parentHeight) = GetSize(_parentTransform);
            var (width, height) = GetSize((RectTransform)transform);
            var parentPosition = _parentTransform.position;
            transform.position = _tooltipPositions switch
            {
                TooltipPositions.Top => parentPosition + new Vector3(scale*(parentWidth + width)/2, 0, 0),
                TooltipPositions.Bottom => parentPosition + new Vector3(scale*(parentWidth + width)/2, -(parentHeight + height)*scale, 0),
                TooltipPositions.Left => parentPosition + new Vector3(0, -(parentHeight + height)/2*scale, 0),
                TooltipPositions.Right => parentPosition + new Vector3( scale*(parentWidth + width), -(parentHeight + height)/2*scale, 0),
                TooltipPositions.Cursor => Input.mousePosition,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void SetPosition(TooltipPositions tooltipPosition, RectTransform parentTransform)
        {
            _tooltipPositions = tooltipPosition;
            _parentTransform = parentTransform;
            Update();
        }

        private static (float width, float height) GetSize(RectTransform rt)
        {
            var rect = rt.rect;
            return (rect.width, rect.height);
        }
    }
}
