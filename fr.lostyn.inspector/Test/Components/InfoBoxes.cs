using UnityEngine;
using fr.lostyn.inspector;

namespace fr.lostyntest.inspector {
    public class InfoBoxes : MonoBehaviour {
        [InfoBox( "Normal", InfoBoxType.Normal )]
        public int int1;

        [InfoBox( "Warning", InfoBoxType.Warning )]
        public int int2;

        [InfoBox( "Error", InfoBoxType.Error )]
        public int int3;
    }
}