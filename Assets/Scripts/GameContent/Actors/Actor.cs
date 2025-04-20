using UnityEngine;

namespace GameContent.Actors
{
    public abstract class Actor : MonoBehaviour
    {
        #region methodes

        public abstract void Init(Transform player);
        
        public abstract void OnUpdate();

        public virtual void OnAction()
        {
        }
        
        #endregion

        #region fields

        protected Transform playerTransform;

        #endregion
    }
}