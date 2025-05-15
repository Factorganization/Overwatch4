using UnityEngine;

namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    public class NavSpaceGenerator : MonoBehaviour
    {
        #region properties

        public Octree Octree => _octree;
        
        public NavGraph NavGraph => _navGraph;

        public NavSpaceData NavSpaceData => navSpaceData;

        #endregion
        
        #region methodes

        [ContextMenu("Generate Nav Space")]
        private void Bake()
        {
            navSpaceData.nodes.Clear();
            navSpaceData.edges.Clear();
            navSpaceData.minBoundSize = minNodeSize;
            
            _navGraph = new NavGraph();
            _octree = new Octree(transform, worldObjs, minNodeSize, _navGraph, bakeBlockingLayer, navSpaceData);
        }

        private void OnDrawGizmosSelected()
        {
            if (viewOptions.view3dNavSpace)
            {
                foreach (var n in navSpaceData.nodes)
                {
                    Gizmos.color = Color.Lerp(Color.blue, Color.green, navSpaceData.minBoundSize / n.bounds.size.x);
                    Gizmos.DrawWireCube(n.bounds.center, n.bounds.size);
                }
            }

            if (viewOptions.view3dNavPoints)
            {
                Gizmos.color = new Color(1, 0.35f, 0, 0.25f);
                foreach (var n in navSpaceData.nodes)
                {
                    Gizmos.DrawWireSphere(n.position, 0.3f);
                }
            }
            
            if (viewOptions.view3dNavPath && viewOptions.validateViewEdNavPath)
            {
                Gizmos.color = new Color(1, 0, 0, 0.25f);
                foreach (var e in navSpaceData.edges)
                {
                    Gizmos.DrawLine(navSpaceData.nodes[e.a].position, navSpaceData.nodes[e.b].position);
                }
            }
        }

        #endregion
        
        #region fields
        
        [SerializeField] private NavSpaceData navSpaceData;
        
        [SerializeField] private Collider[] worldObjs;
        
        [SerializeField] private float minNodeSize;

        [SerializeField] private ViewOptions viewOptions;
        
        [SerializeField] private LayerMask bakeBlockingLayer;
        
        private NavGraph _navGraph;
        
        private Octree _octree;
        
        #endregion
    }

    [System.Serializable]
    public class ViewOptions
    {
        #region fields
        
        public bool view3dNavSpace;
        
        [Header("A vos risques et perils celui ci")]
        public bool view3dNavPath;
        
        [Header("Vraiment va falloir relancer Unity")]
        public bool validateViewEdNavPath;
        
        [Space(10)]
        
        public bool view3dNavPoints;
        
        #endregion
    }
}