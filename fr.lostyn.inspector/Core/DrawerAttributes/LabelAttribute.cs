using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fr.lostyn.inspector {
    /// <summary>
    /// Override default inspector field label
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class LabelAttribute : DrawerAttribute {
       public string Label { get; private set; }

        public LabelAttribute(string label ) {
            Label = label;
        }
    }
}