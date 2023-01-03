using System.Collections.Generic;
using UnityEngine;

namespace NPP.DE.Core.Dungeon.Generator
{

    // 1 = wall, 0 = corridor
    public class MazeRecursiveDFS : MazeBase
    {
        protected List<Coordinate> Direction = new List<Coordinate>
        {
            new Coordinate(0,1),
            new Coordinate(1,0),
            new Coordinate(-1,0),
            new Coordinate(0,-1)
        };

        protected override void Generate()
        {
            Generate(Map, Random.Range(1, Settings.XValue), Random.Range(1, Settings.ZValue));
        }

        void Generate(byte[,] map, int x, int z)
        {
            if (CountNeighborSquare(map, x, z) >= 2) return;
            map[x, z] = 0;

            Direction.Shuffle();

            Generate(map, x + Direction[0].X, z + Direction[0].Z);
            Generate(map, x + Direction[1].X, z + Direction[1].Z);
            Generate(map, x + Direction[2].X, z + Direction[2].Z);
            Generate(map, x + Direction[3].X, z + Direction[3].Z);
        }
    }
}