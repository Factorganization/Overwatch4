using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    [RequireComponent(typeof(Collider))]
    public class NavSpaceObstacle : MonoBehaviour
    {
        #region methodes

        private void Start()
        {
            _lockedPositions = new List<Vector3>();
            _collider = GetComponent<Collider>();
            NavSpaceRunTimeManager.Manager.Obstacles.Add(this);
        }

        private void Update()
        {
            if (_lastPos != transform.position)
            {
                _lockedPositions = LockNodes();
            }
            
            _lastPos = transform.position;
        }

        private List<Vector3> LockNodes()
        {
            var lockedPositions = new List<Vector3>();
            
            foreach (var n in NavSpaceRunTimeManager.Manager.RunTimePathNodes)
            {
                var b = _collider.bounds.Contains(n.position);
                if (b)
                {
                    n.isAvailable = false;
                    lockedPositions.Add(n.position);
                }

                if (_lockedPositions.Contains(n.position) && !b)
                {
                    n.isAvailable = true;
                }
            }
            return lockedPositions;
        }
        
        private async UniTask<List<Vector3>> LockNodesAsync()
        {
            var lockedPositions = new List<Vector3>();
            
            foreach (var n in NavSpaceRunTimeManager.Manager.RunTimePathNodes)
            {
                var b = _collider.bounds.Contains(n.position);
                if (b)
                {
                    n.isAvailable = false;
                    lockedPositions.Add(n.position);
                }

                if (_lockedPositions.Contains(n.position) && !b)
                {
                    n.isAvailable = true;
                }
            }
            
            await UniTask.Yield(); // certes mais bon
            return lockedPositions;
        }

        #endregion
        
        #region fields

        private Collider _collider;
        
        private List<Vector3> _lockedPositions;

        private Vector3 _lastPos;

        #endregion
    }
}