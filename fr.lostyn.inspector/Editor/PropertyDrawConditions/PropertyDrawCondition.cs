using UnityEditor;

namespace fr.lostyneditor.inspector {
	public abstract class PropertyDrawCondition {
        public abstract bool CanDrawProperty( SerializedProperty property );
    }
}