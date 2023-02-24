using System;

namespace fr.lostyneditor.inspector {
    public class PropertyGrouperAttribute : BaseAttribute {
        public PropertyGrouperAttribute(Type targetAttributeType) : base( targetAttributeType ) { }
    }
}