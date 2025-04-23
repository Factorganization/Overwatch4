using Systems.Inventory.Interface;
using UnityEngine;

public class HackableJunction : MonoBehaviour, IInteractible
{
    [SerializeField] private RoomMap map;

    [SerializeField] private float _hackingTime;
    public float HackingTime => _hackingTime;
    
    public bool _alrHacked;
    
    private void Start()
    {
        _alrHacked = false;
    }
    
    public void OnInteract() 
    {
        if (_alrHacked == false) OnHack();
    }
    
    void OnHack()
    {
        _alrHacked = true;
        NetworkMapController.Instance.RevealRoom(map);
    }
    
    
}