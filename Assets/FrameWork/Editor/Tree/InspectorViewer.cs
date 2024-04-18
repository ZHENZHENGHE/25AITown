using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine;

public class InspectorViewer : VisualElement
{
    public new class UxmlFactory : UxmlFactory<InspectorViewer,VisualElement.UxmlTraits>{}
    Editor editor;
    public InspectorViewer(){

    }
    internal void UpdateSelection(NodeView nodeView ){
        Clear();
        NodeTreeViewer.SelectedNodeViews.Add(nodeView);
        //Debug.Log(NodeTreeViewer.NodeTree.SelectedNodeViews.Count);
        UnityEngine.Object.DestroyImmediate(editor);
        editor = Editor.CreateEditor(nodeView.node);
        IMGUIContainer container = new IMGUIContainer(() => { 
            if(editor.target){
                editor.OnInspectorGUI();
                if (GUI.changed)
                {
                    nodeView.UpdateTitle();
                }
            }
        });
        Add(container);
    }  
    internal void ClearSelection(NodeView nodeView ){
        
        NodeTreeViewer.SelectedNodeViews.Clear();
        //Debug.Log(NodeTreeViewer.NodeTree.SelectedNodeViews.Count);
    }  
}