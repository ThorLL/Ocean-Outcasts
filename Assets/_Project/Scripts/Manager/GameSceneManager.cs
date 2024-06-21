using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Manager.AudioSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Manager
{
    public enum GameSceneName
    {   
        MainMenu = 0,
        SelectCrew = 1,
        Descend = 2,
        Loading = 3
    }
    
    public enum TimeScaleRequester
    {   
        CrewMemberDrag,
        Encounter,
        PauseMenu,
        TutorialMenu,
        GameOver,
        TextEvent
    }
    
    public abstract class GameSceneManager
    {
        private static readonly List<(TimeScaleRequester requester, float factor)> CurrentTimeScaleRequesterStack = new();
        
        public static void LoadScene(GameSceneName name)
        {
            if (name == GameSceneName.MainMenu)
            {
                AudioManager.StopAll();
            }
            SceneManager.LoadScene((int) name);
        }
        
        public static void Pause(TimeScaleRequester requester)
        {
            CurrentTimeScaleRequesterStack.Add((requester, 0.0f));
            Time.timeScale = 0.0f;
        }
        
        public static void Slow(TimeScaleRequester requester, float factor)
        {
            CurrentTimeScaleRequesterStack.Add((requester, Time.timeScale * factor));
            Time.timeScale *= factor;
        }
        
        public static void Resume(TimeScaleRequester requester)
        {
            var index = CurrentTimeScaleRequesterStack.FindIndex(e => e.requester == requester);
            if (index == -1) return;
            CurrentTimeScaleRequesterStack.RemoveAt(index);
            
            if (CurrentTimeScaleRequesterStack.Count == 0)
            {
                Time.timeScale = 1.0f;
                return;
            }
            
            Time.timeScale = CurrentTimeScaleRequesterStack.First().factor;
        }

        public static void Exit()
        {
            #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
            #else
                    Application.Quit();
            #endif
        }
    }
}