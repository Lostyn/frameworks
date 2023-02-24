using fr.lostyn.inspector;
using UnityEngine;

namespace fr.lostyntest.inspector {
    public class OnValueChanged : MonoBehaviour {
        [OnValueChanged("OnValueChangedMethod")]
        public int onValueChanged;

        public void OnValueChangedMethod(){
            Debug.Log(onValueChanged);
        }
    }
}