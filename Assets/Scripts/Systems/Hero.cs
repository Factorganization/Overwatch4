using System;
using Systems.Inventory;
using Systems.Inventory.Interface;
using Systems.Persistence;
using UnityEngine;
using UnityEngine.UI;
using Type = Systems.Inventory.Type;

namespace Systems
{
    public class Hero : MonoBehaviour, IBind<PlayerData>
    {
        public static Hero Instance;
        
        [field: SerializeField]public SerializableGuid Id { get; set; } = SerializableGuid.NewGuid();
        [SerializeField] private Camera camera;
        [SerializeField] private PlayerData data;
        [SerializeField] private ItemDetails currentEquipedItem;
        [SerializeField] private Image _hackProgressImage;
        [SerializeField] private MultiTool _multiToolObject;
        
        private HackableJunction _currentJunction;
        private float _currentHackTimer;
        private bool _isHacking;
        
        public ItemDetails CurrentEquipedItem
        {
            get => currentEquipedItem;
            set => currentEquipedItem = value;
        }

        private void Awake()
        {
            Instance = this;
        }
        
        public void Bind(PlayerData data)
        {
            this.data = data;
            this.data.Id = Id;
            transform.position = data.position;
            transform.rotation = data.rotation;
        }

        private void Update()
        {
            data.position = transform.position;
            data.rotation = transform.rotation;
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                TryInteract();
            }

            if (Input.GetKey(KeyCode.E) && _isHacking)
            {
                ContinueHack();
            }
            else if (Input.GetKeyUp(KeyCode.E) && _isHacking)
            {
                CancelHack();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                NetworkMapController.Instance.CloseNetworkMap();
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                NetworkMapController.Instance.OpenNetworkMap();
            }
        }
        
        private void TryInteract()
        {
            if (!currentEquipedItem)
                return;

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 5f))
            {
                var interactible = hit.collider.GetComponent<IInteractible>();

                if (currentEquipedItem.type == Type.MultiTool)
                {
                    if (interactible is HackableJunction junction && junction._alrHacked == false)
                    {
                        _currentJunction = junction;
                        _isHacking = true;
                        _currentHackTimer = 0f;
                        
                        _hackProgressImage.gameObject.SetActive(true);
                        _hackProgressImage.fillAmount = 0;
                    }
                    else
                    {
                        interactible?.OnInteract();
                    }
                    return;
                }
                
                if (interactible != null)
                {
                    interactible.OnInteract();
                }
            }
        }
        
        private void ContinueHack()
        {
            if (!_currentJunction) return;

            _currentHackTimer += Time.deltaTime;
            
            float progress = _currentHackTimer / _currentJunction.HackingTime;
            _hackProgressImage.fillAmount = Mathf.Clamp01(progress);

            if (_currentHackTimer >= _currentJunction.HackingTime)
            {
                _currentJunction.OnInteract();
                _currentJunction._alrHacked = true;
                _multiToolObject.ConsumeBattery(0.1f);
                CancelHack();
            }
        }

        private void CancelHack()
        {
            ResetHack();
        }

        private void ResetHack()
        {
            _isHacking = false;
            _currentHackTimer = 0f;
            _currentJunction = null;
            _hackProgressImage.fillAmount = 0;
            _hackProgressImage.gameObject.SetActive(false);
        }
    }

    [Serializable]
    public class PlayerData : ISaveable
    {
        [field: SerializeField] public SerializableGuid Id { get; set; }
        public Vector3 position;
        public Quaternion rotation;
    }
}