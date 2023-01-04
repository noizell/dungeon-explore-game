using SpatialHashTable.CullingSystem;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NPP.DE.Core.Dungeon.Generator
{
    //horizontal = x;
    //vertical = z;
    public abstract class MazeBase
    {
        protected class WallRoomPlacement
        {
            public readonly Vector3 Right = new Vector3(0f, 90f, 0f);
            public readonly Vector3 Up = new Vector3(0f, 0f, 0f);
            public readonly Vector3 Down = new Vector3(0f, 180f, 0f);
            public readonly Vector3 Left = new Vector3(0f, -90f, 0f);

            public enum WallDirection { Up, Left, Right, Down, Invalid }

            public bool TryLocateWall(byte[,] map, int x, int z, int width, int depth, int targetType, out List<WallDirection> wallDirection)
            {
                wallDirection = new List<WallDirection>();
                if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return false;

                if (map[x, z + 1] == targetType)
                    wallDirection.Add(WallDirection.Up);
                if (map[x, z - 1] == targetType)
                    wallDirection.Add(WallDirection.Down);
                if (map[x - 1, z] == targetType)
                    wallDirection.Add(WallDirection.Left);
                if (map[x + 1, z] == targetType)
                    wallDirection.Add(WallDirection.Right);

                return true;
            }
        }

        protected class DoorRoomPlacement
        {
            public readonly Vector3 Right = new Vector3(0f, 90f, 0f);
            public readonly Vector3 Up = new Vector3(0f, 180f, 0f);

            public Vector3 GetDirection(byte[,] map, int x, int z)
            {
                if ((map[x, z + 1] == 2 && map[x, z - 1] == 0) || (map[x, z - 1] == 2 && map[x, z + 1] == 0))
                    return Up;

                return Right;
            }

            public bool LocateDoorPlacement(byte[,] map, int x, int z, int targetType)
            {
                //if ((map[x, z + 1] == 2 && map[x + 1, z - 1] == targetType && map[x - 1, z - 1] == targetType) || (map[x, z - 1] == 2 && map[x + 1, z + 1] == targetType && map[x - 1, z - 1] == targetType))
                return (((map[x, z + 1] == 2 && map[x, z - 1] == 0) || (map[x + 1, z] == 2 && map[x - 1, z] == 0)) ||
                    ((map[x, z - 1] == 2 && map[x, z + 1] == 0) || (map[x - 1, z] == 2 && map[x + 1, z] == 0)));
            }
        }

        protected class DeadEndCorridor
        {
            public readonly Vector3 Right = new Vector3(0f, -90f, 0f);
            public readonly Vector3 Left = new Vector3(0f, 90f, 0f);
            public readonly Vector3 Up = new Vector3(0f, 180f, 0f);
            public readonly Vector3 Down = new Vector3(0f, 0f, 0f);

            public Vector3 GetDirection(byte[,] map, int x, int z)
            {
                if ((map[x - 1, z] == 0) || (map[x - 1, z] == 2))
                    return Left;
                if ((map[x + 1, z] == 0) || (map[x + 1, z] == 2))
                    return Right;
                if ((map[x, z + 1] == 0) || (map[x, z + 1] == 2))
                    return Up;

                return Down;
            }
        }

        protected class CornerCorridor
        {
            public readonly Vector3 BotRight = new Vector3(0f, 180f, 0f);
            public readonly Vector3 TopRight = new Vector3(0f, 0f, 0f);
            public readonly Vector3 BotLeft = new Vector3(0f, -90f, 0f);
            public readonly Vector3 TopLeft = new Vector3(0f, 90f, 0f);

            public Vector3 GetDirection(byte[,] map, int x, int z)
            {
                if ((map[x - 1, z] == 0 && map[x, z + 1] == 0) ||
                    (map[x - 1, z] == 2 && map[x, z + 1] == 2) ||
                    (map[x - 1, z] == 2 && map[x, z + 1] == 0) ||
                    (map[x - 1, z] == 0 && map[x, z + 1] == 2))
                    return BotRight;

                if ((map[x - 1, z] == 0 && map[x, z - 1] == 0) ||
                    (map[x - 1, z] == 2 && map[x, z - 1] == 2) ||
                    (map[x - 1, z] == 2 && map[x, z - 1] == 0) ||
                    (map[x - 1, z] == 0 && map[x, z - 1] == 2))
                    return TopLeft;

                if ((map[x + 1, z] == 0 && map[x, z + 1] == 0) ||
                    (map[x + 1, z] == 2 && map[x, z + 1] == 2) ||
                    (map[x + 1, z] == 2 && map[x, z + 1] == 0) ||
                    (map[x + 1, z] == 0 && map[x, z + 1] == 2))
                    return BotLeft;

                return TopRight;
            }
        }

        protected class TJunctionCorridor
        {
            public readonly Vector3 UpDownLeft = new Vector3(0f, 180f, 0f);
            public readonly Vector3 UpDownRight = new Vector3(0f, 0f, 0f);
            public readonly Vector3 LeftRightUp = new Vector3(0f, -90f, 0f);
            public readonly Vector3 LeftRightDown = new Vector3(0f, 90f, 0f);

            public Vector3 GetDirection(byte[,] map, int x, int z)
            {
                if ((map[x - 1, z] == 0 && (map[x, z - 1] == 0) && (map[x, z + 1] == 0)) ||
                    (map[x - 1, z] == 0 && (map[x, z - 1] == 0) && (map[x, z + 1] == 2)) ||
                    (map[x - 1, z] == 0 && (map[x, z - 1] == 2) && (map[x, z + 1] == 0)) ||
                    (map[x - 1, z] == 2 && (map[x, z - 1] == 0) && (map[x, z + 1] == 0)) ||
                    (map[x - 1, z] == 2 && (map[x, z - 1] == 0) && (map[x, z + 1] == 2)) ||
                    (map[x - 1, z] == 2 && (map[x, z - 1] == 2) && (map[x, z + 1] == 0)) ||
                    (map[x - 1, z] == 2 && (map[x, z - 1] == 2) && (map[x, z + 1] == 2)) ||
                    (map[x - 1, z] == 0 && (map[x, z - 1] == 2) && (map[x, z + 1] == 2)) ||
                    (map[x - 1, z] == 0 && (map[x, z - 1] == 0) && (map[x, z + 1] == 2)))
                    return UpDownLeft;
                if ((map[x, z - 1] == 0 && map[x + 1, z] == 0 && map[x - 1, z] == 0) ||
                    (map[x, z - 1] == 0 && map[x + 1, z] == 0 && map[x - 1, z] == 2) ||
                    (map[x, z - 1] == 0 && map[x + 1, z] == 2 && map[x - 1, z] == 0) ||
                    (map[x, z - 1] == 2 && map[x + 1, z] == 0 && map[x - 1, z] == 0) ||
                    (map[x, z - 1] == 2 && map[x + 1, z] == 0 && map[x - 1, z] == 2) ||
                    (map[x, z - 1] == 2 && map[x + 1, z] == 2 && map[x - 1, z] == 0) ||
                    (map[x, z - 1] == 2 && map[x + 1, z] == 2 && map[x - 1, z] == 2) ||
                    (map[x, z - 1] == 0 && map[x + 1, z] == 2 && map[x - 1, z] == 2) ||
                    (map[x, z - 1] == 0 && map[x + 1, z] == 0 && map[x - 1, z] == 2))
                    return LeftRightDown;
                if ((map[x, z + 1] == 0 && map[x + 1, z] == 0 && map[x - 1, z] == 0) ||
                    (map[x, z + 1] == 0 && map[x + 1, z] == 0 && map[x - 1, z] == 2) ||
                    (map[x, z + 1] == 0 && map[x + 1, z] == 2 && map[x - 1, z] == 0) ||
                    (map[x, z + 1] == 2 && map[x + 1, z] == 0 && map[x - 1, z] == 0) ||
                    (map[x, z + 1] == 2 && map[x + 1, z] == 0 && map[x - 1, z] == 2) ||
                    (map[x, z + 1] == 2 && map[x + 1, z] == 2 && map[x - 1, z] == 0) ||
                    (map[x, z + 1] == 2 && map[x + 1, z] == 2 && map[x - 1, z] == 2) ||
                    (map[x, z + 1] == 0 && map[x + 1, z] == 2 && map[x - 1, z] == 2) ||
                    (map[x, z + 1] == 0 && map[x + 1, z] == 0 && map[x - 1, z] == 2))
                    return LeftRightUp;

                return UpDownRight;
            }
        }

        [SerializeField]
        protected MazeSettings Settings;

        protected EzCombine Combiner;
        protected byte[,] Map;

        GameObject _tempFloor;
        GameObject _tempWall;
        GameObject _tempCorridor;

        protected DeadEndCorridor _deadEndHelper;
        protected CornerCorridor _cornerHelper;
        protected TJunctionCorridor _tJunctionHelper;
        protected DoorRoomPlacement _doorPlacementHelper;
        protected WallRoomPlacement _wallPlacementHelper;
        public FOWSystem FOWSystem { get; private set; }

        public void StartMaze(MazeSettings setting)
        {
            Settings = setting;
            StartMaze();
        }

        public void StartMaze()
        {
            if (Settings == null) return;

            Initialize();
            Generate();
            GenerateRoom();
            Draw();
        }

        protected void Initialize()
        {
            Map = new byte[Settings.XValue, Settings.ZValue];
            LoopMap(Settings.XValue, Settings.ZValue,
                (int x, int z) =>
                {
                    Map[x, z] = 1; // 1 = wall, 0 = corridor, 2 =room
                });

            _cornerHelper = new CornerCorridor();
            _tJunctionHelper = new TJunctionCorridor();
            _deadEndHelper = new DeadEndCorridor();
            _doorPlacementHelper = new DoorRoomPlacement();
            _wallPlacementHelper = new WallRoomPlacement();

            if (Settings.EnableCulling)
                InitializeCullSystem();
        }

        protected virtual void Generate()
        {

        }

        protected virtual void GenerateRoom()
        {
            if (Settings.DrawRoom)
            {
                for (int i = 0; i < Settings.RoomCount; i++)
                {
                    int startX = Random.Range(3, Settings.XValue - 3);
                    int startZ = Random.Range(3, Settings.ZValue - 3);
                    int roomX = Random.Range(Settings.MinXRoomSize, Settings.MaxXRoomSize);
                    int roomZ = Random.Range(Settings.MinZRoomSize, Settings.MaxZRoomSize);

                    for (int x = startX; x < Settings.XValue - 3 && x < startX + roomX; x++)
                    {
                        for (int z = startZ; z < Settings.ZValue - 3 && z < startZ + roomZ; z++)
                        {
                            Map[x, z] = 2;
                        }
                    }
                }
            }
        }

        protected void Draw()
        {
            GameObject floorParent = new GameObject("Floors");
            GameObject wallParent = new GameObject("Walls");
            GameObject tempWallParent = new GameObject("Temp Wall");
            LoopMap(Settings.XValue, Settings.ZValue,
                (int x, int z) =>
                {
                    //draw wall.
                    if (IsEligibleDrawWall(x, z))
                    {
                        DrawWall(x, z, tempWallParent);
                    }
                    else
                    {
                        if (!Settings.DrawWall)
                            GameObject.Destroy(tempWallParent);

                        DrawCorridor(Map, x, z, wallParent);
                        DrawRoom(Map, x, z, wallParent);
                    }
                });

            SeparateFloor(floorParent);

            //combine meshes.
            if (Settings.CombineDungeonMeshes)
                CombineInstancedMeshes(floorParent, wallParent);
            else
            {
                if (Settings.EnableCulling)
                {
                    AddCullableComponent(floorParent);
                    AddCullableComponent(wallParent);
                }
            }

            if (Settings.EnableFogOfWar)
            {
                InitializeFogOfWar();
            }

            if (Settings.EnableVerticalFog)
            {
                InitializeVerticalFog();
            }
        }

        private void InitializeVerticalFog()
        {
            GameObject _verticalFog = GameObject.Instantiate(Settings.VerticalFog);
            _verticalFog.transform.localScale = new Vector3(Settings.FogX, Settings.FogY, Settings.FogZ);
            _verticalFog.transform.localPosition = new Vector3((Settings.XValue / 2) * Settings.GlobalScaleMultiplier * Settings.Floor.ScaleX, Settings.FogHeight, (Settings.ZValue / 2) * Settings.GlobalScaleMultiplier * Settings.Floor.ScaleZ);

        }

        protected void InitializeFogOfWar()
        {
            FOWSystem = GameObject.Instantiate(Settings.FOWSystemPrefab);
            FOWSystem.transform.localPosition = new Vector3((Settings.XValue / 2) * Settings.GlobalScaleMultiplier * Settings.Floor.ScaleX , 0f, (Settings.ZValue / 2) * Settings.GlobalScaleMultiplier * Settings.Floor.ScaleZ);
            FOWSystem.worldSize = 1000;
            FOWSystem.heightRange = new Vector2(-200, 200);
        }

        protected void SeparateFloor(GameObject floorParent)
        {
            foreach (GameObject floor in GameObject.FindGameObjectsWithTag("Floor"))
            {
                floor.transform.SetParent(floorParent.transform);
            }
        }

        protected bool IsEligibleDrawRoom(int x, int z)
        {
            return Map[x, z] == 2;
        }

        protected bool IsEligibleDrawWall(int x, int z)
        {
            return Map[x, z] == 1;
        }

        protected bool IsEligibleDrawCorridor(int x, int z)
        {
            return Map[x, z] == 0;
        }

        protected void DrawCorridor(byte[,] map, int x, int z, GameObject wallParent)
        {
            if (IsEligibleDrawCorridor(x, z))
            {
                if (IsCrossroad(map, x, z))
                {
                    _tempCorridor = GameObject.Instantiate(Settings.CrossCorridor.TileMesh, wallParent.transform);
                    _tempCorridor.transform.localScale = _tempCorridor.transform.localScale * Settings.GlobalScaleMultiplier;
                    AdjustPosition(_tempCorridor, Settings.CrossCorridor.ScaleX, Settings.Floor.ScaleY, Settings.CrossCorridor.ScaleZ, x, z);
                }
                else
                {
                    int h = CountNeighborHorizontal(map, x, z);
                    int v = CountNeighborVertical(map, x, z);
                    int hr = CountNeighborHorizontal(map, x, z, 2);
                    int vr = CountNeighborVertical(map, x, z, 2);

                    if (h == 2 || v == 2 || hr == 2 || vr == 2 || (h + hr) == 2 || (v + vr) == 2)
                    {
                        //T Junction.
                        if ((h + v + hr + vr) >= 3)
                        {
                            _tempCorridor = GameObject.Instantiate(Settings.TCorridor.TileMesh, wallParent.transform);
                            _tempCorridor.transform.localScale = _tempCorridor.transform.localScale * Settings.GlobalScaleMultiplier;
                            AdjustPosition(_tempCorridor, Settings.TCorridor.ScaleX, Settings.Floor.ScaleY, Settings.TCorridor.ScaleZ, x, z);
                            _tempCorridor.transform.localRotation = Quaternion.Euler(_tJunctionHelper.GetDirection(map, x, z));
                        }
                        //its normal corridor
                        else
                        {
                            _tempCorridor = GameObject.Instantiate(Settings.StraightCorridor.TileMesh, wallParent.transform);
                            _tempCorridor.transform.localScale = _tempCorridor.transform.localScale * Settings.GlobalScaleMultiplier;
                            AdjustPosition(_tempCorridor, Settings.StraightCorridor.ScaleX, Settings.Floor.ScaleY, Settings.StraightCorridor.ScaleZ, x, z);

                            //its horizontal straight corridor, otherwise vertical straight corridor.
                            if ((h + hr) > (v + vr))
                            {
                                _tempCorridor.transform.localRotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
                            }
                        }
                    }
                    //Corner.
                    else if ((h == 1 && v == 1) || (hr == 1 && vr == 1) || (h == 1 && vr == 1) || (hr == 1 && v == 1))
                    {
                        _tempCorridor = GameObject.Instantiate(Settings.CornerCorridor.TileMesh, wallParent.transform);
                        _tempCorridor.transform.localScale = _tempCorridor.transform.localScale * Settings.GlobalScaleMultiplier;
                        AdjustPosition(_tempCorridor, Settings.CornerCorridor.ScaleX, Settings.Floor.ScaleY, Settings.CornerCorridor.ScaleZ, x, z);
                        _tempCorridor.transform.localRotation = Quaternion.Euler(_cornerHelper.GetDirection(map, x, z));
                    }
                    //dead end.
                    else if (h == 1 || v == 1 || vr == 1 || hr == 1)
                    {
                        _tempCorridor = GameObject.Instantiate(Settings.DeadEndCorridor.TileMesh, wallParent.transform);
                        _tempCorridor.transform.localScale = _tempCorridor.transform.localScale * Settings.GlobalScaleMultiplier;
                        AdjustPosition(_tempCorridor, Settings.DeadEndCorridor.ScaleX, Settings.Floor.ScaleY, Settings.DeadEndCorridor.ScaleZ, x, z);

                        _tempCorridor.transform.localRotation = Quaternion.Euler(_deadEndHelper.GetDirection(map, x, z));
                    }
                }

                void AdjustPosition(GameObject _tempCorridor, float scaleX, float scaleY, float scaleZ, int x, int z)
                {
                    _tempCorridor.transform.localPosition = new Vector3(x * scaleX * Settings.GlobalScaleMultiplier, scaleY * Settings.GlobalScaleMultiplier, z * scaleZ * Settings.GlobalScaleMultiplier);
                }
            }
        }

        private bool IsCrossroad(byte[,] map, int x, int z)
        {
            return ((CountNeighborHorizontal(map, x, z) == 2 && CountNeighborVertical(map, x, z) == 2) ||
                (CountNeighborHorizontal(map, x, z, 2) == 2 && CountNeighborVertical(map, x, z, 2) == 2) ||
                (CountNeighborHorizontal(map, x, z) == 2 && CountNeighborVertical(map, x, z, 2) == 2) ||
                (CountNeighborHorizontal(map, x, z, 2) == 2 && CountNeighborVertical(map, x, z) == 2) ||
                (((CountNeighborHorizontal(map, x, z) + (CountNeighborHorizontal(map, x, z, 2))) == 2) &&
                (CountNeighborVertical(map, x, z, 2) + (CountNeighborVertical(map, x, z))) == 2));
        }

        protected void DrawWall(int x, int z, GameObject wallParent)
        {
            if (Settings.DrawWall)
            {
                _tempWall = GameObject.Instantiate(Settings.Wall.TileMesh, wallParent.transform);
                _tempWall.transform.localScale = _tempWall.transform.localScale * Settings.GlobalScaleMultiplier;
                _tempWall.transform.localPosition = new Vector3(x * Settings.Wall.ScaleX * Settings.GlobalScaleMultiplier, Settings.Floor.ScaleY * Settings.GlobalScaleMultiplier, z * Settings.Wall.ScaleZ * Settings.GlobalScaleMultiplier);
            }
        }

        protected void DrawRoom(byte[,] map, int x, int z, GameObject wallParent)
        {
            if (Settings.DrawRoom)
            {
                if (map[x, z] == 2 && (CountNeighborSquare(map, x, z, 2) > 1 && CountNeighborDiagonal(map, x, z, 2) >= 1 || CountNeighborSquare(map, x, z, 2) >= 1 && CountNeighborDiagonal(map, x, z, 2) > 1))
                {
                    _tempWall = GameObject.Instantiate(Settings.FloorRoom.TileMesh, wallParent.transform);
                    _tempWall.transform.localScale = _tempWall.transform.localScale * Settings.GlobalScaleMultiplier;
                    AdjustPosition(_tempWall, Settings.FloorRoom.ScaleX, Settings.Floor.ScaleY, Settings.FloorRoom.ScaleZ, x, z);

                    if (_wallPlacementHelper.TryLocateWall(map, x, z, Settings.XValue, Settings.ZValue, 1, out List<WallRoomPlacement.WallDirection> wallDir))
                    {
                        foreach (WallRoomPlacement.WallDirection wall in wallDir)
                        {
                            if (wall == WallRoomPlacement.WallDirection.Up)
                            {
                                GameObject _tempWall = GameObject.Instantiate(Settings.WallRoom.TileMesh, wallParent.transform);
                                AdjustPosition(_tempWall, Settings.WallRoom.ScaleX, Settings.Floor.ScaleY, Settings.WallRoom.ScaleZ, x, z);
                                _tempWall.transform.localScale = _tempWall.transform.localScale * Settings.GlobalScaleMultiplier;
                                _tempWall.transform.localRotation = Quaternion.Euler(_wallPlacementHelper.Up);
                            }

                            if (wall == WallRoomPlacement.WallDirection.Down)
                            {
                                GameObject _tempWall = GameObject.Instantiate(Settings.WallRoom.TileMesh, wallParent.transform);
                                AdjustPosition(_tempWall, Settings.WallRoom.ScaleX, Settings.Floor.ScaleY, Settings.WallRoom.ScaleZ, x, z);
                                _tempWall.transform.localScale = _tempWall.transform.localScale * Settings.GlobalScaleMultiplier;
                                _tempWall.transform.localRotation = Quaternion.Euler(_wallPlacementHelper.Down);
                            }

                            if (wall == WallRoomPlacement.WallDirection.Left)
                            {
                                GameObject _tempWall = GameObject.Instantiate(Settings.WallRoom.TileMesh, wallParent.transform);
                                AdjustPosition(_tempWall, Settings.WallRoom.ScaleX, Settings.Floor.ScaleY, Settings.WallRoom.ScaleZ, x, z);
                                _tempWall.transform.localScale = _tempWall.transform.localScale * Settings.GlobalScaleMultiplier;
                                _tempWall.transform.localRotation = Quaternion.Euler(_wallPlacementHelper.Left);
                            }

                            if (wall == WallRoomPlacement.WallDirection.Right)
                            {
                                GameObject _tempWall = GameObject.Instantiate(Settings.WallRoom.TileMesh, wallParent.transform);
                                AdjustPosition(_tempWall, Settings.WallRoom.ScaleX, Settings.Floor.ScaleY, Settings.WallRoom.ScaleZ, x, z);
                                _tempWall.transform.localScale = _tempWall.transform.localScale * Settings.GlobalScaleMultiplier;
                                _tempWall.transform.localRotation = Quaternion.Euler(_wallPlacementHelper.Right);
                            }
                        }
                    }
                }

            }

            void AdjustPosition(GameObject _tempCorridor, float scaleX, float scaleY, float scaleZ, int x, int z)
            {
                _tempCorridor.transform.localPosition = new Vector3(x * scaleX * Settings.GlobalScaleMultiplier, scaleY * Settings.GlobalScaleMultiplier, z * scaleZ * Settings.GlobalScaleMultiplier);
            }
        }

        protected void InitializeCullSystem()
        {
            CullSystemManager cullSystem = GameObject.Instantiate(Settings.CullSystemPrefab).GetComponent<CullSystemManager>();
            cullSystem.cullDistance = 80;
            cullSystem.GetComponent<CenterOfCullSystem>().centerOfCullSystem = Camera.main.transform;
        }

        protected void AddCullableComponent(GameObject parent)
        {
            if (parent.transform.childCount < 1) return;
            foreach (MeshRenderer _renderer in parent.GetComponentsInChildren<MeshRenderer>())
            {
                CullableObjectTag cull = _renderer.gameObject.AddComponent<CullableObjectTag>();
                CullMeshRenderer cullMesh = _renderer.gameObject.AddComponent<CullMeshRenderer>();
            }
        }

        protected void CombineInstancedMeshes(GameObject floorParent, GameObject wallParent)
        {
            foreach (Transform wallMember in wallParent.transform.GetComponentsInChildren<Transform>())
            {
                if (wallMember.transform.childCount > 0)
                    foreach (MeshRenderer _tempWallChild in wallMember.transform.GetComponentsInChildren<MeshRenderer>())
                    {
                        _tempWallChild.transform.SetParent(wallParent.transform);
                    }
            }

            GameObject CombinedMeshes = new GameObject("Combined Meshes");
            Combiner = new GameObject("Dungeon Combiner").AddComponent<EzCombine>();
            Combiner.options = new EzCombine.OptionsClass();
            Combiner.chunksParent = CombinedMeshes.transform;
            Combiner.options.chunkSize = 8;

            Combiner.combine = new EzCombine.CombineClass[2];

            Combiner.combine[0] = new EzCombine.CombineClass();
            Combiner.combine[0].rootTransform = floorParent.transform;
            Combiner.combine[0].chunkName = "Floor";
            Combiner.combine[0].tag = "Floor";
            Combiner.combine[0].addMeshCollider = true;

            Combiner.combine[1] = new EzCombine.CombineClass();
            Combiner.combine[1].rootTransform = wallParent.transform;
            Combiner.combine[1].chunkName = "Wall";
            Combiner.combine[1].tag = "Wall";
            Combiner.combine[1].addMeshCollider = true;

            Combiner.CombineCall();
            GameObject.Destroy(floorParent);
            GameObject.Destroy(wallParent);

            AddCullableComponent(CombinedMeshes);
        }

        protected void LoopMap(int xVal, int zVal, System.Action<int, int> action)
        {
            for (int x = 0; x < xVal; x++)
            {
                for (int z = 0; z < zVal; z++)
                {
                    action?.Invoke(x, z);
                }
            }
        }

        protected int CountNeighbor(byte[,] map, int x, int z, int targetType = 0)
        {
            return CountNeighborDiagonal(map, x, z, targetType) + CountNeighborSquare(map, x, z, targetType);
        }

        protected int CountNeighborDiagonal(byte[,] map, int x, int z, int targetType = 0)
        {
            int count = 0;
            if (IsDoneGenerating(x, z, Settings.XValue, Settings.ZValue)) return 5;

            if (map[x - 1, z + 1] == targetType) count++;
            if (map[x + 1, z - 1] == targetType) count++;
            if (map[x - 1, z - 1] == targetType) count++;
            if (map[x + 1, z + 1] == targetType) count++;

            return count;
        }

        protected int CountNeighborSquare(byte[,] map, int x, int z, int targetType = 0)
        {
            int count = 0;
            if (IsDoneGenerating(x, z, Settings.XValue, Settings.ZValue)) return 5;

            if (map[x, z + 1] == targetType) count++;
            if (map[x, z - 1] == targetType) count++;
            if (map[x + 1, z] == targetType) count++;
            if (map[x - 1, z] == targetType) count++;

            return count;
        }

        protected int CountNeighborVertical(byte[,] map, int x, int z, int targetType = 0)
        {
            int count = 0;
            if (IsDoneGenerating(x, z, Settings.XValue, Settings.ZValue)) return 5;

            if (map[x, z + 1] == targetType) count++;
            if (map[x, z - 1] == targetType) count++;

            return count;
        }
        protected int CountNeighborHorizontal(byte[,] map, int x, int z, int targetType = 0)
        {
            int count = 0;
            if (IsDoneGenerating(x, z, Settings.XValue, Settings.ZValue)) return 5;

            if (map[x + 1, z] == targetType) count++;
            if (map[x - 1, z] == targetType) count++;

            return count;
        }

        protected bool IsDoneGenerating(int x, int z, int maxX, int maxZ)
        {
            return (x >= maxX - 1 || z >= maxZ - 1 || x <= 0 || z <= 0);
        }
    }
}