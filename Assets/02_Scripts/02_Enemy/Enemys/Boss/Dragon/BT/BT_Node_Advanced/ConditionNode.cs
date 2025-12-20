public class ConditionNode : Node
{
    private System.Func<bool> condition;

    public ConditionNode(System.Func<bool> condition) => this.condition = condition;

    public override ENodeState Evaluate()
    {
        return condition() ? ENodeState.Success : ENodeState.Failure;
    }
}