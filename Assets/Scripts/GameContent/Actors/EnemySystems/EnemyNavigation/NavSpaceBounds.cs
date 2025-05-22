using UnityEngine;

namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    public class NavSpaceBounds : MonoBehaviour
    {
        #region properties

        public Bounds Bounds => bounds;

        #endregion

        #region methodes

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
            
            Gizmos.color = new Color(0f, 1f, 0.5f, 0.25f);
            Gizmos.DrawCube(bounds.center, bounds.size);
        }

        #endregion
        
        #region fields

        [SerializeField] private Bounds bounds;

        #endregion
    }
}