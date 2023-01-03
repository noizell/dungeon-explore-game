using System.Collections.Generic;

namespace NPP.DE.Core.Dungeon.Generator
{
    public static class MazeRecursiveDFSExtension
    {
        public static System.Random Rand = new System.Random();

        public static void Shuffle<T>(this List<T> list) where T : Coordinate
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Rand.Next(n + 1);
                T val = list[k];
                list[k] = list[n];
                list[n] = val;
            }
        }
    }
}