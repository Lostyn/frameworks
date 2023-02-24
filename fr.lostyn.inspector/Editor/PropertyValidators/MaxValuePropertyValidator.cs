using fr.lostyn.inspector;
using UnityEditor;

namespace fr.lostyneditor.inspector
{
    [PropertyValidator(typeof(MaxValueAttribute))]
    public class MaxValuePropertyValidator : PropertyValidator {
        
        public override void ValidateProperty(UnityEditor.SerializedProperty property) {
            MaxValueAttribute maxValueAttribute = PropertyUtility.GetAttribute<MaxValueAttribute>(property);

            if (property.propertyType == SerializedPropertyType.Integer)
            {
                if (property.intValue > maxValueAttribute.MaxValue)
                {
                    property.intValue = (int)maxValueAttribute.MaxValue;
                }
            }
            else if (property.propertyType == SerializedPropertyType.Float)
            {
                if (property.floatValue > maxValueAttribute.MaxValue)
                {
                    property.floatValue = maxValueAttribute.MaxValue;
                }
            }
            else
            {
                string warning = maxValueAttribute.GetType().Name + " can be used only on int or float fields";
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, context: PropertyUtility.GetTargetObject(property), logToConsole: false);
            }
        }
    }
}