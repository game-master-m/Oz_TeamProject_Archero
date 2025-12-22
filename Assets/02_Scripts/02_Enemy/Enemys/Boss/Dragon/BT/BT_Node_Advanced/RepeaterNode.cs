public class RepeaterNode : Node
{
    private Node child;
    public RepeaterNode(Node node) => child = node;

    public override ENodeState Evaluate()
    {
        child.Evaluate();
        return ENodeState.Running; // 무한 반복
    }

    public override void Abort()
    {
        if (child != null)
        {
            child.Abort();
        }
        base.Abort();
    }
}

