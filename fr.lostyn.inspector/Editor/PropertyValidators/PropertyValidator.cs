using UnityEditor;

namespace fr.lostyneditor.inspector {
    public abstract class PropertyValidator {
        public abstract void ValidateProperty( SerializedProperty property );
    }
}