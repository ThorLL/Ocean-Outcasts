using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Scripts.UI.DragAndDrop
{
    public abstract class DragAndDropper<T> : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        
        private IAssignable<T> _parent;

        private void Awake()
        {
            _parent = GetComponentInParent<IAssignable<T>>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(!_parent.CanDragFrom) return;
            var (item, sprite) = _parent.Unassign();
            if (item == null) return;
            DragAndDropSystem<T>.SetItem(item);
            DragAndDropSystem<T>.SetActive(true, sprite);
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnDrop(PointerEventData eventData)
        {
            var item = DragAndDropSystem<T>.GetItem();
            if (item == null) return;
            var oldOwner = eventData.pointerDrag.GetComponent<IAssignable<T>>();

            var acceptedAssignment = _parent.Assign(item);
            if (!acceptedAssignment) oldOwner.Assign(item);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            DragAndDropSystem<T>.SetActive(false);

            var item = DragAndDropSystem<T>.GetItem();
            if (item == null) return;
            _parent.Assign(item);
        }
    }
}