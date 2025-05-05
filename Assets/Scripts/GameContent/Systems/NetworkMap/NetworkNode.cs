using GameContent.Actors;
using TMPro;
using UnityEngine;

public enum NodeType { Junction, Device, Processor }

public class NetworkNode : MonoBehaviour
{
    public string nodeId;
    public NodeType type;
    public GameObject nodeVisual;
    public Actor actor;
    public TextMeshProUGUI name;

    private void Start()
    {
        name.text = nodeId;
    }
}