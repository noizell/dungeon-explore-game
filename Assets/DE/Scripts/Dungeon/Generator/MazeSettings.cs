using SpatialHashTable.CullingSystem;
using UnityEngine;

namespace NPP.DE.Core.Dungeon.Generator
{
    [CreateAssetMenu(fileName = "Maze Settings", menuName = "NPP/DE/Dungeon/Create Maze Settings")]
    public class MazeSettings : ScriptableObject
    {
        [Header("Dungeon Components")]

        [SerializeField]
        private Tile _wall;

        [SerializeField]
        private Tile _floor;

        [SerializeField]
        private Tile _straightCorridor;
        [SerializeField]
        private Tile _crossCorridor;
        [SerializeField]
        private Tile _tCorridor;
        [SerializeField]
        private Tile _cornerCorridor;
        [SerializeField]
        private Tile _deadEndCorridor;

        [Header("Room Components")]
        [SerializeField]
        private Tile _floorRoom;
        [SerializeField]
        private Tile _wallRoom;
        [SerializeField]
        private Tile _ceilingRoom;
        [SerializeField]
        private Tile _doorRoom;

        [Header("Dungeon Size")]
        [SerializeField]
        private int _xValue;

        [SerializeField]
        private int _zValue;

        [SerializeField]
        [Range(1, 30)]
        private int _roomCount;

        [SerializeField]
        [Range(1, 30)] private int _minXRoomSize;

        [SerializeField]
        [Range(1, 30)] private int _maxXRoomSize;

        [SerializeField]
        [Range(1, 30)] private int _minZRoomSize;

        [SerializeField]
        [Range(1, 30)] private int _maxZRoomSize;

        [SerializeField]
        [Range(1f, 40f)] private float _globalScaleMultiplier = 1f;

        [SerializeField]
        [Range(1, 1000f)] private float _fogHeight;

        [SerializeField]
        [Range(1f, 400f)] private float _fogScaleX;

        [SerializeField]
        [Range(1f, 400f)] private float _fogScaleY;

        [SerializeField]
        [Range(1f, 400f)] private float _fogScaleZ;

        [Header("Dungeon Settings")]
        [SerializeField]
        private CullSystemDependencyManager _cullSystem;
        [SerializeField]
        private FOWSystem _fowSystem;
        [SerializeField]
        private GameObject _verticalFog;

        [SerializeField]
        private bool _enableFogOfWar;
        [SerializeField]
        private bool _combineDungeonMeshes;
        [SerializeField]
        private bool _enableVerticalFog;
        [SerializeField]
        private bool _enableCulling;
        [SerializeField]
        private bool _drawWall;
        [SerializeField]
        private bool _drawRoom;

        public int XValue => _xValue;
        public int ZValue => _zValue;
        public float GlobalScaleMultiplier => _globalScaleMultiplier;
        public Tile Wall => _wall;
        public Tile Floor => _floor;
        public Tile StraightCorridor => _straightCorridor;
        public Tile CrossCorridor => _crossCorridor;
        public Tile TCorridor => _tCorridor;
        public Tile CornerCorridor => _cornerCorridor;
        public Tile DeadEndCorridor => _deadEndCorridor;
        public bool CombineDungeonMeshes => _combineDungeonMeshes;
        public bool DrawWall => _drawWall;
        public bool EnableFogOfWar => _enableFogOfWar;
        public bool EnableVerticalFog => _enableVerticalFog;
        public int RoomCount => _roomCount;
        public int MinXRoomSize => _minXRoomSize;
        public int MaxXRoomSize => _maxXRoomSize;
        public int MinZRoomSize => _minZRoomSize;
        public int MaxZRoomSize => _maxZRoomSize;
        public CullSystemDependencyManager CullSystemPrefab => _cullSystem;
        public FOWSystem FOWSystemPrefab => _fowSystem;
        public GameObject VerticalFog => _verticalFog;
        public bool EnableCulling => _enableCulling;
        public bool DrawRoom => _drawRoom;
        public Tile FloorRoom => _floorRoom;
        public Tile WallRoom => _wallRoom;
        public Tile CeilingRoom => _ceilingRoom;
        public Tile DoorRoom => _doorRoom;
        public float FogX => _fogScaleX;
        public float FogY => _fogScaleY;
        public float FogZ => _fogScaleZ;
        public float FogHeight => _fogHeight;
    }
}