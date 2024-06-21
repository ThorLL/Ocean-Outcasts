using UnityEngine;

namespace _Project.Scripts.Logic.Util
{
    public static class SkillFormatUtil
    {
        public static string FormatSkill(string title, float skill)
        {
            return $"{title}: {Mathf.FloorToInt(skill)}";
        }
    }
}