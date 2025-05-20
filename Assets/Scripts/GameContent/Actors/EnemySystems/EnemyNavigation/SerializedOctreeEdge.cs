using System.Collections.Generic;

namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    [System.Serializable]
    public class SerializedOctreeEdge
    {
        #region constructors
        
        public SerializedOctreeEdge(Edge edge)
        {
            a = edge.a.id;
            b = edge.b.id;
            depth = edge.depth;
            aDepth = edge.aDepth;
            bDepth = edge.bDepth;
        }
        
        #endregion

        #region methodes

        public static RunTimePathEdge CreateRunTimePathEdge(SerializedOctreeEdge current, List<RunTimePathNode> nodes)
        {
            return new RunTimePathEdge(current.a, current.b, nodes, current.depth, current.aDepth, current.bDepth);
        }

        public static RunTimePathEdge CreateRunTimePathEdge(RunTimePathNode a, RunTimePathNode b, int depth, int aDepth, int bDepth)
        {
            return new RunTimePathEdge(a, b, depth, aDepth, bDepth);
        }

        #endregion
        
        #region fields
        
        public int a;
        
        public int aDepth;

        public int b;
        
        public int bDepth;
        
        public int depth;
        
        #endregion
    }

    public class RunTimePathEdge
    {
        #region constructors

        public RunTimePathEdge(int a, int b, List<RunTimePathNode> nodes, int depth, int aDepth, int bDepth)
        {
            this.a = nodes[a];
            this.b = nodes[b];
            this.aDepth = aDepth;
            this.bDepth = bDepth;
            this.depth = depth;
        }

        public RunTimePathEdge(RunTimePathNode a, RunTimePathNode b, int depth, int aDepth, int bDepth)
        {
            this.a = a;
            this.b = b;
            this.aDepth = aDepth;
            this.bDepth = bDepth;
            this.depth = depth;
        }

        #endregion
        
        #region fields

        public readonly RunTimePathNode a;
        
        public readonly int aDepth;
        
        public readonly RunTimePathNode b;
        
        public readonly int bDepth;

        public readonly int depth;

        #endregion
    }
}