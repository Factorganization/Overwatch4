using GameContent.Actors.ActorData;
using UnityEngine;
using UnityEngine.AI;

namespace GameContent.Actors.EnemySystems.Seekers
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Drone : Actor
    {
        #region methodes

        public override void Init(Transform player)
        {
            base.Init(player);
            
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _currentWaypoint = waypoints[0].position;
        }
        
        public override void OnUpdate()
        {
        }

        public override void OnFixedUpdate()
        {
        }
        
        #endregion

        #region fields

        [SerializeField] private DroneData droneData;
        
        [SerializeField] private Transform[] waypoints;

        private Vector3 _currentWaypoint;
        
        private NavMeshAgent _navMeshAgent;
        
        private Vector3 _currentTargetPosition;
        
        #endregion
    }
}