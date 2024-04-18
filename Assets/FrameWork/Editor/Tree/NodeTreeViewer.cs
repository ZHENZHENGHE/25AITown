using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using Edge = UnityEditor.Experimental.GraphView.Edge;

public class NodeTreeViewer : GraphView
{
    public Action<NodeView> OnNodeSelected;
    public Action<NodeView> OnNodeUnSelected;
    public static List<NodeView> SelectedNodeViews = new List<NodeView>();
    public static List<NodeView> SelectedCtrlNodeViews = new List<NodeView>();
    public Vector2 Pos;

    public new class UxmlFactory : UxmlFactory<NodeTreeViewer, GraphView.UxmlTraits>
    {
    }

    public static NodeTreeViewer NodeTree;
    NodeTree tree;
    private Vector2 graphMousePosition;

    public NodeTreeViewer()
    {
        NodeTree = this;
        Insert(0, new GridBackground());
        // 添加视图缩放
        this.AddManipulator(new ContentZoomer());
        // 添加视图拖拽
        this.AddManipulator(new ContentDragger());
        // 添加选中对象拖拽
        this.AddManipulator(new SelectionDragger());
        // 添加框选
        this.AddManipulator(new RectangleSelector());
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/Tree/NodeTreeViewer.uss");
        styleSheets.Add(styleSheet);
    }

    // NodeTreeViewer视图中添加右键节点创建栏
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        // 添加Node抽象类下的所有子类到右键创建栏中
        {
            var types = TypeCache.GetTypesDerivedFrom<Node>();

            foreach (var type in types)
            {
                if (type.Name == "CompositeNode" | type.Name == "SingleNode") continue;
                evt.menu.AppendAction($"{type.Name}", (a) =>
                {
                    var pos = a.eventInfo.mousePosition;
                    pos.y = pos.y - 100;
                    var windowMousePosition = viewport.ChangeCoordinatesTo(viewport.parent, pos);
                    graphMousePosition = contentViewContainer.WorldToLocal(windowMousePosition);
                    CreateNode(type);
                });
            }

            evt.menu.AppendAction("复制", (a) => { CtrlC(); });
            evt.menu.AppendAction("粘贴", (a) => { CtrlV(); });
        }
    }

    [MenuItem("Window/UI Toolkit/BTree/CtrlC _c")]
    public static void CtrlC()
    {
        SelectedCtrlNodeViews = SelectedNodeViews;
    }

    [MenuItem("Window/UI Toolkit/BTree/CtrlC", validate = true)]
    static bool CtrlCValidate()

    {
        return SelectedNodeViews.Count != 0;
    }

    [MenuItem("Window/UI Toolkit/BTree/CtrlV _V")]
    private static void V()
    {
        var type = typeof(NodeTreeViewer);
        // 获取方法信息
        var methods = type.GetMethods();
        foreach (var methodInfo in methods)
        {
            if (methodInfo.Name == "pp")
            {
                methodInfo.Invoke(NodeTree, null);
            }
        }
    }

    [MenuItem("Window/UI Toolkit/BTree/CtrlV", validate = true)]
    static bool CtrlvValidate()
    {
        return SelectedCtrlNodeViews.Count != 0;
    }

    public void pp()
    {
        var pos = Pos;
        Debug.Log(pos);
        foreach (var selectedCtrlNodeView in SelectedCtrlNodeViews)
        {
            pos.y = pos.y - 100;
            var windowMousePosition = viewport.ChangeCoordinatesTo(viewport.parent, pos);
            graphMousePosition = contentViewContainer.WorldToLocal(windowMousePosition);
            CreateNodeByCV(selectedCtrlNodeView.node);
        }
    }

    public void CtrlV()
    {
        var pos = Pos;
        //var pos = action.eventInfo.mousePosition;
        foreach (var selectedCtrlNodeView in SelectedCtrlNodeViews)
        {
            pos.y = pos.y - 100;
            var windowMousePosition = viewport.ChangeCoordinatesTo(viewport.parent, pos);
            graphMousePosition = contentViewContainer.WorldToLocal(windowMousePosition);
            CreateNodeByCV(selectedCtrlNodeView.node);
        }
    }

    void CreateNode(System.Type type)
    {
        // 创建运行时节点树上的对应类型节点
        Node node = tree.CreateNode(type);
        CreateNodeView(node);
    }

    void CreateNodeByCV(Node oldNode)
    {
        // 创建运行时节点树上的对应类型节点
        Node node = tree.CreateNode(oldNode.GetType());
        node.dialogueContent = oldNode.dialogueContent;
        node.bg = oldNode.bg;
        node.role = oldNode.role;
        node.description = oldNode.description;
        node.musicBg = oldNode.musicBg;
        node.musicFx = oldNode.musicFx;
        node.rolePosition = oldNode.rolePosition;
        node.roleEmo = oldNode.roleEmo;
        node.roleAnime = oldNode.roleAnime;
        CreateNodeView(node);
    }

    void CreateNodeView(Node node)
    {
        // 创建节点UI
        NodeView nodeView = new NodeView(node);
        if (graphMousePosition != Vector2.zero)
        {
            nodeView.SetPosition(new Rect(graphMousePosition, Vector2.one));
            graphMousePosition = Vector2.zero;
        }

        // 节点创建成功后 让nodeView.OnNodeSelected与当前节点树上的OnNodeSelected关联 让该节点属性显示在InspectorViewer上
        nodeView.OnNodeSelected = OnNodeSelected;
        nodeView.OnNodeUnSelected = OnNodeUnSelected;
        // 将对应节点UI添加到节点树视图上
        AddElement(nodeView);
    }

    // 只要节点树视图发生改变就会触发OnGraphViewChanged方法
    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        // 对所有删除进行遍历记录 只要视图内有元素删除进行判断
        if (graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(elem =>
            {
                // 找到节点树视图中删除的NodeView
                NodeView nodeView = elem as NodeView;
                if (nodeView != null)
                {
                    // 并将该NodeView所关联的运行时节点删除
                    tree.DeleteeNode(nodeView.node);
                }

                Edge edge = elem as Edge;
                if (edge != null)
                {
                    var parentView = edge.output.node as NodeView;
                    var childView = edge.input.node as NodeView;
                    tree.RemoveChild(parentView?.node, childView?.node);
                }
            });
        }

        if (graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                var parentView = edge.output.node as NodeView;
                var childView = edge.input.node as NodeView;
                tree.AddChild(parentView?.node, childView?.node);
            });
        }
       
        return graphViewChange;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList()
            .Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node)
            .ToList();
    }

    internal void PopulateView(NodeTree tree)
    {
        this.tree = tree;
        // 在节点树视图重新绘制之前需要取消视图变更方法OnGraphViewChanged的订阅
        // 以防止视图变更记录方法中的信息是上一个节点树的变更信息
        graphViewChanged -= OnGraphViewChanged;
        // 清除之前渲染的graphElements图层元素
        DeleteElements(graphElements);
        // 在清除节点树视图所有的元素之后重新订阅视图变更方法OnGraphViewChanged
        graphViewChanged += OnGraphViewChanged;
        tree.nodes.ForEach(n => CreateNodeView(n));
        tree.nodes.ForEach(n =>
        {
            if (n.name == "NormalDialogue")
            {
                var parentView = FindNodeView(n);
                if (tree.GetChild(n))
                {
                    var childView = FindNodeView(tree.GetChild(n));
                    Edge edge = parentView.output.ConnectTo(childView.input);
                    AddElement(edge);
                }
            }
            else
            {
                if (tree.GetChildren(n).Count > 0)
                {
                    tree.GetChildren(n).ForEach(c =>
                    {
                        var parentView = FindNodeView(n);
                        var childView = FindNodeView(c);
                        Edge edge = parentView.output.ConnectTo(childView.input);
                        AddElement(edge);
                    });
                }
            }
        });
    }

    private NodeView FindNodeView(Node node)
    {
        return GetNodeByGuid(node.guid) as NodeView;
    }
}