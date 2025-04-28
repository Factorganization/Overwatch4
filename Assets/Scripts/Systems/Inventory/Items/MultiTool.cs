using Systems.Inventory;
using UnityEngine;

public class MultiTool : MonoBehaviour
{
    [SerializeField] ItemDetails itemDetails;
    [SerializeField] private float _battery;
    
    public float Battery
    {
        get => _battery;
        set => _battery = value;
    }
    
    public void ConsumeBattery(float amount)
    {
        if (_battery - amount < 0)
        {
            Debug.Log("Battery is empty");
            return;
        }
        
        _battery -= amount;
    }
    
    public void RechargeBattery(float amount)
    {
        _battery += amount;
    }
}
