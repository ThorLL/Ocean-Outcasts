using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Menu
{
    public class OptionsMenu : MonoBehaviour
    {
        [SerializeField] private Button backButton;
        
        public Action OnBack { get; set; }
        
        private void Start()
        {
            backButton.onClick.AddListener(() => OnBack.Invoke());
        }
        
        
    }
}