using fr.lostyn.inspector;
using System.Collections.Generic;
using UnityEngine;

namespace fr.lostyntest.inspector {
    public class Dropdowns : MonoBehaviour {
        private int[] intValues = new int[] { 1, 2, 3 };
        private List<string> stringValues = new List<string>() { "A", "B", "C" };
        private DropdownList<Vector3> vectorValues = new DropdownList<Vector3>() {
            {"Right", Vector3.right },
            {"Up", Vector3.up },
            {"Forward", Vector3.forward }
        };

        [Dropdown("intValues")]
        public int intValue;

        [Dropdown( "stringValues" )]
        public string stringValue;

        [Dropdown( "vectorValues" )]
        public Vector3 vectorValue;
    }
}