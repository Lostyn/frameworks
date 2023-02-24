using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public static class NodeExtensions {

    public static IEnumerable<Node> OnlyNodes(this IEnumerable<GraphElement> elements) {
        var view = elements.Where( e => e is NodeView).Select( e => e as NodeView);
        return view.Select( e => e.node);
    }

}