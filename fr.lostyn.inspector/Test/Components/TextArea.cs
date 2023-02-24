using fr.lostyn.inspector;
using UnityEngine;

namespace fr.lostyntest.inspector {
    public class TextArea : MonoBehaviour {
        
        [ResizableTextArea]
        public string textArea;
    }
}