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
                    if (Inventory.Instance.Controller.Model.Items[1].quantity <= 0 
                        || Hero.Instance.Health.CurrentHealth >= Hero.Instance.Health.MaxHealth) return;
                    
                    Hero.Instance.Health.Heal(25);
                    Inventory.Instance.Controller.SubtractItem(this,1);
                    break;
                case Action.Recharge:
                    if (Inventory.Instance.Controller.Model.Items[2].quantity <= 0 
                        || Hero.Instance.MultiToolObject.CurrentBattery >= Hero.Instance.MultiToolObject.MaxBattery) return;
                    
                    Hero.Instance.MultiToolObject.RechargeBattery(25);
                    Inventory.Instance.Controller.SubtractItem(this,1);
                    break;
                default:
                    break;
            }
        }
    }
}
