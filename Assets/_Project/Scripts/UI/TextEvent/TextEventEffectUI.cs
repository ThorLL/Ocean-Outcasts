using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.TextEvent
{
    public class TextEventEffectUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI effectText;
        
        private const string _TEXT_FORMAT = "> {0}"; 
        
        public void SetText(string text)
        {
            effectText.text = string.Format(_TEXT_FORMAT, text);
        }
        
    }
}