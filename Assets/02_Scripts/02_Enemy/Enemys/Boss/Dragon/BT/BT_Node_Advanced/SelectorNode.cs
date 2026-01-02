using System.Collections.Generic;

// 고급 셀렉터 (Lower Priority Abort 지원)
public class SelectorNode : Node
{
    private List<Node> children;
    private int lastRunningIndex = -1;

    public SelectorNode(List<Node> nodes) => children = nodes;

    public override ENodeState Evaluate()
    {
        for (int i = 0; i < children.Count; i++)
        {
            var childState = children[i].Evaluate();

            // 실패가 아닌 상태(Success나 Running)를 만나면
            if (childState != ENodeState.Failure)
            {
                // [조건부 중단 로직]
                // 이전에 실행 중이던 노드보다 현재 선택된 노드의 우선순위(Index)가 더 높다면
                if (lastRunningIndex != -1 && lastRunningIndex > i)
                {
                    children[lastRunningIndex].Abort(); // 낮은 순위 노드 강제 중단
                }

                lastRunningIndex = (childState == ENodeState.Running) ? i : -1;
                return childState;
            }
        }
        if (lastRunningIndex != -1)
        {
            children[lastRunningIndex].Abort();
            lastRunningIndex = -1;
        }
        return ENodeState.Failure;
    }
    public override void Abort()
    {
        base.Abort();

        if (lastRunningIndex != -1 && lastRunningIndex < children.Count)
        {
            children[lastRunningIndex].Abort();
        }

        lastRunningIndex = -1;

    }
}
