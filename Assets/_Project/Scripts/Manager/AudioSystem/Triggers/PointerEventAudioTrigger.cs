using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Scripts.Manager.AudioSystem.Triggers
{
    public class PointerEventAudioTrigger : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
    {
        [SerializeField] private string onHover = "OnHoverButton";
        [SerializeField] private string onClick = "OnClickButton";
        public void OnPointerEnter(PointerEventData eventData) => AudioManager.Play(onHover);
        public void OnPointerDown(PointerEventData eventData) => AudioManager.Play(onClick);
    }
}
