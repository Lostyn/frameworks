using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(SubtreeNode))]
public class SubtreeNodeInspector : NodeInspector
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var container = base.CreatePropertyGUI(property);

        var subContainer = new VisualElement();
        subContainer.style.flexDirection = FlexDirection.Row;

        container.Add(subContainer);

        var subtreeProperty = property.Find("subtree");
        var subtreeField = new PropertyField( property.Find("subtree") );
        var btn = new Button(() => {
            BehaviourTreeEditor.OpenWindow(subtreeProperty.objectReferenceValue as BehaviourTree);
        });
        btn.text = ">";
        btn.style.width = new StyleLength(20);

        subContainer.Add(subtreeField);
        subContainer.Add(btn);

        return container;
    }
}

