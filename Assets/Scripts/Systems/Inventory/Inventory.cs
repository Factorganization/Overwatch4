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
        
        [SerializeField] int capacity;
        [SerializeField] List<ItemDetails> startingItems = new List<ItemDetails>();
        [SerializeField] private RadialMenu radialMenu;
        
        [field: SerializeField]public SerializableGuid Id { get; set; }
        
        public RadialMenu RadialMenu => radialMenu;
        
        InventoryController controller;
        
        public InventoryController Controller => controller;
        
        private void Awake()
        {
            controller = new InventoryController.Builder()
                .WithStartingItems(startingItems)
                .WithCapacity(capacity)
                .Build();
            Instance = this;
        }

        public void Update()
        {
            // Temporaire pour ouvrir et fermer l'inventaire
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                radialMenu.Open();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else if (Input.GetKeyUp(KeyCode.Tab))
            {
                radialMenu.Close();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        public void Bind(InventoryData data)
        {
            controller.Bind(data); 
            data.Id = Id; 
        }
        
        public void EquipItem(ItemDetails item)
        {
            Hero.Instance.CurrentEquipedItem = item;
        }
    }
}