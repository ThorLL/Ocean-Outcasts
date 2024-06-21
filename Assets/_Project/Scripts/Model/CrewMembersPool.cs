using System.Collections.Generic;
using Newtonsoft.Json;

namespace _Project.Scripts.Model
{
    public class CrewMembersPool
    {
        [JsonProperty("Names")] 
        public List<string> Names;
        [JsonProperty("Sprites")] 
        public List<string> Sprites;
        [JsonProperty("SkillsWithBio")] 
        public List<CrewMember> SkillsWithBio;
    }
}