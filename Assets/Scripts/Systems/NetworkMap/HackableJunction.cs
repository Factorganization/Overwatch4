using Systems.Inventory.Interface;
using UnityEngine;

public class HackableJunction : MonoBehaviour, IInteractible
{
    private NetworkNode node;
    [SerializeField] private RunTimeUI runtimeUI;

    [SerializeField] private float _hackingTime;
    public float HackingTime => _hackingTime;
    
    public bool _alrHacked;
    
    private void Start()
    {
        node = GetComponent<NetworkNode>();
        _alrHacked = false;
    }
    
    public void OnInteract() 
    {
        if (_alrHacked == false) OnHack();
        else OpenMap();
    }
    
    void OnHack()
    {
        _alrHacked = true;
        runtimeUI.RevealFromJunction(node);
    }

    private void OpenMap()
    {
        NetworkMapController.Instance.OpenNetworkMap();
    } 
    
    public void CloseMap() => NetworkMapController.Instance.CloseNetworkMap();
    
}