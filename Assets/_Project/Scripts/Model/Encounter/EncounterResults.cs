using System.Collections.Generic;
using _Project.ScriptableObjects;

namespace _Project.Scripts.Model.Encounter
{
    public class EncounterResults
    {
        public readonly bool DodgedAttack;
        public readonly float HullDamage;
        public readonly float TorpedoesHit;

        public readonly Dictionary<CrewMember, CrewMemberEncounterResult> CrewMemberResults;

        public EncounterResults
        (
            bool dodgedAttack,
            int torpedoesHit,
            float hullDamage,
            Dictionary<CrewMember, CrewMemberEncounterResult> crewMemberResults)
        {
            TorpedoesHit = torpedoesHit;
            DodgedAttack = dodgedAttack;
            HullDamage = hullDamage;
            CrewMemberResults = crewMemberResults;
        }
    }

    public class CrewMemberEncounterResult
    {
        public float Damage = 0;
        public (float xp, EncounterRoles? role) SkillXp = (0f, null);
    }
}