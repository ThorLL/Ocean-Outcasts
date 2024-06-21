using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Menu
{
    public class MainMenu : MonoBehaviour
    {
        
        [SerializeField] private Button startButton;
        [SerializeField] private Button creditsButton;
        [SerializeField] private Button optionsButton;
        [SerializeField] private Button exitButton;

        [SerializeField] private List<MainMenuLore> loreScreens;
        [SerializeField] private MainMenuLore creditScreen;
        [SerializeField] private OptionsMenu optionsScreen;
        
        private void Awake()
        {
            loreScreens.ForEach(x => x.gameObject.SetActive(false));
            startButton.onClick.AddListener(OnStartClicked);
            optionsButton.onClick.AddListener(OnOptionsClicked);
            creditsButton.onClick.AddListener(OnCreditsClicked);
            exitButton.onClick.AddListener(OnExitClicked);
        }
        
        private static void StartGame()
        {
            GameSceneManager.LoadScene(GameSceneName.SelectCrew);
        }
        
        private void OnStartClicked()
        {
            for (var i = 0; i < loreScreens.Count - 1; i++)
            {
                var nextLoreScreen = loreScreens[i + 1];
                loreScreens[i].OnNext = () => nextLoreScreen.gameObject.SetActive(true);
            }
            
            loreScreens.Last().OnNext = StartGame;
            loreScreens.First().gameObject.SetActive(true);
        }

        private void OnCreditsClicked()
        {
            creditScreen.OnNext = () => creditScreen.gameObject.SetActive(false);
            creditScreen.gameObject.SetActive(true);
        }
        
        private void OnOptionsClicked()
        {
            optionsScreen.OnBack = () => optionsScreen.gameObject.SetActive(false);
            optionsScreen.gameObject.SetActive(true);
        }
        
        private static void OnExitClicked()
        {
            GameSceneManager.Exit();
        }
        
    }
}
