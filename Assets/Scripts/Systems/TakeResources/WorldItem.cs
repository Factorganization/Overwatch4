using System;
using Systems.Inventory;
using UnityEngine;

namespace Systems.TakeResources
{
    public class WorldItem : MonoBehaviour
    {
        public ItemDetails itemDetails;
        public int quantity;

        private void OnInteract()
        {
            Inventory.Inventory.Instance.Controller.AddItem(itemDetails, quantity);
            // Temporaire pour les tests
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
               OnInteract();
            }
        }
    }
}
