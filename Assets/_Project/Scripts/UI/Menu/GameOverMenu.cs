using _Project.Scripts.Logic.Submarine;
using _Project.Scripts.Manager;
using _Project.Scripts.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Menu
{
    public class GameOverMenu: MonoBehaviour
    {
        [SerializeField] private GameObject visual;

        [SerializeField] private Button retryButton;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button exitButton;
        
        [SerializeField] private TextMeshProUGUI depthText;
        [SerializeField] private TextMeshProUGUI monsterCountText;
        

        private const string _DEPT_FORMAT_TEXT = "You reached a depth of {0}m";
        private const string _MONSTER_COUNT_FORMAT_TEXT = "You killed {0} monsters";
        
        private void Awake()
        {
            ShipBehaviour.OnGameOver += OnGameOver;
            
            retryButton.onClick.AddListener(Retry);
            mainMenuButton.onClick.AddListener(MainMenu);
            exitButton.onClick.AddListener(Exit);
            
            visual.SetActive(false);
        }

        private void OnGameOver(object sender, GameData e)
        {
            visual.SetActive(true);
            
            depthText.text = string.Format(_DEPT_FORMAT_TEXT, Mathf.RoundToInt(e.Ship.Altimeter));
            monsterCountText.text = string.Format(_MONSTER_COUNT_FORMAT_TEXT, Mathf.RoundToInt(e.KillCount));
            
            GameSceneManager.Pause(TimeScaleRequester.GameOver);
        }
        
        private static void Exit()
        {
            GameSceneManager.Exit();
        }
        
        private static void Retry()
        {
            GameSceneManager.Resume(TimeScaleRequester.GameOver);
            GameSceneManager.LoadScene(GameSceneName.SelectCrew);
        }
        
        private static void MainMenu()
        {
            GameSceneManager.Resume(TimeScaleRequester.GameOver);
            GameSceneManager.LoadScene(GameSceneName.MainMenu);
        }
        
    }
}