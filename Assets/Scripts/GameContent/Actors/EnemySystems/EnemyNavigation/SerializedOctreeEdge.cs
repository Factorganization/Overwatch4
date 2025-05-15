namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    [System.Serializable]
    public class SerializedOctreeEdge
    {
        #region constructors
        
        public SerializedOctreeEdge(Edge edge)
        {
            a = edge.a.id;
            b = edge.b.id;
        }
        
        #endregion
        
        #region fields
        
        public int a;

        public int b;
        
        #endregion
    }
}