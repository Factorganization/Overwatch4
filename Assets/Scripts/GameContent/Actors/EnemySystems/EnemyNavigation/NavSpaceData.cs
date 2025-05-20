using System.Collections.Generic;
using UnityEngine;

namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    [CreateAssetMenu(fileName = "NavSpaceData", menuName = "NavSpace/NavSpaceData")]
    public class NavSpaceData : ScriptableObject
    {
        #region methodes

        public void AddSubData(NavSpaceSubData subData)
        {
            subDatas.Add(subData);

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
        
        public void AddNode(SerializedOctreeNode node, NavSpaceSubData subData)
        {
            subData.AddNode(node);
        }
        
        public void AddEdge(SerializedOctreeEdge edge, NavSpaceSubData subData)
        {
            subData.AddEdge(edge);
        }

        #endregion
        
        #region fields

        /*[HideInInspector]*/ public List<NavSpaceSubData> subDatas;
/**/
        /*[HideInInspector]*/ public float minBoundSize;
/**/
        /*[HideInInspector]*/ public string subDataPath;

        #endregion
    }
}