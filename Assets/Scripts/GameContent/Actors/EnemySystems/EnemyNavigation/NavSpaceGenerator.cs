using System.IO;
using UnityEditor;
using UnityEngine;

namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    public class NavSpaceGenerator : MonoBehaviour
    {
        #region methodes

        public void Bake()
        {
            navSpaceData.subData.Clear();
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
            
            if (Directory.Exists(sdp))
                Directory.Delete(sdp, true);
            AssetDatabase.Refresh();
            Directory.CreateDirectory(sdp);
            navSpaceData.subDataPath = sdp;
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<NavSpaceSubData>(), sdp + "\\sd0" + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.ImportAsset(sdp + "\\sd0" + ".asset", ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
            
            var z = AssetDatabase.LoadAssetAtPath(sdp + "\\sd0" + ".asset", typeof(NavSpaceSubData)) as NavSpaceSubData;
            navSpaceData.AddSubData(z);
            
            _navGraph = new NavGraph();
            _octree = new Octree(transform, worldObjs, minNodeSize, _navGraph, bakeBlockingLayer, navSpaceData);
        }

        private void Bknozddofknz()
        {
            /*var e = File.Open(Application.dataPath + "/Resources/bhjnbhjn.bytes", FileMode.CreateNew);
            var b = new BinaryWriter(e);
            b.Write("k,k,l,kl");
            e.Close();
            b.Dispose();
            b.Dispose();*/
            
            var tp = AssetDatabase.GetAssetPath(navSpaceData);
            var s = tp.Split('/');
            var ns = "";
            for (var i = 0; i < s.Length - 1; i++)
            {
                ns += s[i];
                ns += '\\';
            }
            var sdp = ns + navSpaceData.name + "Subs";
            
            
            var f = new FileInfo(sdp + "\\sd0" + ".asset").Length;
            Debug.Log(f);
        }
        
        #endregion
        
        #region fields
        
        [SerializeField] private NavSpaceData navSpaceData;
        
        [SerializeField] private Collider[] worldObjs;
        
        [SerializeField] private float minNodeSize;
        
        [SerializeField] private LayerMask bakeBlockingLayer;
        
        private NavGraph _navGraph;
        
        private Octree _octree;
        
        #endregion
    }
}