using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Menu
{
    public class MainMenuLore: MonoBehaviour
    {
        [SerializeField] private Button button;

        public Action OnNext { get; set; }
        
        private void Awake()
        {
            button.onClick.AddListener(Next);
        }

        private void Next()
        {
            OnNext?.Invoke();
        }
    }
}