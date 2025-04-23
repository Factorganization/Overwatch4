using System;
using GameContent.Actors.EnemySystems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapLink : MonoBehaviour
{
     [SerializeField] private TMP_InputField _linkNameInputField;
     [SerializeField] private Button _sabotageButton;
     [SerializeField] private NetworkNode _linkedNode;
     [SerializeField] private EnemyCamera _enemyCamera;

     private void Awake()
     {
          _sabotageButton.onClick.AddListener(UnlinkDevice);
          _linkNameInputField.onEndEdit.AddListener(VerifyID);
     }

     private void Start()
     {
          _enemyCamera = (EnemyCamera)_linkedNode.actor;
          _linkNameInputField.text = _linkedNode.nodeId;
     }

     private void VerifyID(string playerInput)
     {
          if (_linkedNode.type != NodeType.Device) return;
          
          if (_linkNameInputField.text != _linkedNode.nodeId)
          {
               _enemyCamera.IsActive = false;
          }
          else
          {
               _enemyCamera.IsActive = true;
          }
     }

     private void UnlinkDevice()
     {
          _enemyCamera.IsActive = false;
     }
}
