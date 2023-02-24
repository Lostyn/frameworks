using UnityEngine;
using fr.lostyn.inspector;
using UnityEngine.AI;

namespace fr.lostyntest.inspector {
    public class Label : MonoBehaviour {
        
        [Label("A Short Name")]
        public string aMoreSpecificName;

        [Label("RGB")]
        public Vector3 vectorXYZ;

        [Label("Agent")]
        public NavMeshAgent navMeshAgent;
        
        [Label("Ints")]
        public int[] arrayOfInts;

        [Label("Custom Class")]
        public MyClassExemple myClass;

        [System.Serializable]
        public class MyClassExemple
        {
            public int aInt;
            public string aString;
        }
    }
}