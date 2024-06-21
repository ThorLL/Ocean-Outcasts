using UnityEngine;

namespace _Project.Scripts.UI.DragAndDrop
{
    public interface IAssignable<T>
    {
        public bool CanDragFrom { get; }
        public bool Assign(T t);
        public (T, Sprite) Unassign();
    }
}