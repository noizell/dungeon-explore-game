
using System.Collections.Generic;
using UnityEngine;

namespace NPP.DE.Core.Dungeon.Generator
{

    // 1 = wall, 0 = corridor
    public class MazePrism : MazeBase
    {
        protected override void Generate()
        {
            int x = Settings.XValue / 2;
            int z = Settings.ZValue / 2;
            Map[x, z] = 0;

            List<Coordinate> walls = new List<Coordinate> {
                new Coordinate(x+1, z),
                new Coordinate(x-1, z),
                new Coordinate(x, z+1),
                new Coordinate(x, z-1)
            };

            int countLoops = 0;
            while (walls.Count > 0 && countLoops < 2000)
            {
                int rWall = Random.Range(0, walls.Count);
                x = walls[rWall].X;
                z = walls[rWall].Z;
                walls.RemoveAt(rWall);
                if (CountNeighborSquare(Map, x, z) == 1)
                {
                    Map[x, z] = 0;
                    walls.Add(new Coordinate(x + 1, z));
                    walls.Add(new Coordinate(x - 1, z));
                    walls.Add(new Coordinate(x, z + 1));
                    walls.Add(new Coordinate(x, z - 1));
                }
                countLoops++;
            }
        }

    }
}