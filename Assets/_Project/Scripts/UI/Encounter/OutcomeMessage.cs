using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.Encounter
{
    public class OutcomeMessage : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI message;
        public void SetText(string text) => message.text = text;
    }
}
