using UnityEditor;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(RootNode))]
public class RootNodeInspector : NodeInspector {
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var container = base.CreatePropertyGUI(property);

        return container;
    }
}