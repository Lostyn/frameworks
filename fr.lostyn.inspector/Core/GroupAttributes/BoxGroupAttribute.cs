using System;

namespace fr.lostyn.inspector {
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class BoxGroupAttribute : GroupAttribute {
        public BoxGroupAttribute( string name = "" ) : base( name ) { }
    }
}