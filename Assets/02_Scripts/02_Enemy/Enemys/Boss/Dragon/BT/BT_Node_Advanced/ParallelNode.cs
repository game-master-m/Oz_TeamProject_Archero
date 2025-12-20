using System.Collections.Generic;

// 패러렐 노드 (동시 실행)
public class ParallelNode : Node
{
    private List<Node> children;

    public ParallelNode(List<Node> nodes) => children = nodes;

    public override ENodeState Evaluate()
    {
        bool anyRunning = false;
        int successCount = 0;

        foreach (var child in children)
        {
            var childState = child.Evaluate();
            if (childState == ENodeState.Running) anyRunning = true;
            if (childState == ENodeState.Success) successCount++;
        }

        if (successCount == children.Count) return ENodeState.Success;
        return anyRunning ? ENodeState.Running : ENodeState.Failure;
    }

    public override void Abort()
    {
        foreach (var child in children) child.Abort();
        base.Abort();
    }
}