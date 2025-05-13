using System.Collections.Generic;
using UnityEngine;

namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    [System.Serializable]
    public class OctreeNode
    {
         #region properties

        public bool IsLeaf => children is null;

        #endregion

        #region constructors

        public OctreeNode(Bounds bounds, float minNodeSize)
        {
            Id = nextId++;
            
            this.bounds = bounds; 
            this.minNodeSize = minNodeSize;

            var newSize = bounds.size * 0.5f;
            var centerOffset = bounds.size * 0.25f;
            var parentCenter = bounds.center;

            for (var i = 0; i < 8; i++)
            {
                var childCenter = parentCenter;
                childCenter.x += centerOffset.x * ((i & 1) == 0 ? -1 : 1);
                childCenter.y += centerOffset.y * ((i & 2) == 0 ? -1 : 1);
                childCenter.z += centerOffset.z * ((i & 4) == 0 ? -1 : 1);
                _childBounds[i] = new Bounds(childCenter, newSize);
            }
        }

        #endregion
        
        #region methodes

        public void Divide(Collider obj) => Divide(new OctreeObj(obj));

        private void Divide(OctreeObj obj)
        {
            if (bounds.size.x <= minNodeSize)
            {
                _objs.Add(obj);
                return;
            }

            children ??= new OctreeNode[8];
            var intersectChild = false;

            for (var i = 0; i < 8; i++)
            {
                children[i] ??= new OctreeNode(_childBounds[i], minNodeSize);

                if (!obj.Intersects(_childBounds[i]))
                    continue;
                
                children[i].Divide(obj);
                intersectChild = true;
            }
            
            if (intersectChild)
                _objs.Add(obj);
        }
        
        public void DrawNode()
        {
            Gizmos.color = Color.Lerp(Color.blue, Color.green, minNodeSize / bounds.size.x);
            Gizmos.DrawWireCube(bounds.center, bounds.size);

            /* cube rouge outline obj intersectés, pas fou utile :/
            foreach (var o in _objs)
            {
                if (o.Intersects(bounds))
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawCube(bounds.center, bounds.size);
                }
            }*/
            
            if (children is null)
                return;
            
            foreach (var child in children)
                child?.DrawNode();
        }

        #endregion
        
        #region fields

        private static int nextId;
        
        public readonly int Id;
        
        public List<OctreeObj> _objs = new();

        public Bounds bounds;
        
        private Bounds[] _childBounds = new Bounds[8];

        public OctreeNode[] children;

        [SerializeField] [HideInInspector] public float minNodeSize;

        #endregion
    }
}