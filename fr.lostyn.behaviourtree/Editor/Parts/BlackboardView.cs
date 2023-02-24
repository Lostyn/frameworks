using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class BlackboardView : VisualElement {
    public new class UxmlFactory : UxmlFactory<BlackboardView, VisualElement.UxmlTraits> { }

    public BlackboardView() { }

    public void Bind(SerializedBehaviourTree serializer) {
        Clear();

        var blackboardProperty = serializer.Blackboard;
        blackboardProperty.isExpanded = true;

        PropertyField field = new PropertyField(blackboardProperty);
        field.BindProperty(blackboardProperty);
        Add(field);
    }
}