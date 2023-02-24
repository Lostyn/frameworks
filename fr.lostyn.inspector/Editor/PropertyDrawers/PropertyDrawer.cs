using UnityEditor;

namespace fr.lostyneditor.inspector {
    public abstract class PropertyDrawer {
        public abstract void DrawProperty( SerializedProperty property );

        public virtual void ClearCache() { }
    }
}