using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    public static class PathFinder
    {
        public static List<TempPathNode> AStar(SerializedOctreeNode startNode, SerializedOctreeNode endNode, NavSpaceData navSpaceData)
        {
            PathList.Clear();
            var start = SerializedOctreeNode.CreateTempNode(startNode);
            var end = SerializedOctreeNode.CreateTempNode(endNode);

            if (navSpaceData is null)
            {
                Debug.LogError("NavSpaceData is null");
                return new List<TempPathNode>();
            }
            
            if (start is null || end is null)
            {
                Debug.LogError("No start or end node found");
                return new List<TempPathNode>();
            }

            OpenList.Clear();
            ClosedList.Clear();
            
            var iterationCount = 0;

            start.g = 0;
            start.h = Heuristic(start, end);
            start.f = start.g + start.h;
            start.from = null;
            OpenList.Add(start);

            while (OpenList.Count > 0)
            {
                if (++iterationCount > GameConstants.MaxPathFindIteration)
                {
                    Debug.LogError("Pathfind iteration exceeded");
                    return new List<TempPathNode>();
                }

                OpenList.Sort(NodeComparer);
                var current = OpenList[0];
                OpenList.Remove(current);

                if (current.id == end.id)
                {
                    return GetFullPath(current);
                }
                
                ClosedList.Add(current);
                
                foreach (var e in current.edges)
                {
                    var n = e.a == current.id
                        ? SerializedOctreeNode.CreateTempNode(navSpaceData.nodes[e.b])
                        : SerializedOctreeNode.CreateTempNode(navSpaceData.nodes[e.a]);
                    
                    if (ClosedList.Contains(n))
                        continue;
                    
                    var tempG = current.g + Heuristic(current, n);

                    if (tempG >= n.g && OpenList.Contains(n))
                        continue;
                    
                    n.g = tempG;
                    n.h = Heuristic(n, end);
                    n.f = n.g + n.h;
                    n.from = current;
                    OpenList.Add(n);
                }
            }
            
            Debug.Log("No path found");
            return new List<TempPathNode>();
        }

        private static float Heuristic(TempPathNode a, TempPathNode b) => (a.position - b.position).sqrMagnitude;

        private static List<TempPathNode> GetFullPath(TempPathNode current)
        {
            while (current is not null)
            {
                PathList.Add(current);
                current = current.from;
            }
            
            PathList.Reverse();
            return PathList;
        }

        private static readonly List<TempPathNode> PathList = new();
        
        private static readonly List<TempPathNode> OpenList = new();
        
        private static readonly List<TempPathNode> ClosedList = new();
        
        private static readonly Comparison<TempPathNode> NodeComparer = (a, b) => (int)Mathf.Sign(a.f - b.f);
    }
}