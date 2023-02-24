using System.Collections.Generic;
using fr.lostyn.inspector;
using UnityEngine;

namespace fr.lostyntest.inspector {
    [System.Serializable]
    public struct SomeStruct {
        public int Int;
        public float Float;
        public Vector3 Vector;
    }

    public class ReorderableLists : MonoBehaviour {
        
        [BoxGroup("Reorderable Lists")]
        [ReorderableList]
        public int[] intArray;

        [BoxGroup("Reorderable Lists")]
        [ReorderableList]
        public List<Vector3> vertorList;

        [BoxGroup("Reorderable Lists")]
        [ReorderableList]
        public List<SomeStruct> structList;
    }
}