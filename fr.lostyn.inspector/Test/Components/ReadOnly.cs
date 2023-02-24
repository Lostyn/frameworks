using fr.lostyn.inspector;
using UnityEngine;

namespace fr.lostyntest.inspector {
    public class ReadOnly : MonoBehaviour {
        
        [ReadOnly]
        public int readOnlyInt = 5;
    }
}