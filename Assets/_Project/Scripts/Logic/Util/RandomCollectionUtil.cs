using System.Collections.Generic;

namespace _Project.Scripts.Logic.Util
{
    public static class RandomCollectionUtil
    {
        public static List<T> GetRandomElementsFromCollection<T>(IList<T> collection, int element)
        {
            var pickedIndexes = new HashSet<int>();
            var result = new List<T>();
            
            while (pickedIndexes.Count < element)
            {
                var index = UnityEngine.Random.Range(0, collection.Count);
                if (pickedIndexes.Contains(index))
                {
                    continue;
                }
                pickedIndexes.Add(index);
                result.Add(collection[index]);
            }
            
            return result;
        }
        
    }
}