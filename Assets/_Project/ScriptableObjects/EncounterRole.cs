using UnityEngine;

namespace _Project.ScriptableObjects
{
    public enum EncounterRoles
    {
        Torpedo,
        Repairs,
        Navigator,
        Medics
    }
    [CreateAssetMenu]
    public class EncounterRole : ScriptableObject
    {
        public EncounterRoles role;
        public string roleName;
        public Sprite icon;
    }
}