using System.Reflection;

namespace fr.lostyneditor.inspector {
    public abstract class NativePropertyDrawer {
        public abstract void DrawNativeProperty( UnityEngine.Object target, PropertyInfo property );
    }
}