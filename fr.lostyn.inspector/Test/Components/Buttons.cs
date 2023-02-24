using fr.lostyn.inspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fr.lostyntest.inspector {
    public class Buttons : MonoBehaviour
    {
        [Button]
        public void Method() {
            Debug.Log( "Method" );
        }

        [Button("Button Text")]
        public void OtherMethod() {
            Debug.Log( "OtherMethod" );
        }
    }
}