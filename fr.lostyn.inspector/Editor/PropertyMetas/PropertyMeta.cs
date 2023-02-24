using fr.lostyn.inspector;
using UnityEditor;

namespace fr.lostyneditor.inspector {
    public abstract class PropertyMeta {
        public abstract void ApplyPropertyMeta( SerializedProperty property, MetaAttribute metaAttribute );
    }
}