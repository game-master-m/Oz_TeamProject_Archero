using System.Collections.Generic;

// 고급 시퀀스 (Memory 기능 지원)
public class SequenceNode : Node
{
    private List<Node> mChildren;
    private int mCurrentIndex = 0;
    private bool bUseMemory;

    public SequenceNode(List<Node> nodes, bool useMemory = true)
    {
        mChildren = nodes;
        this.bUseMemory = useMemory;
    }

    public override ENodeState Evaluate()
    {
        // 메모리 사용 여부에 따라 시작 인덱스 결정
        int start = bUseMemory ? mCurrentIndex : 0;

        for (int i = start; i < mChildren.Count; i++)
        {
            mCurrentIndex = i;
            var childState = mChildren[i].Evaluate();

            switch (childState)
            {
                case ENodeState.Running:
                    return ENodeState.Running;
                case ENodeState.Failure:
                    mCurrentIndex = 0;
                    return ENodeState.Failure;
                case ENodeState.Success:
                    continue;
            }
        }

        mCurrentIndex = 0;
        return ENodeState.Success;
    }

    public override void Abort()
    {
        if (mCurrentIndex < mChildren.Count)
            mChildren[mCurrentIndex].Abort();

        mCurrentIndex = 0;
        base.Abort();
    }
}