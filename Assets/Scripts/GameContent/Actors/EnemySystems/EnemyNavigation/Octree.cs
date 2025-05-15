using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    public class Octree
    {
        #region constructors

        public Octree(Transform parent, Collider[] worldObjs, float minNodeSize, NavGraph navGraph, LayerMask bakeLayer, NavSpaceData navSpaceData)
        {
            _navGraph = navGraph;
            _bakeLayer = bakeLayer;
            _navSpaceData = navSpaceData;
            
            CalculateBounds(parent, worldObjs);
            CreateTree(worldObjs, minNodeSize);
            
            GetEmptyLeaves(_root);
            GetEdges();

            BakeData();
        }

        #endregion

        #region methodes

        private void CreateTree(Collider[] worldObjs, float minNodeSize)
        {
            _root = new OctreeNode(_bounds, minNodeSize);

            foreach (var obj in worldObjs)
            {
                _root.Divide(obj);
            }
        }
        
        private void CalculateBounds(Transform parent, Collider[] worldObjs)
        {
            _bounds.center = parent.position;
            
            foreach (var worldObj in worldObjs)
                _bounds.Encapsulate(worldObj.bounds);
            
            var size = Vector3.one * Mathf.Max(_bounds.size.x, _bounds.size.y, _bounds.size.z) * 0.6f;
            _bounds.SetMinMax(_bounds.center - size, _bounds.center + size);
        }

        //public OctreeNode FindClosestNode(Vector3 position) => FindClosestNode(_root, position);
        /*public OctreeNode FindClosestNode(OctreeNode node, Vector3 position)
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
        }*/
        
        private void GetEmptyLeaves(OctreeNode node)
        {
            if (node.IsLeaf && node.objs.Count == 0)
            {
                _emptyLeaves.Add(node);
                _navGraph.AddNode(node);
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
                    _navGraph.AddEdge(node.children[i], node.children[j]);
                }
            }
        }

        private void GetEdges()
        {
            foreach (var el in _emptyLeaves)
            {
                foreach (var ol in _emptyLeaves)
                {
                    var ray = new Ray(el.bounds.center, ol.bounds.center - el.bounds.center);
                    var cast = Physics.Raycast(ray, Vector3.Distance(el.bounds.center, ol.bounds.center), _bakeLayer);
                    
                    if (!cast)
                        _navGraph.AddEdge(el, ol);
                    
                    /*if (el.bounds.Intersects(ol.bounds)) //TODO conditions
                    {
                        graph.AddEdge(el, ol);
                    }*/
                }
            }
        }

        private void BakeData()
        {
            foreach (var n in _navGraph.nodes.Values)
                _navSpaceData.AddNode(new SerializedOctreeNode(n));

            _navSpaceData.nodes.Sort(CompareSerializedNodes);
            
            foreach (var e in _navGraph.edges)
                _navSpaceData.AddEdge(new SerializedOctreeEdge(e));
        }

        #endregion
        
        #region fields

        private OctreeNode _root;

        private Bounds _bounds;

        private readonly NavGraph _navGraph;
        
        private readonly List<OctreeNode> _emptyLeaves = new();
        
        private readonly LayerMask _bakeLayer;

        private readonly NavSpaceData _navSpaceData;

        private static readonly Comparison<SerializedOctreeNode> CompareSerializedNodes =
            (a, b) => (int)Mathf.Sign(a.id - b.id);

        #endregion
    }
}