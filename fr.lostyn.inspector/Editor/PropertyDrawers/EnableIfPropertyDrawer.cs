using System.Collections.Generic;
using System.Reflection;
using fr.lostyn.inspector;
using UnityEditor;
using UnityEngine;

namespace fr.lostyneditor.inspector {
    [PropertyDrawer(typeof(EnableIfAttribute))]
    public class EnableIfPropertyDrawer : PropertyDrawer {
        public override void DrawProperty( SerializedProperty property ) {
            EnableIfAttribute enableIfAttribute = PropertyUtility.GetAttribute<EnableIfAttribute>( property );
            UnityEngine.Object target = PropertyUtility.GetTargetObject( property );

            List<bool> conditionValues = new List<bool>();
            foreach( var condition in enableIfAttribute.Conditions ) {
                FieldInfo conditionField = ReflectionUtility.GetField( target, condition );
                if (conditionField != null &&
                    conditionField.FieldType == typeof(bool)) 
                {
                    conditionValues.Add( (bool) conditionField.GetValue( target ) );    
                }

                MethodInfo conditionMethod = ReflectionUtility.GetMethod( target, condition );
                if (conditionMethod != null &&
                    conditionMethod.ReturnType == typeof(bool) && 
                    conditionMethod.GetParameters().Length == 0) 
                {
                    conditionValues.Add( (bool) conditionMethod.Invoke( target, null ) );
                }
            }

            if (conditionValues.Count > 0) {
                bool enable;
                if (enableIfAttribute.ConditionOperator == ConditionOperator.And) {
                    enable = true;
                    foreach( var value in conditionValues )
                        enable = enable && value;
                } else {
                    enable = false;
                    foreach( var value in conditionValues )
                        enable = enable || value;
                }

                if( enableIfAttribute.Reversed )
                    enable = !enable;

                GUI.enabled = enable;
                EditorDrawUtility.DrawPropertyField( property );
                GUI.enabled = true;
            }
        }
    }
}