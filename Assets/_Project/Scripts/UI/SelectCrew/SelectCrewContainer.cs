using System.Collections.Generic;
using _Project.Scripts.Configuration;
using _Project.Scripts.Logic.Util;
using _Project.Scripts.Manager;
using _Project.Scripts.Model;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.SelectCrew
{
    public class SelectCrewContainer : MonoBehaviour
    {
        public TextAsset jsonFile;
        
        [SerializeField] private SelectCrewMemberDescription descriptionPrefab;
        [SerializeField] private TextMeshProUGUI selectCounter;
        [SerializeField] private GameObject continueButton;
        
        private List<SelectCrewMemberDescription> ChosenCrew { get; } = new();
        
        private const int _MAX_DISPLAYED = 6;
        private const int _MAX_SELECT = 3;

        // TODO Should be moved to logic for MVC pattern
        private void Awake()
        {
            var selectableCrewMembersPool = JsonConvert.DeserializeObject<CrewMembersPool>(jsonFile.text);
            var selectableCrewMembers = RandomCollectionUtil.GetRandomElementsFromCollection(selectableCrewMembersPool.SkillsWithBio, _MAX_DISPLAYED);
            var selectableCrewNames = RandomCollectionUtil.GetRandomElementsFromCollection(selectableCrewMembersPool.Names, _MAX_DISPLAYED);
            var selectableCrewSprites = RandomCollectionUtil.GetRandomElementsFromCollection(selectableCrewMembersPool.Sprites, _MAX_DISPLAYED);

            for (var i = 0; i < selectableCrewMembers.Count; i++)
            {
                var member = selectableCrewMembers[i];
                member.Name = selectableCrewNames[i];
                member.GreenSprite = ResourceLoaderUtil.GetAsset<Sprite>("textures/crewmember/green/" + selectableCrewSprites[i]);
                member.NeutralSprite = ResourceLoaderUtil.GetAsset<Sprite>("textures/crewmember/neutral/" + selectableCrewSprites[i]);
                member.InitialSkills = new CrewMember.SkillData
                {
                    Navigation = member.Skills.Navigation,
                    Engineer = member.Skills.Engineer,
                    Medic = member.Skills.Medic,
                    Weapons = member.Skills.Weapons
                };
                var instance = Instantiate(descriptionPrefab, gameObject.transform, false);
                instance.CrewMember = member;
                instance.OnPrisonerSelected = CrewMemberClicked;
            }
            
            /*
            selectableCrewMembers.ForEach(member =>
            {
                var instance = Instantiate(descriptionPrefab, gameObject.transform, false);
                member.Sprite = ResourceLoaderUtil.GetAsset<Sprite>(member.SpritePath);
                instance.CrewMember = member;
                instance.OnPrisonerSelected = CrewMemberClicked;
            });
            */
        }

        private void CrewMemberClicked(SelectCrewMemberDescription desc)
        {
            //Update select status
            desc.Selected = !desc.Selected;
            
            //Is it trying to be selected?
            if (desc.Selected)
            {
                //Is there room?
                if (ChosenCrew.Count < _MAX_SELECT)
                {
                    ChosenCrew.Add(desc);
                }
                else desc.Selected = false;
            }
            else ChosenCrew.Remove(desc);
            
            selectCounter.text = (_MAX_SELECT - ChosenCrew.Count).ToString();
            continueButton.SetActive(ChosenCrew.Count == _MAX_SELECT);
        }

        public void GoToNextScene()
        {
            if (ChosenCrew.Count != _MAX_SELECT) return;
            
            var gameData = GameDataManager.Instance.GameData;
            
            //Update GameData
            foreach (var crewMemberDescription in ChosenCrew)
            {
                gameData.CrewMembers.Add(crewMemberDescription.CrewMember);
            }
            
            var parameters = GameParameters.Instance;
            gameData.Ship = new Ship
            {
                Oxygen = parameters.startingOxygen,
                EngineIntegrity = parameters.startingEngine,
                Navigation = parameters.startingNavigation,
                Altimeter = parameters.startingAltimeter,
                Ammunition = parameters.startingAmmo
            };
            
            GameSceneManager.Pause(TimeScaleRequester.TutorialMenu);
            GameSceneManager.LoadScene(GameSceneName.Descend);
        }
    }
}
