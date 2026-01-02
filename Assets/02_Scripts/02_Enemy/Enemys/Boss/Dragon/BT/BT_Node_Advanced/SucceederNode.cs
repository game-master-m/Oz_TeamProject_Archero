public class SucceederNode : Node
{
    private Node child;
    public SucceederNode(Node node) => child = node;

    public override ENodeState Evaluate()
    {
        var childState = child.Evaluate();
        return childState == ENodeState.Running ? ENodeState.Running : ENodeState.Success;
    }
}