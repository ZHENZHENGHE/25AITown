using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class BTree : EditorWindow
{
    NodeTreeViewer nodeTreeViewer;
    InspectorViewer inspectorViewer;
    private NodeTree tree;
    [MenuItem("Window/UI Toolkit/BTree %q")]
    public static void ShowExample()
    {
        BTree wnd = GetWindow<BTree>();
        wnd.titleContent = new GUIContent("BTree");
    }
    void Update()
    {
        Undo.RecordObject(this, "Change Objects");
    }
    void OnEnable()
    {
        Undo.undoRedoPerformed += Refresh;
    }

    public void OnDestroy()
    {
        Selection.selectionChanged -= OnSelectionChange;
    }

    private void Refresh()
    {
        rootVisualElement.Clear();
        Init();
        nodeTreeViewer.OnNodeSelected = inspectorViewer.UpdateSelection;
        nodeTreeViewer.OnNodeUnSelected = inspectorViewer.ClearSelection;
        if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
        {
            if (nodeTreeViewer != null)
            {
                nodeTreeViewer.PopulateView(tree);
            }
        }
    }

    public void CreateGUI()
    {
        Init();
    }

    void Init()
    {
        VisualElement root = rootVisualElement;
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/Tree/BTree.uxml");
        visualTree.CloneTree(root);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/Tree/BTree.uss");
        root.styleSheets.Add(styleSheet);
        // 将节点树视图添加到节点编辑器中
        nodeTreeViewer = root.Q<NodeTreeViewer>();
        // 将节属性面板视图添加到节点编辑器中
        inspectorViewer = root.Q<InspectorViewer>();
        Selection.selectionChanged += OnSelectionChange;
    }

    private void OnGUI()
    {
        Vector2 mousePosition = Event.current.mousePosition;
        if (nodeTreeViewer != null)
        {
            nodeTreeViewer.Pos = mousePosition;
        }
    }

    private void OnSelectionChange()
    {
        // 检测该选中对象中是否存在节点树
        NodeTree tree = Selection.activeObject as NodeTree;
        this.tree = tree;
        if (nodeTreeViewer != null)
        {
            nodeTreeViewer.OnNodeSelected = inspectorViewer.UpdateSelection;
            nodeTreeViewer.OnNodeUnSelected = inspectorViewer.ClearSelection;
        }

        // 判断如果选中对象不为节点树，则获取该对象下的节点树运行器中的节点树
        if (!tree)
        {
            if (Selection.activeGameObject)
            {
                DialogueRunner runner = Selection.activeGameObject.GetComponent<DialogueRunner>();
                if (runner)
                {
                    tree = runner.tree;
                }
            }
        }

        if (Application.isPlaying)
        {
            if (tree)
            {
                if (nodeTreeViewer != null)
                {
                    nodeTreeViewer.PopulateView(tree);
                }
            }
        }
        else
        {
            if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
            {
                if (nodeTreeViewer != null)
                {
                    nodeTreeViewer.PopulateView(tree);
                }
            }
        }
    }
}