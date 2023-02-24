using System.Reflection;

namespace fr.lostyneditor.inspector {
    public abstract class MethodDrawer {
        public abstract void DrawMethod( UnityEngine.Object target, MethodInfo methodInfo );
    }
}
