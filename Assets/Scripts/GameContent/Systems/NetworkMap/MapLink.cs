using GameContent.Actors.EnemySystems.Seekers;
using GameContent.Management;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapLink : MonoBehaviour
{
     [SerializeField] private TMP_InputField _linkNameInputField;
     [SerializeField] private Button _sabotageButton;
     [SerializeField] private NetworkNode _linkedNode;
     [SerializeField] private EnemyCamera _enemyCamera;
     [SerializeField] private float _suspicionValue;

     public EnemyCamera EnemyCamera { get { return _enemyCamera; } }

     private void Awake()
     {
          _sabotageButton.onClick.AddListener(UnlinkDevice);
          _linkNameInputField.onEndEdit.AddListener(VerifyID);
     }

     private void Start()
     {
          _enemyCamera = _linkedNode.actor as EnemyCamera;
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
          
          SuspicionManager.Manager.AddSuspicion(_suspicionValue);
     }

     public void UnlinkDevice()
     {
          _enemyCamera.IsActive = !_enemyCamera.IsActive;
        Debug.Log($"{_enemyCamera} is {_enemyCamera.IsActive}");
          SuspicionManager.Manager.AddSuspicion(_suspicionValue);
     }
}
