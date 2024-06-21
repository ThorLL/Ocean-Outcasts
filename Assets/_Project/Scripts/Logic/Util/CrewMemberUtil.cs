using System;
using System.Collections.Generic;
using _Project.Scripts.Model;
using UnityEngine;

namespace _Project.Scripts.Logic.Util
{
    public static class CrewMemberUtil
    {
        private static readonly Dictionary<ShipRoomName, Func<CrewMember, float>> RoomsToSkill = new()
        {
            { ShipRoomName.Engine, crewMember => crewMember.Skills.Engineer },
            { ShipRoomName.Navigation, crewMember => crewMember.Skills.Navigation },
            { ShipRoomName.Oxygen, crewMember => crewMember.Skills.Medic },
            { ShipRoomName.Weapons, crewMember => crewMember.Skills.Weapons }
        };

        public static CrewMember GetCrewMemberById(GameData gameData, string id)
        {
            return gameData.CrewMembers.Find(crewMember => crewMember.Id == id);
        }

        public static float GetCrewMemberStatForRoom(CrewMember crewMember, ShipRoomName roomName)
        {
            return RoomsToSkill[roomName].Invoke(crewMember);
        }

        public static float InvLerpCrewMemberSkill(float skillValue)
        {
            return Mathf.InverseLerp(CrewMember.SkillData.MIN_VALUE, CrewMember.SkillData.MAX_VALUE, skillValue);
        }
    }
}