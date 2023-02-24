using fr.lostyn.inspector;
using UnityEngine;

namespace fr.lostyntest.inspector {
    public class ShowAssetPreview : MonoBehaviour {
        
        [ShowAssetPreview]
        public Sprite sprite;

        [ShowAssetPreview(96, 96)]
        public GameObject prefab;
    }
}