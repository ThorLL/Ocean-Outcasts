using System;
using Newtonsoft.Json;
using UnityEngine;

namespace _Project.Scripts.Model
{
    public class CrewMember
    {
        public const float MIN_HEALTH = 0f;
        public const float MAX_HEALTH = 100f;

        [JsonIgnore] public string Id { get; } = Guid.NewGuid().ToString();

        private float _health = MAX_HEALTH;
        [JsonIgnore] public float Health
        {
            get => _health;
            set => _health = Mathf.Clamp(value, MIN_HEALTH, MAX_HEALTH);
        }
        [JsonIgnore] public Sprite GreenSprite { get; set; }
        [JsonIgnore] public Sprite NeutralSprite { get; set; }

        [JsonIgnore] public string Name { get; set; }
        [JsonProperty("Bio")] public string Bio { get; set; }
        
        [JsonProperty("Skills")] public SkillData Skills { get; set; }
        [JsonIgnore] public SkillData InitialSkills { get; set; }
        
        public class SkillData
        {
            public const float MIN_VALUE = 1;
            public const float MAX_VALUE = 10;

            public const string ENGINEER_SKILL_NAME = "Engineer";
            public const string MEDIC_SKILL_NAME = "Medics";
            public const string NAVIGATION_SKILL_NAME = "Navigation";
            public const string WEAPONS_SKILL_NAME = "Weapons";
            
            private float _engineer;
            private float _medic;
            private float _navigation;
            private float _weapons;

            [JsonProperty("Engineer")]
            public float Engineer
            {
                get => _engineer;
                set => _engineer = Mathf.Clamp(value, MIN_VALUE, MAX_VALUE);
            }

            [JsonProperty("Medic")]
            public float Medic
            {
                get => _medic;
                set => _medic = Mathf.Clamp(value, MIN_VALUE, MAX_VALUE);
            }

            [JsonProperty("Navigation")]
            public float Navigation
            {
                get => _navigation;
                set => _navigation = Mathf.Clamp(value, MIN_VALUE, MAX_VALUE);
            }

            [JsonProperty("Weapons")]
            public float Weapons
            {
                get => _weapons;
                set => _weapons = Mathf.Clamp(value, MIN_VALUE, MAX_VALUE);
            }
        }
    }
}