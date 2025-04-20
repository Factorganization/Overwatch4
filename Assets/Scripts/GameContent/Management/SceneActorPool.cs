using GameContent.Actors;
using UnityEngine;

namespace GameContent.Management
{
    public class SceneActorPool : MonoBehaviour
    {
        #region methodes

        private void Start()
        {
            foreach (var a in actors)
                a.Init(playerTransform);
        }

        private void Update()
        {
            foreach (var a in actors)
                a.OnUpdate();
        }
        
        #endregion
        
        #region fields

        [SerializeField] private Transform playerTransform;
        
        [SerializeField] private Actor[] actors;

        #endregion
    }
}