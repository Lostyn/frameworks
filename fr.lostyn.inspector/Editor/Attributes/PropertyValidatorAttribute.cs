using System;

namespace fr.lostyneditor.inspector {
    public class PropertyValidatorAttribute : BaseAttribute {
        public PropertyValidatorAttribute(Type targetAttributeType) : base (targetAttributeType) { }
    }
}