using GameContent.Actors.EnemySystems.EnemyNavigation;
using UnityEditor;
using UnityEngine.UIElements;

namespace Editor.NavSpace
{
    [CustomEditor(typeof(NavSpaceGenerator))]
    public class NavSpaceEditor : UnityEditor.Editor
    {
        #region methodes

        private void OnEnable()
        {
            _target = (NavSpaceGenerator)target;
        }

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            visualTree.CloneTree(root);
            
            _bakeButton = root.Q<Button>("Bake");
            _bakeButton.RegisterCallback<ClickEvent>(OnBake);
            
            return root;
        }

        private void OnBake(ClickEvent evt)
        {
            _target.Bake();
        }

        #endregion

        #region fields

        public VisualTreeAsset visualTree;
        
        private Button _bakeButton;
        
        private NavSpaceGenerator _target;

        #endregion
    }
}