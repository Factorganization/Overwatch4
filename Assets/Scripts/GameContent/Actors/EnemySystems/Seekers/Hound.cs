using System.Linq;
using GameContent.Actors.ActorData;
using GameContent.Actors.EnemySystems.EnemyNavigation;
using GameContent.Management;
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

            /*_navSpace = navSpace.NavSpaceData;
            //_currentNode = GetClosestNode(transform.position);
            GetRandomDestination();*/
        }

        public override void OnUpdate()
        {
            /*if (_graph is null)
                return;

            if (_graph.PathLength == 0 || _currentWayPointId >= _graph.PathLength)
            {
                GetRandomDestination();
                return;
            }

            if (Vector3.Distance(_graph[_currentWayPointId].bounds.center, transform.position) <= _accuracy)
                _currentWayPointId++;

            if (_currentWayPointId < _graph.PathLength)
            {
                _currentNode = _graph[_currentWayPointId];
                _targetPosition = _currentNode.bounds.center;
                
                var dir = (_targetPosition - transform.position).normalized;
                
                transform.position += dir * (Time.fixedDeltaTime * 5);
                //transform.Translate(0, 0, _speed * Time.deltaTime);
            }
            else
            {
                GetRandomDestination();
            }*/
            
            _atkTimer += Time.deltaTime;
            
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
            }
        }

        public override void OnFixedUpdate()
        {
        }

        public void SetTargetPosition(Vector3 pos)
        {
            _navMeshAgent.destination = pos;
        }

        #region custom path Find

        /*private OctreeNode GetClosestNode(Vector3 pos)
        {
            return navSpace.Octree.FindClosestNode(pos);
        }*/
        
        //TODO virer cee truc apres les tests
        /*private void GetRandomDestination()
        {
            OctreeNode dest;
            do
            {
                dest = navSpace.NavSpaceData.nodes.ElementAt(Random.Range(0, _navSpace.nodes.Count)).position;
            } while (!_navSpace.AStar(_currentNode, dest));

            _currentWayPointId = 0;
        }*/
        
        #endregion

        /*private void OnDrawGizmos()
        {
            if (_navSpace is null || _navSpace.PathLength == 0)
                return;
            
            Gizmos.color = Color.Lerp(Color.red, Color.yellow, 0.5f);
            
            Gizmos.DrawWireSphere(_navSpace[0].bounds.center, 0.7f);
            Gizmos.DrawWireSphere(_navSpace[_navSpace.PathLength - 1].bounds.center, 0.7f);

            for (var i = 0; i < _navSpace.PathLength; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(_navSpace[i].bounds.center, 0.4f);

                if (i < _navSpace.PathLength - 1)
                {
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawLine(_navSpace[i].bounds.center, _navSpace[i + 1].bounds.center);
                }
            }
        }*/
        
        #endregion
        
        #region fields

        [SerializeField] private HoundData houndData;
        
        private NavMeshAgent _navMeshAgent;

        private Vector3 _currentTargetPosition;

        private float _atkTimer;

        #region custom nav volume

        [SerializeField] private NavSpaceGenerator navSpace;

        private NavSpaceData _navSpace;
        
        private float _speed = 5f;

        private float _accuracy = 1f;

        private float _turnSpeed = 5f;

        private int _currentWayPointId;
        
        private OctreeNode _currentNode;
        
        private Vector3 _targetPosition;

        #endregion

        #endregion
    }
}
