using System;

namespace fr.lostyneditor.inspector {
    public class PropertyDrawerAttribute : BaseAttribute {
        public PropertyDrawerAttribute( Type targetAttributeType ) : base( targetAttributeType ) { }
    }
}