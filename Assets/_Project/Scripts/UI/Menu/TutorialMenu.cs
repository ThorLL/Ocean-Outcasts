using System;
using _Project.Scripts.Manager;
using UnityEngine;

namespace _Project.Scripts.UI.Menu
{
    public class TutorialMenu : MonoBehaviour
    {
        public static event EventHandler OnGameStarted;

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
            
            gameObject.SetActive(false);
            GameSceneManager.Resume(TimeScaleRequester.TutorialMenu);
            OnGameStarted?.Invoke(this, EventArgs.Empty);
        }

        private void OnDestroy()
        {
            OnGameStarted = null;
        }
    }
}