using UnityEngine;

namespace NPP.DE.Core.Dungeon.Generator
{
    public class Tile : MonoBehaviour
    {

        [SerializeField]
        private GameObject _tileMesh;

        public GameObject TileMesh => _tileMesh;

        #region Floor Helper

        private BoxCollider _collider;

        public BoxCollider Collider
        {
            get
            {
                if (_collider == null)
                    _collider = GetComponent<BoxCollider>();
                return _collider;
            }
        }


        public float ScaleX
        {
            get
            {
                return Collider.size.x;
            }
        }

        public float ScaleY
        {
            get
            {
                return Collider.size.y;
            }
        }

        public float ScaleZ
        {
            get
            {
                return Collider.size.z;
            }
        }

        #endregion
    }
}