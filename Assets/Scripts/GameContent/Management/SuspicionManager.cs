using UnityEngine;

namespace GameContent.Management
{
    public class SuspicionManager : MonoBehaviour
    {
        #region properties
        
        public static SuspicionManager Manager { get; private set; }
        
        public bool IsInvestigating { get; private set; }
        
        #endregion

        #region methodes

        private void Awake()
        {
            if (Manager is not null)
            {
                Destroy(gameObject);
                return;
            }
            
            Manager = this;
        }
        
        private void Start()
        {
            _suspicionLevel = 0;
        }

        private void Update()
        {
            _suspicionDecreaseTimer -= Time.deltaTime;

            if (_suspicionLevel > 0 && _suspicionDecreaseTimer < 0)
            {
                RemoveSuspicion(suspicionDecreasePerSecond);
                _suspicionDecreaseTimer = 1;
            }
        }

        public void AddSuspicion(float value) => _suspicionLevel += value;
        
        public void RemoveSuspicion(float value) => _suspicionLevel -= value;
        
        #endregion

        #region fields

        [SerializeField] private float investigationLevel;

        [SerializeField] private float suspicionDecreasePerSecond;
        
        private float _suspicionLevel; //Game Core

        private float _suspicionDecreaseTimer;
        
        #endregion
    }
}