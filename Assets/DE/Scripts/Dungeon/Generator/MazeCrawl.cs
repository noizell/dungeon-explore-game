using UnityEngine;

namespace NPP.DE.Core.Dungeon.Generator
{

    public class MazeCrawl : MazeBase
    {
        protected enum CrawlDirection { Horizontal = 1, Vertical = 0, Random = 2 }

        protected override void Generate()
        {
            for (int i = 0; i < 2; i++)
                Crawl(Map, 1, Random.Range(0, Settings.ZValue - 1), CrawlDirection.Vertical);

            for (int i = 0; i < 3; i++)
                Crawl(Map, (Settings.XValue / 2) - 1, 1, CrawlDirection.Horizontal);
        }

        protected void Crawl(byte[,] map, int xStartPos, int zStartPos, CrawlDirection direction = CrawlDirection.Random)
        {
            bool done = false;
            int x = xStartPos;
            int z = zStartPos;

            while (!done)
            {
                map[x, z] = 0;

                if (direction == CrawlDirection.Random)
                {
                    if (Random.value > .3f)
                    {
                        x += (Random.value > .4f) ? Random.Range(0, 2) : Random.Range(-1, 2);
                    }
                    else
                    {
                        z += (Random.value > .4f) ? Random.Range(0, 2) : Random.Range(-1, 2);
                    }
                }
                else
                {
                    if (Random.value > .3f)
                    {
                        x += direction == CrawlDirection.Vertical ? Random.Range(0, 2) : Random.Range(-1, 2);
                    }
                    else
                        z += direction == CrawlDirection.Horizontal ? Random.Range(0, 2) : Random.Range(-1, 2);
                }

                done |= IsDoneGenerating(x, z, Settings.XValue, Settings.ZValue);
            }
        }
        protected virtual void Crawl(byte[,] map, int xStartPos, int zStartPos, CrawlDirection direction = CrawlDirection.Random, System.Action<int, int> OnEachCoordinate = null)
        {
            bool done = false;
            int x = xStartPos;
            int z = zStartPos;

            while (!done)
            {
                map[x, z] = 0;

                if (direction == CrawlDirection.Random)
                {
                    if (Random.value > .3f)
                    {
                        x += (Random.value > .4f) ? Random.Range(0, 2) : Random.Range(-1, 2);
                    }
                    else
                    {
                        z += (Random.value > .4f) ? Random.Range(0, 2) : Random.Range(-1, 2);
                    }
                }
                else
                {
                    if (Random.value > .3f)
                    {
                        x += direction == CrawlDirection.Vertical ? Random.Range(0, 2) : Random.Range(-1, 2);
                    }
                    else
                        z += direction == CrawlDirection.Horizontal ? Random.Range(0, 2) : Random.Range(-1, 2);
                }

                OnEachCoordinate?.Invoke(x, z);

                done |= IsDoneGenerating(x, z, Settings.XValue, Settings.ZValue);
            }
        }

        protected virtual void Crawl(byte[,] map, int xMin, int zMin, int xMax, int zMax, CrawlDirection direction = CrawlDirection.Random, System.Action<int, int> OnEachCoordinate = null)
        {
            bool done = false;
            int x = Random.Range(xMin, xMax);
            int z = Random.Range(zMin, zMax);

            while (!done)
            {
                map[x, z] = 0;

                if (direction == CrawlDirection.Random)
                {
                    if (Random.value > .3f)
                    {
                        x += (Random.value > .4f) ? Random.Range(0, 2) : Random.Range(-1, 2);
                    }
                    else
                    {
                        z += (Random.value > .4f) ? Random.Range(0, 2) : Random.Range(-1, 2);
                    }
                }
                else
                {
                    if (Random.value > .3f)
                    {
                        x += direction == CrawlDirection.Vertical ? Random.Range(0, 2) : Random.Range(-1, 2);
                    }
                    else
                        z += direction == CrawlDirection.Horizontal ? Random.Range(0, 2) : Random.Range(-1, 2);
                }

                OnEachCoordinate?.Invoke(x, z);

                done |= IsDoneGenerating(x, z, Settings.XValue, Settings.ZValue);
            }
        }
    }
}