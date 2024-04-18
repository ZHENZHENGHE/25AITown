using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public abstract class CompositeNode : Node
{
    // 有多个子节点构成的列表
    public List<Node> children = new List<Node>();
}
