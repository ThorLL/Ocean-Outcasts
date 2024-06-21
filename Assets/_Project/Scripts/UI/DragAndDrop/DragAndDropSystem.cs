using _Project.Scripts.UI.Tooltip;
using JetBrains.Annotations;
using UnityEngine;

namespace _Project.Scripts.UI.DragAndDrop
{
    public abstract class DragAndDropSystem<T> : MonoBehaviour
    {
        private static DragAndDropSystem<T> _current;
        [SerializeField] private DragAndDropItem dragAndDropItem;
        [CanBeNull] private T _item;

        private void Awake()
        {
            _current = this;
        }

        public static T GetItem()
        {
            var item = _current._item;
            _current._item = default;
            return item;
        }

        public static void SetItem(T item)
        {
            _current._item = item;
        }

        public static void SetActive(bool isActive, Sprite sprite)
        {
            TooltipSystem.Disable(isActive);
            if (_current._item != null)
                _current.dragAndDropItem.SetActive(isActive, sprite);
            else
                _current.dragAndDropItem.SetActive(isActive);
        }

        public static void SetActive(bool isActive)
        {
            TooltipSystem.Disable(isActive);
            _current.dragAndDropItem.SetActive(isActive);
        }
    }
}