using _Project.Scripts.Model;
using UnityEngine;

namespace _Project.Scripts.Manager
{
    public class GameDataManager : MonoBehaviour
    {
        public static GameDataManager Instance { get; private set; }

        public GameData GameData { get; private set; } = new();
    
        private void Awake()
        {
            if (Instance != null)
            {
                Instance.GameData = new GameData();
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
