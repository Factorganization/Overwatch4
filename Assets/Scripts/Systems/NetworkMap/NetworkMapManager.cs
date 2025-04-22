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

    private void Awake()
    {
        Instance = this;
    }

    public void RevealNode(NetworkNode node)
    {
        foreach (NetworkNode nodes in node.connectedNodes)
        {
            if (nodes.nodeVisual != null)
            {
                nodes.nodeVisual.SetActive(true);
            }
        }
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