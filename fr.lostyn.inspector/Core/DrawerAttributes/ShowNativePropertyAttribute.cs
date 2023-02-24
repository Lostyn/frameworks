using System;

namespace fr.lostyn.inspector {
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ShowNativePropertyAttribute : DrawerAttribute {
        
    }
}