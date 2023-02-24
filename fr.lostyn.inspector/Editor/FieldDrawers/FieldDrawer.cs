using System.Reflection;

namespace fr.lostyneditor.inspector {
    public abstract class FieldDrawer {
        public abstract void DrawField( UnityEngine.Object target, FieldInfo field );
    }
}
