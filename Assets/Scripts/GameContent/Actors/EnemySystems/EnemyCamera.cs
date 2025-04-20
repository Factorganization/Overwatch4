using System;
using GameContent.ActorViews.Player;
using UnityEngine;

namespace GameContent.Actors.EnemySystems
{
    public class EnemyCamera : Actor
    {
        #region properties

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                coneRenderer.material.color = !_isActive ? Color.clear : new Color(1, 1, 1, 0.2f);
            }
        }

        #endregion
        
        #region methodes

        public override void Init(Transform player)
        {
            playerTransform = player;
            _playerView = playerTransform.GetComponent<PlayerView>();
            IsActive = true;

            foreach (var c in cameraRotations)
                c.Init(gameObject);
        }

        public override void OnUpdate()
        {
            if (!IsActive)
                return;
            
            HandleCameraRotation();
            
            var s = HasPlayerInSight();
            switch (s)
            {
                case true when !_inSight:
                    _inSight = true;
                    coneRenderer.material.color = new Color(1, 0, 0, 0.2f);
                    _playerView.SightCount++;
                    break;
                case false when _inSight:
                    _inSight = false;
                    coneRenderer.material.color = new Color(1, 1, 1, 0.2f);
                    _playerView.SightCount--;
                    break;
            }
        }

        private bool HasPlayerInSight()
        {
            if (Vector3.Distance(transform.position, playerTransform.position) > range)
                return false;
            
            if (Vector3.Dot(transform.forward, (playerTransform.position - transform.position).normalized) < angle)
                return false;
            
            var r = Physics.Raycast(transform.position, playerTransform.position - transform.position, out var hit, range, collisionLayer);

            return r && hit.collider.gameObject.layer == LayerMask.NameToLayer("Player");
        }

        private void HandleCameraRotation()
        {
            foreach (var c in cameraRotations)
            {
                switch (c.rotationType)
                {
                    case RotationType.Continuous:
                        HandleContinuousRotation(c);
                        break;
                    
                    case RotationType.Step:
                        HandleStepRotation(c);
                        break;
                    
                    case RotationType.None:
                    default:
                        break;
                }
            }
        }

        private void HandleContinuousRotation(CameraRotation cameraRotation)
        {
            switch (cameraRotation.rotationAxis)
            {
                case RotationAxis.X:
                    transform.RotateAround(transform.position,
                        cameraRotation.rotationRef is RotationReferential.Local ? transform.right : Vector3.right,
                        cameraRotation.rotationSpeed * Time.deltaTime);
                    break;
                case RotationAxis.Y:
                    transform.RotateAround(transform.position,
                        cameraRotation.rotationRef is RotationReferential.Local ? transform.up : Vector3.up,
                        cameraRotation.rotationSpeed * Time.deltaTime);
                    break;
                case RotationAxis.Z:
                    transform.RotateAround(transform.position,
                        cameraRotation.rotationRef is RotationReferential.Local ? transform.forward : Vector3.forward,
                        cameraRotation.rotationSpeed * Time.deltaTime);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleStepRotation(CameraRotation cameraRotation)
        {
            if (cameraRotation.currentWaitTime > 0)
            {
                cameraRotation.currentWaitTime -= Time.deltaTime;
                return;
            }
            
            cameraRotation.angleRemains -= cameraRotation.rotationSpeed * Time.deltaTime * cameraRotation.currentSpeedSign;
            
            if (Mathf.Abs(cameraRotation.angleRemains) < Mathf.Abs(cameraRotation.rotationSpeed) * Time.deltaTime + GameConstants.FloatPointComparisonValue)
                OnSwitchRotationTarget(cameraRotation);
            
            switch (cameraRotation.rotationAxis)
            {
                case RotationAxis.X:
                    transform.RotateAround(transform.position,
                        cameraRotation.rotationRef is RotationReferential.Local ? transform.right : Vector3.right,
                        cameraRotation.rotationSpeed * Time.deltaTime * cameraRotation.currentSpeedSign);
                    break;
                case RotationAxis.Y:
                    transform.RotateAround(transform.position,
                        cameraRotation.rotationRef is RotationReferential.Local ? transform.up : Vector3.up,
                        cameraRotation.rotationSpeed * Time.deltaTime * cameraRotation.currentSpeedSign);
                    break;
                case RotationAxis.Z:
                    transform.RotateAround(transform.position,
                        cameraRotation.rotationRef is RotationReferential.Local ? transform.forward : Vector3.forward,
                        cameraRotation.rotationSpeed * Time.deltaTime * cameraRotation.currentSpeedSign);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void OnSwitchRotationTarget(CameraRotation cameraRotation)
        {
            cameraRotation.currentWaitTime = cameraRotation.additionalStepData.waitTime;
            cameraRotation.currentStep = (cameraRotation.currentStep + 1) % cameraRotation.additionalStepData.stepNumbers;

            if (cameraRotation.currentStep == 0)
            {
                cameraRotation.currentStep = 1;
                
                if (cameraRotation.additionalStepData.loopAround)
                {
                    cameraRotation.angleRemains = 360 - cameraRotation.additionalStepData.stepNumbers * cameraRotation.additionalStepData.angle;
                    return;
                }
                
                cameraRotation.angleRemains = -cameraRotation.additionalStepData.stepNumbers * cameraRotation.additionalStepData.angle;
                cameraRotation.currentSpeedSign = -1;
                return;
            }
            
            if (cameraRotation.currentSpeedSign < 0) //Pas joli mais marche
                cameraRotation.currentSpeedSign = 1;
            
            cameraRotation.angleRemains = cameraRotation.additionalStepData.angle;
        }
        
        #endregion

        #region fields

        [SerializeField] private MeshRenderer coneRenderer;
        
        [SerializeField] private CableLink cableLinkRef;

        [SerializeField] private float range;

        [Range(0f, 1f)]
        [SerializeField] private float angle;
        
        [SerializeField] private LayerMask collisionLayer;

        [SerializeField] private CameraRotation[] cameraRotations;
        
        private PlayerView _playerView;

        private bool _isActive;
        
        private bool _inSight;

        #endregion
    }

    [Serializable]
    internal class CameraRotation
    {
        #region methodes
        
        public void Init(GameObject actor)
        {
            angleRemains = additionalStepData.angle;
            currentStep = 1;
            currentSpeedSign = 1;
            currentWaitTime = 0;
        }
        
        #endregion
        
        #region fields
        
        public RotationType rotationType;

        public RotationReferential rotationRef;
        
        public RotationAxis rotationAxis;

        public float rotationSpeed;
        
        public CameraRotationAdditionalStepData additionalStepData;
        
        internal int currentStep;
        
        internal float currentWaitTime;

        internal float currentSpeedSign;

        internal float angleRemains;

        #endregion
    }

    [Serializable]
    internal class CameraRotationAdditionalStepData
    {
        #region fields
        
        public float angle;

        [Range(1, 5)]
        public int stepNumbers;
        
        public float waitTime;

        public bool loopAround;
        
        #endregion
    }
    
    internal enum RotationType : byte
    {
        None,
        Continuous,
        Step,
    }

    internal enum RotationReferential : byte
    {
        Local,
        Global,
    }
    
    internal enum RotationAxis : byte
    {
        X, Y, Z
    }
}