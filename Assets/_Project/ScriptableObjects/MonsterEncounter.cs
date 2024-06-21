using UnityEngine;

namespace _Project.ScriptableObjects
{
    [CreateAssetMenu]
    public class MonsterEncounter : ScriptableObject
    {
        public string monsterName;
        public int health;
        public int monsterDamage;
        public Sprite sprite;
        public string desc;
    }
}