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
    
    public override void OnInteract() 
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
        foreach (var t in map.MapLink)
        {
            t.UnlinkDevice();
        }
        yield return new WaitForSeconds(10f);
        foreach (var t in map.MapLink)
        {
            t.UnlinkDevice();
        }
        yield return new WaitForSeconds(_coolDownTime);
        _onCooldown = false;
        _alrHacked = false;
    }
}
