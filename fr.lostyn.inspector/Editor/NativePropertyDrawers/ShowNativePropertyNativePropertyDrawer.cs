using fr.lostyn.inspector;
using UnityEditor;

namespace fr.lostyneditor.inspector
{
    [NativePropertyDrawer(typeof(ShowNativePropertyAttribute))]
    public class ShowNativePropertyNativePropertyDrawer : NativePropertyDrawer {
        
        public override void DrawNativeProperty(UnityEngine.Object target, System.Reflection.PropertyInfo property){
            object value = property.GetValue(target, null);

            if (value == null)
            {
                string warning = string.Format("{0} doesn't support {1} types", typeof(ShowNativePropertyNativePropertyDrawer).Name, "Reference");
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, context: target);
            }
            else if (!EditorDrawUtility.DrawLayoutField(value, property.Name))
            {
                string warning = string.Format("{0} doesn't support {1} types", typeof(ShowNativePropertyNativePropertyDrawer).Name, property.PropertyType.Name);
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, context: target);
            }
        }
    }
}