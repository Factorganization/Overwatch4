using CustomSceneManagement;
using UnityEditor;
using UnityEngine.UIElements;

namespace Editor.CustomScene
{
    [CustomEditor(typeof(SceneListSo))]
    public class CustomSceneEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            return root;
        }
    }
}