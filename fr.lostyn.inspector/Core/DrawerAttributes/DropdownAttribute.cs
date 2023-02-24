using System;
using System.Collections;
using System.Collections.Generic;

namespace fr.lostyn.inspector {

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class DropdownAttribute : DrawerAttribute {
        public string ValuesFieldName { get; private set; }

        public DropdownAttribute(string valuesFieldName ) {
            ValuesFieldName = valuesFieldName;
        }
    }

    public interface IDropdownList : IEnumerable<KeyValuePair<string, object>> { }

    public class DropdownList<T> : IDropdownList {
        private List<KeyValuePair<string, object>> values;

        public DropdownList() {
            values = new List<KeyValuePair<string, object>>();
        }

        public void Add(string displayName, T value) {
            values.Add( new KeyValuePair<string, object>( displayName, value ) );
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() {
            return values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public static explicit operator DropdownList<object>(DropdownList<T> target ) {
            DropdownList<object> result = new DropdownList<object>();
            foreach( var kvp in target ) {
                result.Add( kvp.Key, kvp.Value );
            }

            return result;
        }
    }
}