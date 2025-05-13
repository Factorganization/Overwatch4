using UnityEngine;

namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    public class NavSpaceGenerator : MonoBehaviour
    {
        #region properties

        public Octree Octree => octree;

        #endregion
        
        #region methodes

        [ContextMenu("Generate Nav Space")]
        private void Awake()
        {
            octree = new Octree(transform, worldObjs, minNodeSize, navGraph);
        }

        private void OnDrawGizmos()
        {
            if (view3dNavSpace)
                octree?.root.DrawNode();
            
            if (view3dNavPath)
                octree?.graph.DrawGraph();
        }

        #endregion
        
        #region fields
        
        [SerializeField] private Collider[] worldObjs;
        
        [SerializeField] private float minNodeSize;

        [SerializeField] private bool view3dNavSpace;
        
        [SerializeField] private bool view3dNavPath;
        
        public readonly Graph navGraph = new();
        
        [SerializeField] [HideInInspector] private Octree octree;
        
        #endregion
    }
}