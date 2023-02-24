using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fr.lostyn.inspector {
    public abstract class GroupAttribute : HYPAttribute {
        public string Name { get; private set; }

        public GroupAttribute(string name ) {
            Name = name;
        }
    }
}