using System;

namespace fr.lostyneditor.inspector {
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public abstract class BaseAttribute : Attribute, IAttribute {
        public BaseAttribute(Type targetAttributeType ) {
            TargetAttributeType = targetAttributeType;
        }

        public Type TargetAttributeType { get; }
    }
}