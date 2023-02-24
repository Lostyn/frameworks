using System;

namespace fr.lostyn.inspector
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ShowNonSerializedFieldAttribute : DrawerAttribute{
        
    }
}