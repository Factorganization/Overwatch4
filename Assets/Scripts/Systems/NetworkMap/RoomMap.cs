using System.Collections.Generic;
using UnityEngine;

public class RoomMap : MonoBehaviour
{
    [SerializeField] List<NetworkNode> _nodes = new List<NetworkNode>();
    [SerializeField] List<MapLink> _mapLink = new List<MapLink>();
}
