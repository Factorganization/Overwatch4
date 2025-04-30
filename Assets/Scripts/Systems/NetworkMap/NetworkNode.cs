using GameContent.Actors;
using UnityEngine;

public enum NodeType { Junction, Device, Processor }

public class NetworkNode : MonoBehaviour
{
    public string nodeId;
    public NodeType type;
    public GameObject nodeVisual;
    public Actor actor;
}