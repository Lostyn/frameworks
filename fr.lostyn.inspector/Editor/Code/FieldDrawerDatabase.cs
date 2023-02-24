// This class is auto generated

using System;
using System.Collections.Generic;
using fr.lostyn.inspector;

namespace fr.lostyneditor.inspector
{
    public static class FieldDrawerDatabase
    {
        private static Dictionary<Type, FieldDrawer> drawersByAttributeType;

        static FieldDrawerDatabase()
        {
            drawersByAttributeType = new Dictionary<Type, FieldDrawer>();
            drawersByAttributeType[typeof(ShowNonSerializedFieldAttribute)] = new ShowNonSerializedFieldFieldDrawer();

        }

        public static FieldDrawer GetDrawerForAttribute(Type attributeType)
        {
            FieldDrawer drawer;
            if (drawersByAttributeType.TryGetValue(attributeType, out drawer))
            {
                return drawer;
            }
            else
            {
                return null;
            }
        }
    }
}

