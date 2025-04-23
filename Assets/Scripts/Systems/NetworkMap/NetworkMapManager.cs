using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class NetworkMapController : MonoBehaviour
{
    public static NetworkMapController Instance;
    
    [SerializeField] private Camera _networkMapCamera;
    [SerializeField] private GameObject _networkMapUI;
    [SerializeField] private List<RoomMap> _roomMaps = new List<RoomMap>();

    private void Awake()
    {
        Instance = this;
    }

    public void RevealRoom(RoomMap roomMap)
    {
       roomMap.gameObject.SetActive(true); 
    }
    
    public void OpenNetworkMap()
    {
        _networkMapUI.SetActive(true);
        _networkMapCamera.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    public void CloseNetworkMap()
    {
        _networkMapUI.SetActive(false);
        _networkMapCamera.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}