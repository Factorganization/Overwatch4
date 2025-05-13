using UnityEngine;

namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    public class NavSpaceGenerator : MonoBehaviour
    {
        #region methodes

        private void Awake()
        {
            _octree = new Octree(transform, worldObjs, minNodeSize, navGraph);
        }

        private void OnDrawGizmos()
        {
            if (view3dNavSpace)
                _octree?.root.DrawNode();
            
            if (view3dNavPath)
                _octree?.graph.DrawGraph();
        }

        #endregion
        
        #region fields
        
        [SerializeField] private Collider[] worldObjs;
        
        [SerializeField] private float minNodeSize;

        [SerializeField] private bool view3dNavSpace;
        
        [SerializeField] private bool view3dNavPath;
        
        public readonly Graph navGraph = new();
        
        private Octree _octree;
        
        #endregion
    }
}