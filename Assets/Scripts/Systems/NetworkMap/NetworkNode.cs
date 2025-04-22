using System;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType { Junction, Device, Processor }

public class NetworkNode : MonoBehaviour
{
    public string nodeId;
    public NodeType type;
    public GameObject nodeVisual;
    public List<NetworkNode> connectedNodes = new();

    public void ConnectTo(NetworkNode other)
    {
        if (!connectedNodes.Contains(other)) connectedNodes.Add(other);
        if (!other.connectedNodes.Contains(this)) other.connectedNodes.Add(this);
    }
}