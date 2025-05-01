using GameContent.Actors.ActorData;
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
        }

        public override void OnUpdate()
        {
            _atkTimer += Time.deltaTime;

            Debug.Log(Vector3.Distance(transform.position, playerTransform.position));
            
            if (Vector3.Distance(transform.position, playerTransform.position) < 10)
            {
                _navMeshAgent.destination = playerTransform.position;
                SuspicionManager.Manager.DetectionTime += 1;
            }
            
            if (Vector3.Distance(transform.position, playerTransform.position) < 2.5f)
            {
                if (_atkTimer > 2)
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
        
        #endregion
        
        #region fields

        [SerializeField] private HoundData houndData;
        
        private NavMeshAgent _navMeshAgent;

        private Vector3 _currentTargetPosition;

        private float _atkTimer;
        
        #endregion
    }
}