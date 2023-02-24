using System;

namespace fr.lostyn.inspector {
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class OnValueChangedAttribute : MetaAttribute {
        public string CallbackName { get; private set; }

        public OnValueChangedAttribute(string callbaclName) {
            CallbackName = callbaclName;
        }
    }
}