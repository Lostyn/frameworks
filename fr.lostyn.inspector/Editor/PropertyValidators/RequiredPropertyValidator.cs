using fr.lostyn.inspector;
using UnityEditor;

namespace fr.lostyneditor.inspector
{
    [PropertyValidator(typeof(RequiredAttribute))]
    public class RequiredPropertyValidator : PropertyValidator
    {
        public override void ValidateProperty(SerializedProperty property)
        {
            RequiredAttribute requiredAttribute = PropertyUtility.GetAttribute<RequiredAttribute>(property);

            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                if (property.objectReferenceValue == null)
                {
                    string errorMessage = property.name + " is required";
                    if (!string.IsNullOrEmpty(requiredAttribute.Message))
                    {
                        errorMessage = requiredAttribute.Message;
                    }

                    EditorDrawUtility.DrawHelpBox(errorMessage, MessageType.Error, context: PropertyUtility.GetTargetObject(property), logToConsole: false);
                }
            }
            else
            {
                string warning = requiredAttribute.GetType().Name + " works only on reference types";
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, context: PropertyUtility.GetTargetObject(property), logToConsole: false);
            }
        }
    }
}
