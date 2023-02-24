using System.Reflection;
using fr.lostyn.inspector;
using UnityEditor;
using UnityEngine;

namespace fr.lostyneditor.inspector {
    [PropertyMeta(typeof( OnValueChangedAttribute) )]
    public class OnValueChangedPropertyMeta : PropertyMeta {
        public override void ApplyPropertyMeta( SerializedProperty property, MetaAttribute metaAttribute ) {
            OnValueChangedAttribute onValueChangedAttribute = (OnValueChangedAttribute) metaAttribute;
            UnityEngine.Object target = PropertyUtility.GetTargetObject( property );

            MethodInfo callbackMethod = ReflectionUtility.GetMethod( target, onValueChangedAttribute.CallbackName );
            if (callbackMethod != null && 
                callbackMethod.ReturnType == typeof(void) &&
                callbackMethod.GetParameters().Length == 0 ) 
            {
                // We must apply modified property before callback to have updated datas
                property.serializedObject.ApplyModifiedProperties();

                callbackMethod.Invoke( target, null );
            } else {
                string warning = onValueChangedAttribute.GetType().Name + "can invoke only action methods - with void return type and no params";
                Debug.LogWarning( warning, target );
            }
        }
    }
}