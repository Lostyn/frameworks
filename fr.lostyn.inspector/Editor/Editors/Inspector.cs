using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using fr.lostyn.inspector;
using UnityEditor;
using UnityEngine;

namespace fr.lostyneditor.inspector {

    [CanEditMultipleObjects]
    [CustomEditor(typeof(Object), true)]
    public class Inspector : Editor 
    {
        private SerializedProperty _script;

        private IEnumerable<FieldInfo> _fields;

        private bool _useDefaultInspector;
        private HashSet<FieldInfo> _groupedFields;
        private Dictionary<string, List<FieldInfo>> _groupedFieldsByGroupName;
        private Dictionary<string, SerializedProperty> _serializedPropertiesByFieldName;
        private IEnumerable<FieldInfo> _nonSerializedFields;
        private IEnumerable<PropertyInfo> _nativeProperties;
        private IEnumerable<MethodInfo> _methods;

        private void OnEnable() {
            _script = this.serializedObject.FindProperty( "m_Script" );

            // Cache serialized fields
            _fields = ReflectionUtility.GetAllFields( target, f => this.serializedObject.FindProperty( f.Name ) != null );

            // If there are no custom attributes use default inspector
            if (_fields.All( f => f.GetCustomAttributes<HYPAttribute>( true ).Count() == 0)) 
            {
                _useDefaultInspector = true;
            } 
            else 
            {
                _useDefaultInspector = false;

                // Cache grouped fields
                _groupedFields = new HashSet<FieldInfo>( _fields.Where( f => f.GetCustomAttributes<GroupAttribute>( true ).Count() > 0 ) );

                // Cache grouped fields by group name
                _groupedFieldsByGroupName = new Dictionary<string, List<FieldInfo>>();
                foreach( var groupedField in _groupedFields ) {
                    string groupName = groupedField.GetCustomAttributes<GroupAttribute>( true ).First().Name;

                    if( _groupedFieldsByGroupName.ContainsKey( groupName ) ) {
                        _groupedFieldsByGroupName[groupName].Add( groupedField );
                    }
                    else 
                    {
                        _groupedFieldsByGroupName[groupName] = new List<FieldInfo>() {
                            groupedField
                        };
                    }
                }

                // Cache serialized properties by field name
                _serializedPropertiesByFieldName = new Dictionary<string, SerializedProperty>();
                foreach( var field in _fields )
                    _serializedPropertiesByFieldName[field.Name] = serializedObject.FindProperty( field.Name );
            }

            // Cache non-serialized fields
            _nonSerializedFields = ReflectionUtility.GetAllFields( 
                target, f => f.GetCustomAttributes<DrawerAttribute>( true ).Count() > 0 && serializedObject.FindProperty( f.Name ) == null );

            // Cache the native properties
            _nativeProperties = ReflectionUtility.GetAllProperties(
                target, p => p.GetCustomAttributes<DrawerAttribute>( true ).Count() > 0 );

            // Cache methods with drawer
            _methods = ReflectionUtility.GetAllMethods(
                target, p => p.GetCustomAttributes<DrawerAttribute>( true ).Count() > 0 );
        }

        private void OnDisable() {
            PropertyDrawerDatabase.ClearCache();
        }

        public override void OnInspectorGUI() {
            if( _useDefaultInspector ) {
                DrawDefaultInspector();
            } else {
                serializedObject.Update();
                
                if (_script != null ) {
                    GUI.enabled = false;
                    EditorGUILayout.PropertyField( _script );
                    GUI.enabled = true;
                }

                // Draw fields
                HashSet<string> drawnGroups = new HashSet<string>();
                foreach( var field in _fields ) {
                    if (_groupedFields.Contains(field)) {
                        // Draw grouped fields
                        string groupName = field.GetCustomAttributes<GroupAttribute>( true ).ElementAt(0).Name;
                        if( !drawnGroups.Contains( groupName ) ) {
                            drawnGroups.Add( groupName );

                            PropertyGrouper grouper = GetPropertyGrouperForField( field );
                            if (grouper != null) {
                                grouper.BeginGroup( groupName );
                                ValidateAndDrawFields( _groupedFieldsByGroupName[groupName] );
                                grouper.EndGroup();
                            }
                            else 
                            {
                                ValidateAndDrawFields( _groupedFieldsByGroupName[groupName] );
                            }
                        }
                    } 
                    else {
                        // Draw non-grouped field
                        ValidateAndDrawField( field );
                    }
                }

                this.serializedObject.ApplyModifiedProperties();
            }

            // Draw non-serialized fields
            foreach( var field in _nonSerializedFields ) {
                DrawerAttribute drawerAttribute = field.GetCustomAttributes<DrawerAttribute>( true ).ElementAt( 0 );
                FieldDrawer drawer = FieldDrawerDatabase.GetDrawerForAttribute( drawerAttribute.GetType() );
                if( drawer != null )
                    drawer.DrawField( target, field );
            }

            // Draw native properties
            foreach( var property in _nativeProperties ) {
                DrawerAttribute drawerAttribute = property.GetCustomAttributes<DrawerAttribute>( true ).ElementAt( 0 );
                NativePropertyDrawer drawer = NativePropertyDrawerDatabase.GetDrawerForAttribute( drawerAttribute.GetType() );
                if( drawer != null )
                    drawer.DrawNativeProperty( target, property );
            }

            // Draw methods
            foreach(var method in _methods ) {
                DrawerAttribute drawerAttribute = method.GetCustomAttributes<DrawerAttribute>( true ).ElementAt( 0 );
                MethodDrawer methodDrawer = MethodDrawerDatabase.GetDrawerForAttribute( drawerAttribute.GetType() );
                if( methodDrawer != null )
                    methodDrawer.DrawMethod( target, method );
            }
        }


        private void ValidateAndDrawFields(IEnumerable<FieldInfo> fields ) {
            foreach( var field in fields ) {
                ValidateAndDrawField( field );
            }
        }

        private void ValidateAndDrawField(FieldInfo field ) {
            if( !ShouldDrawField( field ) )
                return;

            ValidateField( field );
            ApplyFieldMeta( field );
            DrawField( field );
        }

        private bool ShouldDrawField(FieldInfo field) {
            // Check if the field has draw conditions
            PropertyDrawCondition drawCondition = GetPropertyDrawConditionForField( field );
            if (drawCondition != null ) {
                bool canDrawProperty = drawCondition.CanDrawProperty( _serializedPropertiesByFieldName[field.Name] );
                if( !canDrawProperty )
                    return false;
            }

            // Check if the field has HideInInspectorAttribute
            IEnumerable<HideInInspector> hideInInspectorAttributes = field.GetCustomAttributes<HideInInspector>( true );
            if( hideInInspectorAttributes.Count() > 0 )
                return false;

            return true;
        }

        private void ValidateField(FieldInfo field) {
            IEnumerable<ValidatorAttribute> validatorAttributes = field.GetCustomAttributes<ValidatorAttribute>( true );

            foreach( var attribute in validatorAttributes ) {
                PropertyValidator validator = PropertyValidatorDatabase.GetValidatorForAttribute( attribute.GetType() );
                if( validator != null )
                    validator.ValidateProperty( _serializedPropertiesByFieldName[field.Name] );
            }
        }

        private void ApplyFieldMeta(FieldInfo field) {
            // Apply custom meta attributes
            IEnumerable<MetaAttribute> metaAttributes = field
                .GetCustomAttributes<MetaAttribute>( true )
                .Where( attr => attr.GetType() != typeof( OnValueChangedAttribute ) )
                .Select( obj => obj as MetaAttribute )
                .OrderBy( obj => obj.Order );

            foreach( var metaAttribute in metaAttributes ) {
                PropertyMeta meta = PropertyMetaDatabase.GetMetaForAttribute( metaAttribute.GetType() );
                if( meta != null )
                    meta.ApplyPropertyMeta( _serializedPropertiesByFieldName[field.Name], metaAttribute );
            }
        }

        private void DrawField(FieldInfo field ) {
            EditorGUI.BeginChangeCheck();
            PropertyDrawer drawer = GetPropertyDrawerForField( field );
            if( drawer != null )
                drawer.DrawProperty( _serializedPropertiesByFieldName[field.Name] );
            else
                EditorDrawUtility.DrawPropertyField( _serializedPropertiesByFieldName[field.Name] );

            if( EditorGUI.EndChangeCheck() ) {
                IEnumerable<OnValueChangedAttribute> onValueChangedAttributes = field.GetCustomAttributes<OnValueChangedAttribute>( true );
                foreach( var onValueChanedAttribute in onValueChangedAttributes ) {
                    PropertyMeta meta = PropertyMetaDatabase.GetMetaForAttribute( onValueChanedAttribute.GetType() );
                    if( meta != null )
                        meta.ApplyPropertyMeta( _serializedPropertiesByFieldName[field.Name], onValueChanedAttribute );
                }
            }
        }

        private PropertyDrawer GetPropertyDrawerForField(FieldInfo field ) {
            IEnumerable<DrawerAttribute> drawerAttributes = field.GetCustomAttributes<DrawerAttribute>( true );
            if (drawerAttributes.Count() > 0 ) {
                PropertyDrawer drawer = PropertyDrawerDatabase.GetDrawerForAttribute( drawerAttributes.ElementAt( 0 ).GetType() );
                return drawer;
            }

            return null;
        }

        private PropertyGrouper GetPropertyGrouperForField(FieldInfo field ) {
            IEnumerable<GroupAttribute> groupAttributes = field.GetCustomAttributes<GroupAttribute>( true );
            if (groupAttributes.Count() > 0 ) {
                PropertyGrouper grouper = PropertyGrouperDatabase.GetGrouperForAttribute( groupAttributes.ElementAt( 0 ).GetType() );
                return grouper;
            }

            return null;
        }

        private PropertyDrawCondition GetPropertyDrawConditionForField(FieldInfo field) {
            IEnumerable<DrawConditionAttribute> drawConditionAttributes = field.GetCustomAttributes<DrawConditionAttribute>( true );
            if( drawConditionAttributes.Count() > 0 ) {
               PropertyDrawCondition drawCondition = PropertyDrawConditionDatabase.GetDrawConditionForAttribute( drawConditionAttributes.ElementAt( 0 ).GetType() );
               return drawCondition;
            }

            return null;
        }
    }
}