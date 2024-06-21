using System.Collections.Generic;

namespace _Project.Scripts.Model
{
    public class GameData
    {
        public List<CrewMember> CrewMembers { set; get; } = new();
        public Ship Ship { set; get; }
        public int KillCount { set; get; } = 0;
    }
}