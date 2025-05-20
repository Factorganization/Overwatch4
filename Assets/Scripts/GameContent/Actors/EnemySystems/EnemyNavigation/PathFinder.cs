using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    public static class PathFinder
    {
        public static List<RunTimePathNode> FindPath(RunTimePathNode start, RunTimePathNode end)
        {
            PathList.Clear();
            
            if (start is null || end is null)
            {
                Debug.LogError("No start or end node found");
                return new List<RunTimePathNode>();
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
                    return new List<RunTimePathNode>();
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
                    var n = e.a.id == current.id ? e.b : e.a;
                    
                    if (ClosedList.Contains(n))
                        continue;
                    
                    var tempG = current.g + Heuristic(current, n);

                    if ((tempG >= n.g && OpenList.Contains(n)) || !n.isAvailable)
                        continue;
                    
                    n.g = tempG;
                    n.h = Heuristic(n, end);
                    n.f = n.g + n.h;
                    n.from = current;
                    OpenList.Add(n);
                }
            }
            
            Debug.Log("No path found");
            return new List<RunTimePathNode>();
        }

        private static float Heuristic(RunTimePathNode a, RunTimePathNode b) => (a.position - b.position).sqrMagnitude;

        private static List<RunTimePathNode> GetFullPath(RunTimePathNode current)
        {
            while (current is not null)
            {
                PathList.Add(current);
                current = current.from;
            }
            
            PathList.Reverse();
            return PathList;
        }

        private static readonly List<RunTimePathNode> PathList = new();
        
        private static readonly List<RunTimePathNode> OpenList = new();
        
        private static readonly List<RunTimePathNode> ClosedList = new();
        
        private static readonly Comparison<RunTimePathNode> NodeComparer = (a, b) => (int)Mathf.Sign(a.f - b.f);
    }
}