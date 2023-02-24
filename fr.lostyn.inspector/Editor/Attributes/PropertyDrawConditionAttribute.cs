using System;

namespace fr.lostyneditor.inspector {
    public class PropertyDrawConditionAttribute : BaseAttribute {
        public PropertyDrawConditionAttribute(Type targetAttributeType): base(targetAttributeType) { }
    }
}