using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    public class NavGraph
    {
        #region constructors

        public NavGraph()
        {
            Node.ResetId();
        }

        #endregion
        
        #region methodes

        #region pathFind
        
        public bool AStar(OctreeNode startNode, OctreeNode endNode)
        {
            _pathList.Clear();
            var start = FindNode(startNode);
            var end = FindNode(endNode);

            if (start is null || end is null)
            {
                Debug.LogError("No start or end node found");
                return false;
            }

            _openList.Clear();
            _closedList.Clear();
            
            var iterationCount = 0;

            start.g = 0;
            start.h = Heuristic(start, end);
            start.f = start.g + start.h;
            start.from = null;
            _openList.Add(start);

            while (_openList.Count > 0)
            {
                if (++iterationCount > GameConstants.MaxPathFindIteration)
                {
                    Debug.LogError("Pathfind iteration exceeded");
                    return false;
                }

                _openList.Sort(NodeComparer);
                var current = _openList[0];
                _openList.Remove(current);

                if (current.Equals(end))
                {
                    GetFullPath(current);
                    return true;
                }
                
                _closedList.Add(current);
                
                foreach (var e in current.edges)
                {
                    var n = Equals(e.a, current) ? e.b : e.a;
                    
                    if (_closedList.Contains(n))
                        continue;
                    
                    var tempG = current.g + Heuristic(current, n);

                    if (tempG >= n.g && _openList.Contains(n))
                        continue;
                    
                    n.g = tempG;
                    n.h = Heuristic(n, end);
                    n.f = n.g + n.h;
                    n.from = current;
                    _openList.Add(n);
                }
            }
            
            Debug.Log("No path found");
            return false;
        }

        private float Heuristic(Node a, Node b) => (a.octreeNode.bounds.center - b.octreeNode.bounds.center).sqrMagnitude;

        private void GetFullPath(Node current)
        {
            while (current is not null)
            {
                _pathList.Add(current);
                current = current.from;
            }
            
            _pathList.Reverse();
        }
        
        #endregion
        
        #region graph Gen
        
        public void AddNode(OctreeNode node)
        {
            if (!nodes.ContainsKey(node))
            {
                nodes.Add(node, new Node(node, _currentDepth));
                _currentDepthContentCount++;
                if (_currentDepthContentCount >= DepthCountThreshold)
                {
                    _currentDepth++;
                    _currentDepthContentCount = 0;
                }
            }
        }

        public void AddEdge(OctreeNode a, OctreeNode b)
        {
            var nodeA = FindNode(a);
            var nodeB = FindNode(b);
            
            if (nodeA is null || nodeB is null)
                return;
            
            var edge = new Edge(nodeA, nodeB, _currentDepth);

            if (!edges.Add(edge))
                return;
            
            _currentDepthContentCount++;
            if (_currentDepthContentCount >= DepthCountThreshold)
            {
                _currentDepth++;
                _currentDepthContentCount = 0;
            }
            
            nodeA.edges.Add(edge);
            nodeB.edges.Add(edge);
        }
        
        private Node FindNode(OctreeNode octreeNode)
        {
            nodes.TryGetValue(octreeNode, out var node);
            return node;
        }
        
        #endregion

        #endregion
        
        #region fields
        
        public readonly Dictionary<OctreeNode, Node> nodes = new();
        
        public readonly HashSet<Edge> edges = new();

        private List<Node> _pathList = new();
        
        private List<Node> _openList = new();
        
        private List<Node> _closedList = new();

        private int _currentDepth;

        private int _currentDepthContentCount;
        
        private const int DepthCountThreshold = 500000;
        
        private static readonly Comparison<Node> NodeComparer = (a, b) => (int)Mathf.Sign(a.f - b.f);

        #endregion
    }

    public class Node
    {
        #region constructors

        public Node(OctreeNode ot, int depth)
        {
            id = nextId++;
            octreeNode = ot;
            this.depth = depth;
        }

        #endregion

        #region methodes

        public override bool Equals(object obj) => obj is Node other && id == other.id;
        
        public override int GetHashCode() => id.GetHashCode();
        
        public static void ResetId() => nextId = 0;

        #endregion
        
        #region fields

        private static int nextId;
        
        public readonly int id;
        
        public readonly List<Edge> edges = new();
        
        public readonly OctreeNode octreeNode;

        public Node from;

        public float f, g, h;

        public readonly int depth;

        #endregion
    }

    public class Edge
    {
        #region constructors

        public Edge(Node a, Node b, int depth)
        {
            this.a = a;
            this.b = b;
            this.depth = depth;
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
        
        public readonly int depth;
        
        #endregion
    }
}