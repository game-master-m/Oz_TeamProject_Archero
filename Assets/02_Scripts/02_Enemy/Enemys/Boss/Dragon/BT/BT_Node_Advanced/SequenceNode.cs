using System.Collections.Generic;

// 고급 시퀀스 (Memory 기능 지원)
public class SequenceNode : Node
{
    private List<Node> children;
    private int currentIndex = 0;
    private bool useMemory;

    public SequenceNode(List<Node> nodes, bool useMemory = true)
    {
        children = nodes;
        this.useMemory = useMemory;
    }

    public override ENodeState Evaluate()
    {
        // 메모리 사용 여부에 따라 시작 인덱스 결정
        int start = useMemory ? currentIndex : 0;

        for (int i = start; i < children.Count; i++)
        {
            currentIndex = i;
            var childState = children[i].Evaluate();

            switch (childState)
            {
                case ENodeState.Running:
                    return ENodeState.Running;
                case ENodeState.Failure:
                    currentIndex = 0;
                    return ENodeState.Failure;
                case ENodeState.Success:
                    currentIndex++;
                    if (currentIndex >= children.Count)
                    {
                        currentIndex = 0;
                        return ENodeState.Success;
                    }
                    return ENodeState.Running;
            }
        }

        currentIndex = 0;
        return ENodeState.Success;
    }

    public override void Abort()
    {
        if (currentIndex < children.Count)
            children[currentIndex].Abort();

        currentIndex = 0;
        base.Abort();
    }
}