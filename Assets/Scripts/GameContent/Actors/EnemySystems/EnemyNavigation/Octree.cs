using System.Collections.Generic;
using UnityEngine;

namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    [System.Serializable]
    public class Octree
    {
        #region constructors

        public Octree(Transform parent, Collider[] worldObjs, float minNodeSize, Graph graph)
        {
            this.graph = graph;
            
            CalculateBounds(parent, worldObjs);
            CreateTree(worldObjs, minNodeSize);
            
            GetEmptyLeaves(root);
            GetEdges();
        }

        #endregion

        #region methodes

        private void CreateTree(Collider[] worldObjs, float minNodeSize)
        {
            root = new OctreeNode(bounds, minNodeSize);

            foreach (var obj in worldObjs)
            {
                root.Divide(obj);
            }
        }
        
        private void CalculateBounds(Transform parent, Collider[] worldObjs)
        {
            bounds.center = parent.position;
            
            foreach (var worldObj in worldObjs)
                bounds.Encapsulate(worldObj.bounds);
            
            var size = Vector3.one * Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z) * 0.6f;
            bounds.SetMinMax(bounds.center - size, bounds.center + size);
        }

        public OctreeNode FindClosestNode(Vector3 position) => FindClosestNode(root, position);
        public OctreeNode FindClosestNode(OctreeNode node, Vector3 position)
        {
            OctreeNode found = null;

            for (var i = 0; i < node.children.Length; i++)
            {
                if (node.children[i].bounds.Contains(position))
                {
                    if (node.children[i].IsLeaf)
                    {
                        found = node.children[i];
                        break;
                    }

                    found = FindClosestNode(node.children[i], position);
                }
            }

            return found;
        }
        
        private void GetEmptyLeaves(OctreeNode node)
        {
            if (node.IsLeaf && node._objs.Count == 0)
            {
                emptyLeaves.Add(node);
                graph.AddNode(node);
                return;
            }
            
            if (node.children is null)
                return;

            foreach (var o in node.children)
                GetEmptyLeaves(o);

            for (var i = 0; i < node.children.Length; i++)
            {
                for (var j = i + 1; j < node.children.Length; j++)
                {
                    graph.AddEdge(node.children[i], node.children[j]);
                }
            }
        }

        private void GetEdges()
        {
            foreach (var el in emptyLeaves)
            {
                foreach (var ol in emptyLeaves)
                {
                    if (el.bounds.Intersects(ol.bounds))
                    {
                        graph.AddEdge(el, ol);
                    }
                }
            }
        }

        #endregion
        
        #region fields

        public OctreeNode root;
        
        public Bounds bounds;
        
        public Graph graph;
        
        private List<OctreeNode> emptyLeaves = new();

        #endregion
    }
}