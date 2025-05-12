using Systems.Inventory;
using UnityEngine;

public class MultiTool : MonoBehaviour
{
    [SerializeField] ItemDetails itemDetails;
    [SerializeField] private float _maxBattery;
    [SerializeField] private float _currentBattery;

    public float Battery
    {
        get => _currentBattery;
        set => _currentBattery = value;
    }
    
    private void Start()
    {
        _maxBattery = 100f;
        _currentBattery = _maxBattery;
    }
    
    public void ConsumeBattery(float amount)
    {
        if (_currentBattery - amount < 0)
        {
            Debug.Log("Battery is empty");
            return;
        }
        
        _currentBattery -= amount;
    }
    
    public void RechargeBattery(float amount)
    {
        _currentBattery += amount;
    }
}
