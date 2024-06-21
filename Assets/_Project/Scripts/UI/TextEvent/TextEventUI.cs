using System;
using System.Collections.Generic;
using System.Linq;
using _Project.ScriptableObjects;
using _Project.Scripts.Logic.TextEvent;
using _Project.Scripts.Manager;
using _Project.Scripts.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.TextEvent
{
    public class TextEventUI : MonoBehaviour
    {
        
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionsText;

        [SerializeField] private VerticalLayoutGroup effectsGroup;
        [SerializeField] private List<TextEventEffectUI> effects;
        
        [SerializeField] private Button continueButton;
        
        [SerializeField] private Button choice1Button;
        [SerializeField] private TextMeshProUGUI choice1Text;
        [SerializeField] private Button choice2Button;
        [SerializeField] private TextMeshProUGUI choice2Text;

        private readonly TextEventEffectApplier _textEventEffectApplier = new();

        private void Awake()
        {
            continueButton.onClick.AddListener(OnContinue);
        }

        public void Init(GameData gameData, ScriptableObjects.TextEvent textEvent)
        {
            
            _textEventEffectApplier.GameData = gameData;
            _textEventEffectApplier.TextEvent = textEvent;
            
            _textEventEffectApplier.Init();
            
            titleText.text = textEvent.title;
            var formattedDescriptionText = _textEventEffectApplier.FormatDescription(textEvent.description);
            descriptionsText.text = formattedDescriptionText;
            
            if (textEvent.textEventChoiceType == TextEventChoiceType.NoChoice)
            {
                effectsGroup.gameObject.SetActive(true);
                foreach (var i in Enumerable.Range(0, 3))
                {
                    if (i < textEvent.effects.Count)
                    {
                        effects[i].gameObject.SetActive(true);
                        var formattedEffectText = _textEventEffectApplier.FormatEffect(textEvent.effects[i].text, i);
                        effects[i].SetText(formattedEffectText);
                    }
                    else effects[i].gameObject.SetActive(false);
                }
                
                choice1Button.gameObject.SetActive(false);
                choice2Button.gameObject.SetActive(false);
                
                continueButton.gameObject.SetActive(true);
            }
            else 
            {
                effectsGroup.gameObject.SetActive(false);
                
                continueButton.gameObject.SetActive(false);

                choice1Button.gameObject.SetActive(true);
                choice1Text.text = textEvent.choices[0].text;
                choice1Button.onClick.RemoveAllListeners();
                choice1Button.onClick.AddListener(() =>
                {
                    Init(gameData, textEvent.choices[0].textEventTriggered);
                });
                
                
                choice2Button.gameObject.SetActive(true);
                choice2Text.text = textEvent.choices[1].text;
                choice2Button.onClick.RemoveAllListeners();
                choice2Button.onClick.AddListener(() =>
                {
                    Init(gameData, textEvent.choices[1].textEventTriggered);
                });
            }
        }

        private void OnContinue()
        {
            _textEventEffectApplier.ApplyEffects();
            
            GameSceneManager.Resume(TimeScaleRequester.TextEvent);
            
            gameObject.SetActive(false);
        }
        
    }
}