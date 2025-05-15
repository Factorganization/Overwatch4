using System.Collections.Generic;
using UnityEngine;

namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    [System.Serializable]
    public class SerializedOctreeNode
    {
        #region constructors
        
        public SerializedOctreeNode(Node node)
        {
            id = node.id;
            position = node.octreeNode.bounds.center;
            bounds = node.octreeNode.bounds;
            
            edges = new List<SerializedOctreeEdge>();
            foreach (var e in node.edges)
                edges.Add(new SerializedOctreeEdge(e));
        }
        
        #endregion

        #region methodes
        
        public static TempPathNode CreateTempNode(SerializedOctreeNode current)
        {
            return new TempPathNode(current.id, current.position, current.edges);
        }
        
        #endregion
        
        #region fields
        
        public int id;

        public Vector3 position;

        public List<SerializedOctreeEdge> edges;

        public Bounds bounds;

        #endregion
    }

    public class TempPathNode
    {
        #region constructors
        
        public TempPathNode(int id, Vector3 position, List<SerializedOctreeEdge> edges)
        {
            this.id = id;
            this.position = position;
            this.edges = edges;
        }
        
        #endregion
        
        #region fields

        public readonly int id;
        
        public readonly List<SerializedOctreeEdge> edges;

        public Vector3 position;

        public TempPathNode from;

        public float g;

        public float h;

        public float f;
        
        #endregion
    }
}