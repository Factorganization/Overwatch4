using UnityEngine;

namespace Systems.Inventory
{
    public enum Type
    {
        MultiTool,
        Consumable,
    }

    public enum Action
    {
        None,
        Heal,
        Recharge,
    }
    
    [CreateAssetMenu(fileName = "New Item", menuName = "Scriptable Objects/ItemDetails")]
    public class ItemDetails : ScriptableObject
    {
        public string Name;
        public Type type;
        public Action action;
        public int maxStack;
        public SerializableGuid id = SerializableGuid.NewGuid();
        
        private void AssignNewGuid()
        {
            id = SerializableGuid.NewGuid();
        }
        
        public Sprite icon;
        [TextArea] public string description;

        public Item CreateItem(int quantity)
        {
            return new Item(this, quantity);
        }

        public virtual void OnAction()
        {
            switch (action)
            {
                case Action.None:
                    break;
                case Action.Heal: 
                    if (Inventory.Instance.Controller.Model.Items[1].quantity <= 0) return;
                    Hero.Instance.Health.Heal(25);
                    Inventory.Instance.Controller.SubtractItem(this,1);
                    Debug.Log("Heal");
                    break;
                case Action.Recharge:
                    Debug.Log("Recharge");
                    if (Inventory.Instance.Controller.Model.Items[2].quantity <= 0) return;
                    Inventory.Instance.Controller.SubtractItem(this,1);
                    break;
                default:
                    break;
            }
        }
    }
}
