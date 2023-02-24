using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Callbacks;
using System;

public class BehaviourTreeEditor : EditorWindow
{
    SerializedObject treeObject;

    
    BehaviourTreeView treeView;
    InspectorView inspectorView;
    BlackboardView blackboardView;
    ToolbarMenu toolbarMenu;
    SerializedBehaviourTree serializer;


    Label titleLabel;


    [MenuItem("Tools/BehaviourTreeEditor ...")]
    public static void OpenWindow()
    {
        BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviourTreeEditor");
    }

    public static void OpenWindow(BehaviourTree tree) {
        BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviourTreeEditor");
        wnd.SelectTree(tree);
    }

    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceId, int line) {
        if (Selection.activeObject is BehaviourTree) {
            OpenWindow(Selection.activeObject as BehaviourTree);
            return true;
        }

        return false;
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/fr.lostyn.behaviourtree/Editor/UIBuilder/BehaviourTreeEditor.uxml");
        visualTree.CloneTree(root);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/fr.lostyn.behaviourtree/Editor/UIBuilder/BehaviourTreeEditor.uss");
        root.styleSheets.Add(styleSheet);

        treeView = root.Q<BehaviourTreeView>();
        inspectorView = root.Q<InspectorView>();
        blackboardView = root.Q<BlackboardView>();
        toolbarMenu = root.Q<ToolbarMenu>();
        titleLabel = root.Q<Label>("title-label");

        // Toolbar assets menu
        toolbarMenu.RegisterCallback<MouseEnterEvent>((evt) => {

            // Refresh the menu options just before it's opened (on mouse enter)
            toolbarMenu.menu.MenuItems().Clear();
            var behaviourTrees = TreeEditorUtility.GetAssetPaths<BehaviourTree>();
            behaviourTrees.ForEach( path => {
                var fileName = System.IO.Path.GetFileName(path);
                toolbarMenu.menu.AppendAction($"{fileName}", a => {
                    var tree = AssetDatabase.LoadAssetAtPath<BehaviourTree>(path);
                    SelectTree(tree);
                });
            });
        });

        treeView.OnNodeSelected = OnNodeSelectionChanged;
        treeView.OnNodeDeleted = () => OnNodeSelectionChanged(null);
        
        if (serializer != null)
            SelectTree(serializer.tree);
    }

    void OnEnable() {
        EditorApplication.playModeStateChanged -= OnPlayModeStataChanged;
        EditorApplication.playModeStateChanged += OnPlayModeStataChanged;
    }

    void OnDisable() {
        EditorApplication.playModeStateChanged -= OnPlayModeStataChanged;
    }

    private void OnPlayModeStataChanged(PlayModeStateChange obj)
    {
        switch(obj) {
            case PlayModeStateChange.EnteredEditMode:
                EditorApplication.delayCall += OnSelectionChange; 
                break;
            case PlayModeStateChange.ExitingEditMode:
                break;
            case PlayModeStateChange.EnteredPlayMode:
                EditorApplication.delayCall += OnSelectionChange; 
                break;
            case PlayModeStateChange.ExitingPlayMode:
                break;
        }
    }

    private void OnSelectionChange() {
        if (Selection.activeGameObject) {
            BehaviourTreeRunner runner = Selection.activeGameObject.GetComponent<BehaviourTreeRunner>();
            if (runner) {
                SelectTree(runner.tree);
            }
        }
    }

    void SelectTree(BehaviourTree newTree) {
        OnNodeSelectionChanged(null);
        if (!newTree) {
            ClearSelection();
            return;
        }

        serializer = new SerializedBehaviourTree(newTree);

        if (titleLabel != null) {
            string path = AssetDatabase.GetAssetPath(serializer.tree);
            if (path == "") {
                path = serializer.tree.name;
            }
            titleLabel.text = $"TreeView ({path})";
        }

        treeView.PopulateView(serializer);
        blackboardView.Bind(serializer);

    }

    void ClearSelection() {
        serializer = null;
        treeView.ClearView();
    }

    void OnNodeSelectionChanged(NodeView node) {
        inspectorView.UpdateSelection(serializer, node);
    }

    void OnInspectorUpdate() {
        treeView?.UpdateNodeStates();
    }
}