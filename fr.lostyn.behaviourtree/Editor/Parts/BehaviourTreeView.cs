using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using System;
using System.Linq;

public class BehaviourTreeView : GraphView, IDisposable
{
    public new class UxmlFactory: UxmlFactory<BehaviourTreeView, GraphView.UxmlTraits> { }

    bool frame = false;

    public Action<NodeView> OnNodeSelected;
    public Action OnNodeDeleted;

    protected override bool canPaste => !EditorApplication.isPlaying;

    SerializedBehaviourTree serializer;

    public BehaviourTreeView() { 
        Insert(0, new GridBackground());
        
        var contentZoomer = new ContentZoomer();
        contentZoomer.minScale = 0.05f;

        this.AddManipulator( contentZoomer );
        this.AddManipulator( new ContentDragger());
        this.AddManipulator( new DoubleClickSelection() );
        this.AddManipulator( new SelectionDragger());
        this.AddManipulator( new RectangleSelector());
    
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/fr.losytn.behaviourtree/Editor/UIBuilder/BehaviourTreeEditor.uss");
        styleSheets.Add(styleSheet);
        serializeGraphElements += SerializeForCopy;
        unserializeAndPaste += UnserializeAndPaste;
    }

    public void Dispose()
    {
        graphViewChanged -= OnGraphViewChanged;
        serializeGraphElements -= SerializeForCopy;
        unserializeAndPaste -= UnserializeAndPaste;
    }

    NodeView FindNodeView(Node node) {
        return GetNodeByGuid(node.guid) as NodeView;
    }

    public void ClearView() {
        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements.ToList());
        graphViewChanged += OnGraphViewChanged;
    }

    internal void PopulateView(SerializedBehaviourTree tree)
    {
        serializer = tree;
        ClearView();

        Debug.Assert(serializer.tree.rootNode != null);

        // creates node view
        serializer.tree.nodes.ForEach( n => CreateNodeView(n) );

        // create edges
        serializer.tree.nodes.ForEach( n => {
            var children = serializer.tree.GetChildren(n);
            children.ForEach( c => {
                NodeView parentView = FindNodeView(n);
                NodeView childView = FindNodeView(c);

                Edge edge = parentView.output.ConnectTo(childView.input);
                AddElement(edge);
            });
        });

        frame = true;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endPort => 
            endPort.direction != startPort.direction && 
            endPort.node != startPort.node
        ).ToList();
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        if (graphViewChange.elementsToRemove != null) {
            graphViewChange.elementsToRemove.ForEach( elem => {
                NodeView nodeView = elem as NodeView;
                if (nodeView != null) {
                    serializer.DeleteNode(nodeView.node);
                    OnNodeDeleted();
                }

                Edge edge = elem as Edge;
                if (edge != null) {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;

                    serializer.RemoveChild(parentView.node, childView.node);
                }
            });
        }

        if (graphViewChange.edgesToCreate != null) {
            graphViewChange.edgesToCreate.ForEach( edge => {
                NodeView parentView = edge.output.node as NodeView;
                NodeView childView = edge.input.node as NodeView;

                serializer.AddChild(parentView.node, childView.node);
            });
        }

        return graphViewChange;
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        Vector2 nodePosition = this.ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);

        var selectedNodes = this.selection.Where( o => o is NodeView).Select(o => (NodeView) o).ToList();
        if (selectedNodes.Count > 0) {
            evt.menu.AppendAction($"Move into subtree", a => {
                OnNodeSelected?.Invoke(null);
                MoveIntoSubtree(selectedNodes, nodePosition, true);
            });
            evt.menu.AppendAction($"Duplicate into subtree", a => {
                OnNodeSelected?.Invoke(null);
                MoveIntoSubtree(selectedNodes, nodePosition, false);
            });
        }
        evt.menu.AppendSeparator();

        {
            var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
            foreach( var type in types) {
                evt.menu.AppendAction($"[Action]/{type.Name}", a => CreateNode(type, nodePosition));
            }
        }
        {
            var types = TypeCache.GetTypesDerivedFrom<LogicalNode>();
            foreach( var type in types) {
                evt.menu.AppendAction($"[Logic]/{type.Name}", a => CreateNode(type, nodePosition));
            }
        }
        {
            var types = TypeCache.GetTypesDerivedFrom<IfNode>();
            foreach( var type in types) {
                evt.menu.AppendAction($"[Condition]/{type.Name}", a => CreateNode(type, nodePosition));
            }
        }
    }

    NodeView CreateNode(System.Type type, Vector2 position) {
        Node node = serializer.CreateNode(type, position);
        return CreateNodeView(node);
    }

    NodeView CreateNodeView(Node node) {
        NodeView nodeView = new NodeView(serializer, node);
        nodeView.OnNodeSelected = OnNodeSelected;
        AddElement(nodeView);
        return nodeView;
    }

    public void UpdateNodeStates() {
        if (frame) {
            FrameAll();
            frame = false;
        }
        
        nodes.ForEach( n => {
            NodeView view = n as NodeView;
            view.UpdateState();
        });
    }

    public void DoubleClickNodeView(NodeView nodeView) {
        var subtreeNode = nodeView.node as SubtreeNode;
        if( subtreeNode != null && subtreeNode.subtree != null) {
            BehaviourTreeEditor.OpenWindow(subtreeNode.subtree);
        }
    }

    private void MoveIntoSubtree(List<NodeView> selectedNodeViews, Vector2 position, bool deleteAfter)
    {
        var path = AssetDatabase.GetAssetPath(serializer.tree);
        var pathTry = 1;
        var subtreePath = "";
        var trees = TreeEditorUtility.GetAssetPaths<BehaviourTree>();
        do {
            subtreePath = path.Replace(".asset", $"_subtree{pathTry}.asset");
            pathTry++;
        } while (trees.Contains(subtreePath));
        
        var subtree = ScriptableObject.CreateInstance<BehaviourTree>();
        AssetDatabase.CreateAsset(subtree, subtreePath);
        AssetDatabase.SaveAssets();
        
        List<Node> selectedNodes = selectedNodeViews.Select( n => n.node ).ToList();
        serializer.MoveIntoSubtree(subtree, selectedNodes);
        
        if (deleteAfter) {
            selectedNodeViews.ForEach( nodeView => nodeView.DisconnectAll() );
            DeleteElements(selectedNodeViews);
        }

        var subtreeNode = CreateNode(typeof(SubtreeNode), position);
        serializer.FindNode(serializer.Nodes, subtreeNode.node).Find("subtree").objectReferenceValue = subtree;
        serializer.Save();
    }

    private string SerializeForCopy(IEnumerable<GraphElement> elements)
    {
        string serializedString = CopyPasteSerializer.Serialize(elements);
        return serializedString;
    }

    private void UnserializeAndPaste(string operationName, string data)
    {
        var newNodes = CopyPasteSerializer.Deserialize(data);
        this.ClearSelection();
        newNodes.ForEach( node => {
            node.position = node.position + Vector2.one * 10;
            serializer.InjectNode(node);
            AddToSelection(CreateNodeView(node));
        });
    }
}
