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
     [SerializeField] private float _suspicionValue;
     
     private RoomMap _roomMap;
     private EnemyCamera _enemyCamera;

     public RoomMap RoomMap
     {
          get => _roomMap;
          set => _roomMap = value;
     }
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

     // Verify if the Id is correct, if it's not correct,
     // it's will change the information that the camera will send to the processor
     private void VerifyID(string playerInput)
     {
          if (_linkedNode.type != NodeType.Device) return;
          
          if (_linkNameInputField.text != _linkedNode.nodeId)
          {
               for (int i = 0; i < _roomMap.MapLink.Count; i++)
               {
                    if (_roomMap.MapLink[i]._linkedNode.nodeId == _linkNameInputField.text)
                    {
                         // Will change the information that the camera will send to the processor
                         SuspicionManager.Manager.StartTrack(_roomMap.MapLink[i]._linkedNode.actor as EnemyCamera);
                         _linkedNode._connectedNodes = _roomMap.MapLink[i]._linkedNode._connectedNodes;
                         return;
                    }
               }
          }
          
          SuspicionManager.Manager.AddSuspicion(_suspicionValue);
     }

     public void UnlinkDevice()
     {
          _enemyCamera.IsActive = !_enemyCamera.IsActive;
          SuspicionManager.Manager.AddSuspicion(_suspicionValue);
     }
}
