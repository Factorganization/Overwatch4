using System.IO;
using UnityEditor;
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
            navSpaceData.minBoundSize = minNodeSize;
            
            var tp = AssetDatabase.GetAssetPath(navSpaceData);
            var s = tp.Split('/');
            var ns = "";
            for (var i = 0; i < s.Length - 1; i++)
            {
                ns += s[i];
                ns += '\\';
            }
            var sdp = ns + navSpaceData.name + "Subs";
            Directory.CreateDirectory(sdp);
            navSpaceData.subDataPath = sdp;
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<NavSpaceSubData>(), sdp + "\\sd0" + ".asset");
            AssetDatabase.SaveAssets();
            
            var z = AssetDatabase.LoadAssetAtPath(sdp + "\\sd0" + ".asset", typeof(NavSpaceSubData)) as NavSpaceSubData;
            navSpaceData.AddSubData(z);
            
            _navGraph = new NavGraph();
            _octree = new Octree(transform, worldObjs, minNodeSize, _navGraph, bakeBlockingLayer, navSpaceData, z);
        }

        #endregion
        
        #region fields
        
        [SerializeField] private NavSpaceData navSpaceData;
        
        [SerializeField] private NavSpaceSubData navSpaceSubData;
        
        [SerializeField] private Collider[] worldObjs;
        
        [SerializeField] private float minNodeSize;
        
        [SerializeField] private LayerMask bakeBlockingLayer;
        
        private NavGraph _navGraph;
        
        private Octree _octree;
        
        #endregion
    }
}