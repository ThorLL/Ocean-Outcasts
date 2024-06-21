using System;
using System.Collections.Generic;
using System.Linq;
using _Project.ScriptableObjects;
using _Project.Scripts.Logic.Util;
using _Project.Scripts.Model;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Logic.TextEvent
{

    public class TextEventEffectApplier
    {
        private List<float> _amounts;
        private CrewMember _crewMember;

        public GameData GameData { get; set; }
        public ScriptableObjects.TextEvent TextEvent { get; set; }
        
        public void Init()
        {
            _amounts = new List<float>();
            _crewMember = null;
            
            // Get the random amounts         
            foreach (var effect in TextEvent.effects)
            {
                _amounts.Add(GetRandomAmount(effect));
            }
            
            // Get the initial setup data
            switch (TextEvent.initialSetup)
            {
                case InitialSetup.None:
                {
                    
                } break;
                case InitialSetup.RandomCrewMember:
                {
                    _crewMember = RandomCollectionUtil.GetRandomElementsFromCollection(GameData.CrewMembers, 1).First();
                } break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }
        
        public void ApplyEffects()
        {
            var adding = TextEvent.textEventInfluenceType == TextEventInfluenceType.Positive ? 1 : -1;
            foreach (var i in Enumerable.Range(0, TextEvent.effects.Count))
            {
                switch (TextEvent.effects[i].affectedResource)
                {
                    case AffectedResource.Oxygen:
                    {
                        GameData.Ship.Oxygen += adding * _amounts[i];
                        GameData.Ship.Oxygen = Mathf.Clamp(GameData.Ship.Oxygen, Ship.RESOURCE_MIN_VALUE, Ship.RESOURCE_MAX_VALUE);
                    } break;
                    case AffectedResource.Engine:
                    {
                        GameData.Ship.EngineIntegrity += adding * _amounts[i];
                        GameData.Ship.EngineIntegrity = Mathf.Clamp(GameData.Ship.EngineIntegrity, Ship.RESOURCE_MIN_VALUE, Ship.RESOURCE_MAX_VALUE);
                    } break;
                    case AffectedResource.Weapons:
                    {
                        GameData.Ship.Ammunition += adding * _amounts[i];
                        GameData.Ship.Ammunition = Mathf.Max(GameData.Ship.Ammunition, 0);
                    } break;
                    case AffectedResource.Health:
                    {
                        _crewMember.Health += adding * _amounts[i];
                        _crewMember.Health = Mathf.Clamp(_crewMember.Health, CrewMember.MIN_HEALTH, CrewMember.MAX_HEALTH);
                    } break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public string FormatDescription(string text)
        {
            return _crewMember != null ? text.Replace("{name}", _crewMember.Name) : text;
        }
        
        public string FormatEffect(string text, int i)
        {
            var amount = Mathf.RoundToInt(_amounts[i]).ToString();
            if (_crewMember != null)
            {
                return text
                    .Replace("{name}", _crewMember.Name)
                    .Replace("{amount}", amount);
            } 
            return text.Replace("{amount}", amount);
        }
        
        private static float GetRandomAmount(TextEventEffect effect)
        {
            var random = Random.Range(0f, 1.0f);
            return Mathf.Lerp(effect.minValue, effect.maxValue, random);
        }
        
    }
}