using System.Collections;
using _Project.Scripts.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Menu
{
    public class PauseMenuVisual: MonoBehaviour
    {
        [SerializeField] private GameObject visual;
    
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button optionsButton;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button exitButton;

        [SerializeField] private OptionsMenu optionsScreen;

        private bool _isGameStarted;
        
        private void Awake()
        {
            mainMenuButton.onClick.AddListener(MainMenu);
            exitButton.onClick.AddListener(Exit);
            resumeButton.onClick.AddListener(Resume);
            optionsButton.onClick.AddListener(Options);

            TutorialMenu.OnGameStarted += (_, _) => StartCoroutine(OnGameStarted());
            
            _isGameStarted = false;
        }
        
        private IEnumerator OnGameStarted()
        {
            yield return new WaitForEndOfFrame();
            _isGameStarted = true;
        }

        private void Update()
        {
            if (!_isGameStarted || !Input.GetKeyDown(KeyCode.Escape)) return;
            
            visual.gameObject.SetActive(!visual.gameObject.activeSelf);
            if (visual.gameObject.activeSelf)
            {
                GameSceneManager.Pause(TimeScaleRequester.PauseMenu);
            }
            else GameSceneManager.Resume(TimeScaleRequester.PauseMenu);
        }
        
        private void Options()
        {
            optionsScreen.OnBack = () => optionsScreen.gameObject.SetActive(false);
            optionsScreen.gameObject.SetActive(true);
        }
        
        private void Resume()
        {
            visual.gameObject.SetActive(false);
            GameSceneManager.Resume(TimeScaleRequester.PauseMenu);
        }
        
        private static void Exit()
        {
            GameSceneManager.Exit();
        }
        
        private static void MainMenu()
        {
            GameSceneManager.Resume(TimeScaleRequester.PauseMenu);
            GameSceneManager.LoadScene(GameSceneName.MainMenu);
        }
    }
}