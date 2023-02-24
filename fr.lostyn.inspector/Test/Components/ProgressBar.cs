using fr.lostyn.inspector;
using UnityEngine;

namespace fr.lostyntest.inspector {
    public class ProgressBar : MonoBehaviour {
        
        [ProgressBar("Health", 100, ProgressBarColor.Orange)]
        public float health = 50;
    }
}