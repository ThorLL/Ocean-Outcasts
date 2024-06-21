using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Logic.Util
{
    public static class ResourceLoaderUtil
    {
        private static readonly Dictionary<string, Object> LoadedAssets = new();
        
        public static T GetAsset<T>(string path) where T : Object
        {
            if (LoadedAssets.TryGetValue(path, out var asset))
            {
                return asset as T;
            }
            var loadedAsset = Resources.Load<T>(path);
            LoadedAssets[path] = loadedAsset;
            return loadedAsset;
        }
    }
}