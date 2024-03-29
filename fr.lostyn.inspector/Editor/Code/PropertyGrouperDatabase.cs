// This class is auto generated

using System;
using System.Collections.Generic;
using fr.lostyn.inspector;

namespace fr.lostyneditor.inspector
{
    public static class PropertyGrouperDatabase
    {
        private static Dictionary<Type, PropertyGrouper> groupersByAttributeType;

        static PropertyGrouperDatabase()
        {
            groupersByAttributeType = new Dictionary<Type, PropertyGrouper>();
            groupersByAttributeType[typeof(BoxGroupAttribute)] = new BoxGroupPropertyGrouper();

        }

        public static PropertyGrouper GetGrouperForAttribute(Type attributeType)
        {
            PropertyGrouper grouper;
            if (groupersByAttributeType.TryGetValue(attributeType, out grouper))
            {
                return grouper;
            }
            else
            {
                return null;
            }
        }
    }
}

