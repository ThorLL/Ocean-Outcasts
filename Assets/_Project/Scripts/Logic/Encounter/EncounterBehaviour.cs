using System;
using System.Collections.Generic;
using _Project.ScriptableObjects;
using _Project.Scripts.Configuration;
using _Project.Scripts.Logic.Util;
using _Project.Scripts.Model;
using _Project.Scripts.Model.Encounter;
using _Project.Scripts.UI.Encounter;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Logic.Encounter
{
    public class EncounterBehaviour
    {
        private readonly Func<float, float> _crewMemberXp =
            Easing.EasingLerp(1, 0, CrewMemberUtil.InvLerpCrewMemberSkill, Easing.EaseOutQuart);

        private readonly Func<float, float> _dodgeChance =
            Easing.EasingLerp(0, 0.8f, CrewMemberUtil.InvLerpCrewMemberSkill, Easing.EaseOutQuad);

        private readonly GameParameters _gameParameters = GameParameters.Instance;

        private readonly Func<float, float> _hitChanceFunc =
            Easing.EasingLerp(0, 1, CrewMemberUtil.InvLerpCrewMemberSkill, Easing.EaseOutQuad);

        private readonly Func<float, float> _repairStateFunc =
            Easing.EasingLerp(0, 0.5f, CrewMemberUtil.InvLerpCrewMemberSkill, Easing.EaseOutQuad);

        private readonly Func<float, float> _supportStat =
            Easing.EasingLerp(0, 0.5f, CrewMemberUtil.InvLerpCrewMemberSkill, Easing.EaseOutQuad);

        public EncounterResults Fight(GameData gameData, MonsterEncounter encounter, int torpedoesFired)
        {
            gameData.Ship.Ammunition -= torpedoesFired;

            var hitChance = Random.Range(_gameParameters.baseHitChanceLowerBound,
                _gameParameters.baseHitChanceUpperBound);
            var repairStat = Random.Range(_gameParameters.baseRepairStatLowerBound,
                _gameParameters.baseRepairStatUpperBound);
            var dodgeChange = Random.Range(_gameParameters.baseDodgeChangeLowerBound,
                _gameParameters.baseDodgeChangeUpperBound);
            var supportStat = Random.Range(_gameParameters.baseSupportStatLowerBound,
                _gameParameters.baseSupportStatUpperBound);

            var crewMemberResults = new Dictionary<CrewMember, CrewMemberEncounterResult>();
            gameData.CrewMembers.ForEach(member => crewMemberResults[member] = new CrewMemberEncounterResult());

            foreach (var tile in EncounterUI.GetRoleTiles())
            {
                var role = tile.role.role;
                var (crewMember, _) = tile.Unassign();
                if (crewMember == null) continue;
                float xp;
                switch (role)
                {
                    case EncounterRoles.Torpedo:
                        if (torpedoesFired == 0) continue;
                        hitChance += _hitChanceFunc(crewMember.Skills.Weapons);
                        xp = _crewMemberXp(crewMember.Skills.Weapons);
                        crewMember.Skills.Weapons += xp;
                        break;
                    case EncounterRoles.Repairs:
                        repairStat += _repairStateFunc(crewMember.Skills.Engineer);
                        xp = _crewMemberXp(crewMember.Skills.Engineer);
                        crewMember.Skills.Engineer += xp;
                        break;
                    case EncounterRoles.Navigator:
                        dodgeChange += _dodgeChance(crewMember.Skills.Navigation);
                        xp = _crewMemberXp(crewMember.Skills.Navigation);
                        crewMember.Skills.Navigation += xp;
                        break;
                    case EncounterRoles.Medics:
                        supportStat += _supportStat(crewMember.Skills.Medic);
                        xp = _crewMemberXp(crewMember.Skills.Medic);
                        crewMember.Skills.Medic += xp;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                crewMemberResults[crewMember].SkillXp = (xp, role);
            }

            var dodgesAttack = Random.Range(0f, 1f) < dodgeChange;
            var torpedoesHit = Mathf.Clamp(Mathf.FloorToInt(hitChance * torpedoesFired), 0, torpedoesFired);
            var damageToHull = 0f;

            if (!dodgesAttack)
            {
                damageToHull = (1 - repairStat) * encounter.monsterDamage;
                gameData.Ship.EngineIntegrity -= damageToHull;
                var damageToCrew = (1 - supportStat) * encounter.monsterDamage;
                gameData.CrewMembers.ForEach(member =>
                {
                    var dmg = RandomizeDamageToCrewMember(damageToCrew, gameData.CrewMembers.Count);
                    crewMemberResults[member].Damage = dmg;
                    member.Health -= dmg;
                });
            }

            // give reward for killing monster
            if (encounter.health <= torpedoesHit)
                gameData.KillCount += 1;

            return new EncounterResults(
                dodgesAttack,
                torpedoesHit,
                damageToHull,
                crewMemberResults
            );
        }

        private static float RandomizeDamageToCrewMember(float totalDamage, float nCrewMembers,
            float percentRange = 0.3f)
        {
            var splitDamage = totalDamage / nCrewMembers;
            var lower = splitDamage - totalDamage * percentRange;
            var upper = splitDamage + totalDamage * percentRange;
            return Random.Range(lower, upper);
        }
    }
}