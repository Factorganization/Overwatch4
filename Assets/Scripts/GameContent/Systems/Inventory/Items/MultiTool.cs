using Systems.Inventory;
using UnityEngine;

public class MultiTool : MonoBehaviour
{
    [SerializeField] ItemDetails itemDetails;
    [SerializeField] private float _maxBattery;
    [SerializeField] private float _currentBattery;

    public float CurrentBattery
    {
        get => _currentBattery;
        set => _currentBattery = value;
    }
    
    public float MaxBattery => _maxBattery;
    
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
        if (_currentBattery >= _maxBattery)
        {
            Debug.Log("Battery is full");
            _currentBattery = _maxBattery;
            return;
        }
        _currentBattery += amount;
    }
}
