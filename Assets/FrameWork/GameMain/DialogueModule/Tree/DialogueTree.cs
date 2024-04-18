using UnityEngine;

[CreateAssetMenu()]
public class DialogueTree : NodeTree{
    public override void OnTreeStart(){
        base.OnTreeStart();
        runningNode = nodes[0];//todo
        runningNode.state = Node.State.Running;
    }

    public int id;//章节id
}