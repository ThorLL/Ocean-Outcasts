using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.ScriptableObjects
{

    public enum TextEventChoiceType
    {
        NoChoice,
        BinaryChoice,
    }

    public enum TextEventInfluenceType
    {
        Positive,
        Negative,
    }
    
    public enum AffectedResource
    {
        Oxygen,
        Engine,
        Weapons,
        Health
    }
    
    public enum InitialSetup
    {
        None,
        RandomCrewMember,
    }
    
    [Serializable]
    public class TextEventEffect
    {
        [TextArea] public string text;
        
        public AffectedResource affectedResource;
        
        public float minValue;
        public float maxValue;
    }
    
    [Serializable]
    public class TextEventChoice
    {
        [TextArea] public string text;
        public TextEvent textEventTriggered;
    }
    
    [CreateAssetMenu]
    public class TextEvent : ScriptableObject
    {
        public string title;
        [TextArea] public string description;

        public InitialSetup initialSetup;
        
        public TextEventChoiceType textEventChoiceType;
        public TextEventInfluenceType textEventInfluenceType;

        public List<TextEventEffect> effects;
        public List<TextEventChoice> choices;
    }
    
}