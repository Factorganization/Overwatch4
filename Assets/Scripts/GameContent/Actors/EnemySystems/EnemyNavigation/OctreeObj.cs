using UnityEngine;

namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    //[System.Serializable]
    public class OctreeObj
    {
        #region constructors

        public OctreeObj(Collider obj)
        {
            bounds = obj.bounds;
        }

        #endregion

        #region methodes

        public bool Intersects(Bounds other) => bounds.Intersects(other);

        #endregion
        
        #region fields

        [SerializeField] private Bounds bounds;

        #endregion
    }
}