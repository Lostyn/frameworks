using fr.lostyn.inspector;
using UnityEditor;

namespace fr.lostyneditor.inspector {
    [PropertyDrawer( typeof( DisableIfAttribute ) )]
    public class DisableIfPropertyDrawer : EnableIfPropertyDrawer {
    }
}