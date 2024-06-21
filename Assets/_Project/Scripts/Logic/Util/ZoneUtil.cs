using System.Collections.Generic;
using System.Linq;
using _Project.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Logic.Util
{
    public static class ZoneUtil
    {
        public static DescendZone GetZoneFromDepth(List<DescendZone> zones, float depth)
        {
            return zones.First(zone => depth > zone.threshold);
        }
    }
}
