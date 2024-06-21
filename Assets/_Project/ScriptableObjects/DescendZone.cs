using UnityEngine;

namespace _Project.ScriptableObjects
{
    [CreateAssetMenu]
    public class DescendZone : ScriptableObject
    {
        public string zoneName;
        public int threshold;
        public Sprite sprite;
        public int easyEncounterChance;
        public int mediumEncounterChance;
        public int hardEncounterChance;
    }
}