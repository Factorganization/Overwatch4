using System.Collections.Generic;
using System.Linq;
using GameContent.Actors.ActorData;
using GameContent.Actors.EnemySystems.EnemyNavigation;
using UnityEngine;
using UnityEngine.AI;

namespace GameContent.Actors.EnemySystems.Seekers
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Hound : Actor
    {
        #region methodes

        public override void Init(Transform player)
        {
            IsActive = true;
            base.Init(player);
            _navMeshAgent = GetComponent<NavMeshAgent>();

            _currentNode = GetClosestNode(transform.position);
            GetRandomDestination();
        }

        public override void OnUpdate()
        {
            if (navSpaceRtm is null)
                return;

            if (_currentPath is null)
                return;
            
            if (_currentPath.Count == 0 || _currentWayPointId >= _currentPath.Count)
            {
                GetRandomDestination();
                return;
            }

            if (Vector3.Distance(_currentPath[_currentWayPointId].position, transform.position) <= Accuracy)
                _currentWayPointId++;

            if (_currentWayPointId < _currentPath.Count)
            {
                _currentNode = _currentPath[_currentWayPointId];
                _targetPosition = _currentNode.position;
                
                var dir = (_targetPosition - transform.position).normalized;
                
                transform.position += dir * (Time.fixedDeltaTime * Speed);
                //transform.Translate(0, 0, _speed * Time.deltaTime);
            }
            else
            {
                GetRandomDestination();
            }
            
            /*_atkTimer += Time.deltaTime;
            
            if (Vector3.Distance(transform.position, playerTransform.position) < 10 && Vector3.Distance(transform.position, SuspicionManager.Manager.StartDebugPos) > 2)
            {
                _navMeshAgent.destination = playerTransform.position;
                SuspicionManager.Manager.DetectionTime += 1;
            }
            
            if (Vector3.Distance(transform.position, playerTransform.position) < 2.5f)
            {
                if (_atkTimer > 2 && SuspicionManager.Manager.IsTracking)
                {
                    _atkTimer = 0;
                    SuspicionManager.Manager.PlayerHealth.TakeDamage(10);
                }
                _navMeshAgent.isStopped = true;
            }
            else
                _navMeshAgent.isStopped = false;

            if (Vector3.Distance(transform.position, playerTransform.position) > 10 && !SuspicionManager.Manager.IsTracking)
            {
                _navMeshAgent.destination = SuspicionManager.Manager.StartDebugPos;
            }*/
        }

        public override void OnFixedUpdate()
        {
        }

        public void SetTargetPosition(Vector3 pos)
        {
            _navMeshAgent.destination = pos;
        }

        #region custom path Find

        private RunTimePathNode GetClosestNode(Vector3 pos)
        {
            //return navSpace.Octree.FindClosestNode(pos);

            var d = float.MaxValue;
            float td;
            RunTimePathNode closest = null;
            
            foreach (var rpn in navSpaceRtm.RunTimePathNodes)
            {
                td = Vector3.Distance(rpn.position, pos);

                if (td < d)
                {
                    closest = rpn;
                    d = td;
                }
            }

            return closest;
        }
        
        //TODO virer cee truc apres les tests
        private void GetRandomDestination()
        {
            var closestNode = GetClosestNode(transform.position);
            var dest = navSpaceRtm.RunTimePathNodes.ElementAt(Random.Range(0, navSpaceRtm.RunTimePathNodes.Count));
            _currentPath = PathFinder.FindPath(closestNode, dest);
            
            _currentWayPointId = 0;
        }
        
        #endregion

        private void OnDrawGizmos()
        {
            if (navSpaceRtm is null || _currentPath is null)
                return;
            
            if (_currentPath.Count == 0)
                return;
            
            Gizmos.color = Color.Lerp(Color.red, Color.yellow, 0.5f);
            
            Gizmos.DrawWireSphere(_currentPath[0].position, 0.7f);
            Gizmos.DrawWireSphere(_currentPath[^1].position, 0.7f);

            for (var i = 0; i < _currentPath.Count; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(_currentPath[i].position, 0.4f);

                if (i < _currentPath.Count - 1)
                {
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawLine(_currentPath[i].position, _currentPath[i + 1].position);
                }
            }
        }
        
        #endregion
        
        #region fields

        [SerializeField] private HoundData houndData;
        
        private NavMeshAgent _navMeshAgent;

        private Vector3 _currentTargetPosition;

        private float _atkTimer;

        #region custom nav volume

        [SerializeField] private NavSpaceRunTimeManager navSpaceRtm;

        private List<RunTimePathNode> _currentPath;

        private const float Speed = 5f;

        private const float Accuracy = 1f;

        //private float _turnSpeed = 5f; //used if graph rotation

        private int _currentWayPointId;
        
        private RunTimePathNode _currentNode;
        
        private Vector3 _targetPosition;

        #endregion

        #endregion
    }
}
