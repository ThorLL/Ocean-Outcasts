using System.Collections.Generic;
using System.Linq;
using _Project.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Configuration
{
    public class GameParameters: MonoBehaviour
    {
        [SerializeField] public float engineDecayRate = 0.25f;
        [SerializeField] public float navigationDecayRate = 0.25f;
        [SerializeField] public float oxygenDecayRate = 0.25f;
        
        [SerializeField] public float genericWorkSpeed = 1.05f;
        
        [SerializeField] public float engineSkillMultiplier = 1.0f;
        [SerializeField] public float navigationSkillMultiplier = 1.0f;
        [SerializeField] public float oxygenSkillMultiplier = 1.0f;
        [SerializeField] public float weaponsSkillMultiplier = .1f;
        
        [SerializeField] public float oxygenPenaltyStart = 50f;
        [SerializeField] public float oxygenCrewMemberHealthPenaltyRate = 1f;
        
        [SerializeField] [Tooltip("Meters of depth between crest and trough")] public float encounterDepthFrequency = 1000;
        [SerializeField] [Tooltip("Max time between encounters")] public float maxEncounterTime = 20;
        [SerializeField] [Tooltip("Min time between encounters")] public float minEncounterTime = 5;
        [SerializeField] public float encounterThreatAlert = 5f;
        
        [SerializeField] public float textEventVarianceSeconds = 15f;
        [SerializeField] public float textEventBaseIntervalSeconds = 90f;
        [SerializeField] public float lowNavigationTextEventBrakeSeconds = 90f;
        [SerializeField] public float lowNavigationThreshold = 50f;
        
        [SerializeField] public float minDescendSpeed = 1f;
        [SerializeField] public float maxDescendSpeed = 25f;
        
        [SerializeField] [Range(0, 1)] public float baseHitChanceLowerBound;
        [SerializeField] [Range(0, 1)] public float baseHitChanceUpperBound;
        [SerializeField] [Range(0, 1)] public float baseRepairStatLowerBound;
        [SerializeField] [Range(0, 1)] public float baseRepairStatUpperBound;
        [SerializeField] [Range(0, 1)] public float baseDodgeChangeLowerBound;
        [SerializeField] [Range(0, 1)] public float baseDodgeChangeUpperBound;
        [SerializeField] [Range(0, 1)] public float baseSupportStatLowerBound;
        [SerializeField] [Range(0, 1)] public float baseSupportStatUpperBound;
        
        [SerializeField] public float startingOxygen = 100f;
        [SerializeField] public float startingEngine = 100f;
        [SerializeField] public float startingNavigation = 100f;
        [SerializeField] public float startingAltimeter;
        [SerializeField] public float startingAmmo = 8f;
        
        [SerializeField] public List<DescendZone> descendZones;
        
        public static GameParameters Instance { get; private set; }
        
        private void Awake()
        {
            descendZones = descendZones.OrderBy(e => e.threshold).ToList();
            descendZones.Reverse();
            
            if (Instance != null)
            {
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}