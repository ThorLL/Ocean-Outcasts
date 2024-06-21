using System;
using System.Linq;
using UnityEngine;

namespace _Project.Scripts.Logic.Util
{
    // Use the easing cheat sheep https://easings.net
    public static class Easing
    {
        public static Func<float, float> EasingLerp(float a, float b, params Func<float, float>[] easing) =>
            t => Mathf.Lerp(a, b, easing.Aggregate(t, (current, func) => func(current)));

        public static float EaseInQuad(float x) => x * x;  
        
        public static float EaseOutQuart(float x) => 1 - Mathf.Pow(1 - x, 4);
        public static float EaseOutQuad(float x) => 1 - (1 - x) * (1 - x);

        public static float EaseSineWave(float x) => (Mathf.Sin(2 * Mathf.PI* x * + Mathf.PI / 2) + 1) / 2;
    }
}