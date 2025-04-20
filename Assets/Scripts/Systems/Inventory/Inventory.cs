using System;
using System.Collections.Generic;
using Systems.Persistence;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace Systems.Inventory {
    public class Inventory : MonoBehaviour, IBind<InventoryData>
    {
        public static Inventory Instance;
        
        [SerializeField] InventoryView view;
        [SerializeField] int capacity = 21;
        [SerializeField] UIDocument uiDocument;
        [SerializeField] private bool isOpen;
        [SerializeField] List<ItemDetails> startingItems = new List<ItemDetails>();
        
        [field: SerializeField]public SerializableGuid Id { get; set; }
        
        InventoryController controller;
        
        public InventoryController Controller => controller;
        
        private void Awake()
        {
            controller = new InventoryController.Builder(view)
                .WithStartingItems(startingItems)
                .WithCapacity(capacity)
                .Build();
            isOpen = true;
            Instance = this;
        }

        public void Update()
        {
            // Temporaire pour ouvrir et fermer l'inventaire
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                isOpen = !isOpen;
                uiDocument.rootVisualElement.style.display = isOpen ? DisplayStyle.Flex : DisplayStyle.None;

                if (isOpen)
                {
                    Cursor.lockState = CursorLockMode.None;
                    
                    Cursor.visible = true;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                SaveLoadSystem.Instance.SaveGame();
            }
            
            if (Input.GetKeyDown(KeyCode.T))
            {
                SaveLoadSystem.Instance.LoadGame("New Game");
            }
        }

        public void Bind(InventoryData data)
        {
            controller.Bind(data); 
            data.Id = Id; 
        }
    }
}