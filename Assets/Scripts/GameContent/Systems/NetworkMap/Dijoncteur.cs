using System.Collections;
using UnityEngine;

public class Dijoncteur : HackableJunction
{
    [SerializeField] private float _coolDownTime;
    
    public bool _onCooldown;
    
    private void Start()
    {
        _onCooldown = false;
    }
    
    public new void OnInteract() 
    {
        if (_onCooldown == false) OnHack();
    }

    protected override void OnHack()
    {
        _onCooldown = true;
        
        StartCoroutine(PowerShortage());
    }

    IEnumerator PowerShortage()
    {
        for (int i = 0; i < map.MapLink.Count; i++)
        {
            map.MapLink[i].UnlinkDevice();
        }
        yield return new WaitForSeconds(10f);
        for (int i = 0; i < map.MapLink.Count; i++)
        {
            map.MapLink[i].UnlinkDevice();
        }
        yield return new WaitForSeconds(_coolDownTime);
        _onCooldown = false;
        _alrHacked = false;
    }
}
