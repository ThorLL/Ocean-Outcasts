using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.DragAndDrop
{
    public class DragAndDropItem : MonoBehaviour
    {
        [SerializeField] private Image image;

        private void Update()
        {
            transform.position = Input.mousePosition;
        }

        public void SetActive(bool isActive, Sprite sprite)
        {
            image.sprite = sprite;
            Update();
            SetActive(isActive);
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}