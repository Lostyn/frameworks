using System.Linq;
using System.Reflection;
using fr.lostyn.inspector;
using UnityEditor;
using UnityEngine;

namespace fr.lostyneditor.inspector {
    [MethodDrawer(typeof(ButtonAttribute))]
    public class ButtonMethodDrawer : MethodDrawer {
        public override void DrawMethod( Object target, MethodInfo methodInfo ) {
            if (methodInfo.GetParameters().Length == 0 ) {
                ButtonAttribute buttonAttribute = methodInfo.GetCustomAttributes<ButtonAttribute>( true ).ElementAt( 0 );
                string buttonText = string.IsNullOrEmpty( buttonAttribute.Text ) ? methodInfo.Name : buttonAttribute.Text;

                if( GUILayout.Button( buttonText ) ) {
                    methodInfo.Invoke( target, null );
                }
            } else {
                string warning = typeof( ButtonAttribute ).Name + " works only on methods without params";
                EditorDrawUtility.DrawHelpBox( warning, MessageType.Warning, context: target );
            }
        }
    }
}