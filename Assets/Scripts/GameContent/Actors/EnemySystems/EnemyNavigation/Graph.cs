using System.Collections.Generic;
using UnityEngine;

namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    [System.Serializable]
    public class Graph
    {
        #region methodes

        public void AddNode(OctreeNode node)
        {
            if (!nodes.ContainsKey(node))
                nodes.Add(node, new Node(node));
        }

        public void AddEdge(OctreeNode a, OctreeNode b)
        {
            var nodeA = FindNode(a);
            var nodeB = FindNode(b);
            
            if (nodeA is null || nodeB is null)
                return;
            
            var edge = new Edge(nodeA, nodeB);

            if (!edges.Add(edge))
                return;
            
            nodeA.edges.Add(edge);
            nodeB.edges.Add(edge);
        }
        
        private Node FindNode(OctreeNode octreeNode)
        {
            nodes.TryGetValue(octreeNode, out var node);
            return node;
        }

        public void DrawGraph()
        {
            Gizmos.color = new Color(1, 0, 0, 0.25f);

            foreach (var e in edges)
                Gizmos.DrawLine(e.a.octreeNode.bounds.center, e.b.octreeNode.bounds.center);
            
            foreach (var n in nodes.Values)
                Gizmos.DrawWireSphere(n.octreeNode.bounds.center, 0.5f);
        }

        #endregion
        
        #region fields
        
        public readonly Dictionary<OctreeNode, Node> nodes = new();
        
        public readonly HashSet<Edge> edges = new();

        private List<Node> pathList = new();

        #endregion
    }

    public class Node
    {
        #region constructors

        public Node(OctreeNode ot)
        {
            id = nextId++;
            octreeNode = ot;
        }

        #endregion

        #region methodes

        public override bool Equals(object obj) => obj is Node other && id == other.id;
        
        public override int GetHashCode() => id.GetHashCode();

        #endregion
        
        #region fields

        private static int nextId;
        
        public readonly int id;
        
        public List<Edge> edges = new();
        
        public OctreeNode octreeNode;

        public Node from;

        public float f, g, h;

        #endregion
    }

    public class Edge
    {
        #region constructors

        public Edge(Node a, Node b)
        {
            this.a = a;
            this.b = b;
        }

        #endregion

        #region methodes

        public override bool Equals(object obj)
        {
            return obj is Edge other && ((a == other.a && b == other.b) || (a == other.b && b == other.a));
        }
        
        public override int GetHashCode() => a.GetHashCode() ^ b.GetHashCode();

        #endregion
        
        #region fields
        
        public readonly Node a;
        
        public readonly Node b;
        
        #endregion
    }
}